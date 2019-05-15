using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));
            CreateOtherRounds(model, rounds);
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int value = 2;

            while (value < teamCount)
            {
                output += 1;
                value *= 2;
            }

            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;

            return output;
        }

        private static List<MatchUpModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchUpModel> output = new List<MatchUpModel>();
            MatchUpModel current = new MatchUpModel();

            foreach (TeamModel team in teams)
            {
                current.Entries.Add(new MatchUpEntryModel { TeamCompeting = team });
                if (byes > 0 || current.Entries.Count > 1)
                {
                    current.MatchupRound = 1;
                    output.Add(current);
                    current = new MatchUpModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            List<MatchUpModel> previousRound = model.Rounds[0];
            List<MatchUpModel> currentRound = new List<MatchUpModel>();
            MatchUpModel currentMatchup = new MatchUpModel();

            while (round <= rounds)
            {
                foreach (MatchUpModel match in previousRound)
                {
                    currentMatchup.Entries.Add(new MatchUpEntryModel { ParentMatchup = match });

                    if (currentMatchup.Entries.Count > 1)
                    {
                        currentMatchup.MatchupRound = round;
                        currentRound.Add(currentMatchup);
                        currentMatchup = new MatchUpModel();
                    }
                }

                model.Rounds.Add(currentRound);
                previousRound = currentRound;
                currentRound = new List<MatchUpModel>();
                round += 1;
            }
        }

        public static int CheckCurrentRound(this TournamentModel model)
        {
            int output = 1;

            foreach (List<MatchUpModel> round in model.Rounds)
            {
                if (round.All(x => x.Winner != null))
                {
                    output += 1;
                }
                else
                {
                    return output;
                }
            }

            CompleteTournament(model);

            return output - 1;
        }

        private static void AdvanceWinners(List<MatchUpModel> models, TournamentModel tournament)
        {
            foreach (MatchUpModel m in models)
            {
                foreach (List<MatchUpModel> round in tournament.Rounds)
                {
                    foreach (MatchUpModel rm in round)
                    {
                        foreach (MatchUpEntryModel me in rm.Entries)
                        {
                            if (me.ParentMatchup != null)
                            {
                                if (me.ParentMatchup.Id == m.Id)
                                {
                                    me.TeamCompeting = m.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void MarkWinnersInMatchups(List<MatchUpModel> models)
        {
            string greaterWins = ConfigurationManager.AppSettings["greaterWins"];

            foreach (MatchUpModel m in models)
            {
                // Checks for bye week entry
                if (m.Entries.Count == 1)
                {
                    m.Winner = m.Entries[0].TeamCompeting;
                    continue;
                }

                // 0 means false (lower score wins).
                if (greaterWins == "0")
                {
                    if (m.Entries[0].Score < m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[1].Score < m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allow ties in this application.");
                    }
                }
                // 1 means true (higher score wins).
                else
                {
                    if (m.Entries[0].Score > m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[1].Score > m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allow ties in this application.");
                    }
                }
            }

        }

        public static void UpdateTournamentResults(TournamentModel model)
        {
            int startingRound = model.CheckCurrentRound();

            List<MatchUpModel> toScore = new List<MatchUpModel>();
                
            foreach (List<MatchUpModel> round in model.Rounds)
            {
                foreach (MatchUpModel rm in round)
                {
                    if (rm.Winner == null && (rm.Entries.Any(x => x.Score != 0) || rm.Entries.Count == 1))
                    {
                        toScore.Add(rm);
                    }
                }
            }

            MarkWinnersInMatchups(toScore);

            AdvanceWinners(toScore, model);

            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));

            int endingRound = model.CheckCurrentRound();

            if (endingRound > startingRound)
            {
                model.AlertUsersToNewRound();
            }
        }

        public static void AlertUsersToNewRound(this TournamentModel model)
        {
            // Emailing feature currently disabled.



            //int currentRoundNumber = model.CheckCurrentRound();
            //List<MatchUpModel> currentRound = model.Rounds.Where(x => x.First().MatchupRound == currentRoundNumber).First();

            //foreach (MatchUpModel matchup in currentRound)
            //{
            //    foreach (MatchUpEntryModel me in matchup.Entries)
            //    {
            //        foreach (PersonModel p in me.TeamCompeting.TeamMembers)
            //        {
            //            AlertPersonToNewRound(p, me.TeamCompeting.TeamName, matchup.Entries.Where(x => x.TeamCompeting != me.TeamCompeting).FirstOrDefault());
            //        }
            //    }
            //}
        }

        private static void AlertPersonToNewRound(PersonModel p, string teamName, MatchUpEntryModel competitor)
        {
            // Emailing feature currently disabled.



            //if (p.EmailAddress.Length == 0)
            //{
            //    return;
            //}

            //string to = "";
            //string subject = "";
            //StringBuilder body = new StringBuilder();

            //if (competitor != null)
            //{
            //    subject = $"You have a new matchup with {competitor.TeamCompeting.TeamName }.";

            //    body.AppendLine("<h1>You have a new matchup</h1>");
            //    body.Append("<strong>Competitor: </strong>");
            //    body.Append(competitor.TeamCompeting.TeamName);
            //    body.AppendLine();
            //    body.AppendLine("<br />");
            //    body.AppendLine("Good luck!");
            //    body.AppendLine("<br />");
            //    body.AppendLine("~Tournament Tracker");

            //}
            //else
            //{
            //    subject = "You have a bye week this round.";

            //    body.AppendLine("Enjoy your round off!");
            //    body.AppendLine("<br />");
            //    body.AppendLine("~Tournament Tracker");
            //}

            //to = p.EmailAddress;

            //EmailLogic.SendEmail(to, subject, body.ToString());
        }

        private static decimal CalculatePrizePayout(this PrizeModel prize, decimal totalIncome)
        {
            decimal output = 0;

            if (prize.PrizeAmount > 0)
            {
                output = prize.PrizeAmount;
            }
            else
            {
                output = Decimal.Multiply(totalIncome, Convert.ToDecimal(prize.PrizePercentage / 100));
            }

            return output;
        }

        public static string winnerTeam;
        public static string runnerUpTeam;
        public static string tournamentName;
        public static string prizeWinner;
        public static string prizeRunnerUp;

        private static void CompleteTournament(TournamentModel model)
        {
            GlobalConfig.Connection.CompleteTournament(model);
            TeamModel winners = model.Rounds.Last().First().Winner;
            TeamModel runnerUp = model.Rounds.Last().First().Entries.Where(x => x.TeamCompeting != winners).First().TeamCompeting;

            decimal winnerPrize = 0;
            decimal runnerUpPrize = 0;

            if (model.Prizes.Count > 0)
            {
                decimal totalIncome = model.EntryFee * model.EnteredTeams.Count;

                PrizeModel firstPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 1).FirstOrDefault();
                PrizeModel secondPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 2).FirstOrDefault();


                if (firstPlacePrize != null)
                {
                    winnerPrize = firstPlacePrize.CalculatePrizePayout(totalIncome);
                    prizeWinner = winnerPrize.ToString();
                }

                if (secondPlacePrize != null)
                {
                    runnerUpPrize = secondPlacePrize.CalculatePrizePayout(totalIncome);
                    prizeRunnerUp = runnerUpPrize.ToString();
                }
            }

            winnerTeam = winners.TeamName;
            runnerUpTeam = runnerUp.TeamName;
            tournamentName = model.TournamentName;

            // Emailing feature currently disabled.



            // Send email to all tournament participants.
            //string subject = "";
            //StringBuilder body = new StringBuilder();

            //subject = $"{ winners.TeamName } has won the { model.TournamentName } tournament!";

            //body.AppendLine("<p>Congratulations to our winner on a great tournament.</p>");
            //body.AppendLine("<br />");

            //if (winnerPrize > 0)
            //{
            //    body.AppendLine($"<p> { winners.TeamName } will receive ${ winnerPrize }</p>");
            //}

            //if (runnerUpPrize > 0)
            //{
            //    body.AppendLine($"<p> { runnerUp.TeamName } will receive ${ runnerUpPrize }</p>");
            //}

            //body.AppendLine("<p>Thanks for playing!</p>");
            //body.AppendLine("~Tournament Tracker");

            //List<string> bcc = new List<string>();

            //foreach (TeamModel t in model.EnteredTeams)
            //{
            //    foreach (PersonModel p in t.TeamMembers)
            //    {
            //        if (p.EmailAddress.Length > 0)
            //        {
            //            bcc.Add(p.EmailAddress);
            //        }
            //    }
            //}

            //EmailLogic.SendEmail(new List<string>(), bcc, subject, body.ToString());
            // Complete Tournament

            model.CompleteTournament();
        }
    }
}
