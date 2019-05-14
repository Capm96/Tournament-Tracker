using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            return $@"{Directory.GetCurrentDirectory()}\Data\{fileName}";
        }   

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModel(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                output.Add(p);
            }

            return output;
        }

        public static List<PersonModel> ConverToPersonModel(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellphoneNumber = cols[4];
                output.Add(p);
            }

            return output;
        }

        public static List<TeamModel> ConvertToTeamModel(this List<string> lines)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConverToPersonModel();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|');

                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }

                output.Add(t);
            }

            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModel(this List<string> lines)
        {
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModel();
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModel();
            List<MatchUpModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModel();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                // id = cols 0
                // TournamentName = 1
                // EntryFee = 2
                // EnteredTeams = 3
                // Prizes = 4
                // Rounds = 5

                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');
                foreach (string Id in teamIds)
                {
                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(Id)).First());
                }

                if (cols[4].Length > 0)
                {
                    string[] prizeIds = cols[4].Split('|');
                    foreach (string Id in prizeIds)
                    {
                        tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(Id)).First());
                    }
                }
                
                // Capture Rounds Information.
                string[] rounds = cols[5].Split('|');

                foreach (string round in rounds)
                {
                    string[] muText = round.Split('^');
                    List<MatchUpModel> mu = new List<MatchUpModel>();

                    foreach (string matchupModelTextId in muText)
                    {
                        mu.Add(matchups.Where(x => x.Id == int.Parse(matchupModelTextId)).First());
                    }

                    tm.Rounds.Add(mu);
                }

                output.Add(tm);
            }

            return output;
        }

        public static List<MatchUpEntryModel> ConvertToMatchupEntryModel(this List<string> lines)
        {
            List<MatchUpEntryModel> output = new List<MatchUpEntryModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchUpEntryModel me = new MatchUpEntryModel();
                me.Id = int.Parse(cols[0]);
                if (cols[1].Length == 0)
                {
                    me.TeamCompeting = null;
                }
                else
                {
                    me.TeamCompeting = LookupTeamById(int.Parse(cols[1]));
                }

                me.Score = double.Parse(cols[2]);

                int parentId = 0;
                if (int.TryParse(cols[3], out parentId))
                {
                    me.ParentMatchup = LookupMatchupById(parentId);
                }
                else
                {
                    me.ParentMatchup = null;
                }

                output.Add(me);
            }

            return output;
        }

        public static List<MatchUpModel> ConvertToMatchupModel(this List<string> lines)
        {
            List<MatchUpModel> output = new List<MatchUpModel>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchUpModel m = new MatchUpModel();
                m.Id = int.Parse(cols[0]);
                m.Entries = ConvertStringToMatchupEntryModels(cols[1]);
                if (cols[2].Length == 0)
                {
                    m.Winner = null;
                }
                else
                {
                    m.Winner = LookupTeamById(int.Parse(cols[2]));
                }
                m.MatchupRound = int.Parse(cols[3]);
                output.Add(m);
            }

            return output;
        }

        private static List<MatchUpEntryModel> ConvertStringToMatchupEntryModels(string input)
        {
            string[] ids = input.Split('|');
            List<MatchUpEntryModel> output = new List<MatchUpEntryModel>();
            List<string> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile();
            List<string> matchingEntries = new List<string>();

            foreach (string id in ids)
            {
                foreach (string entry in entries)
                {
                    string[] cols = entry.Split(',');

                    if (cols[0] == id)
                    {
                        matchingEntries.Add(entry);
                    }
                }
            }

            output = matchingEntries.ConvertToMatchupEntryModel();

            return output;
        }

        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            if (people.Count == 0)
            {
                return "";
            }

            foreach (PersonModel p in people)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel t in teams)
            {
                output += $"{ t.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupEntriesToString(List<MatchUpEntryModel> entries)
        {
            string output = "";

            if (entries.Count == 0)
            {
                return "";
            }

            foreach (MatchUpEntryModel me in entries)
            {
                output += $"{ me.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertPrizeListToString(List<PrizeModel> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }

            foreach (PrizeModel p in prizes)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertRoundListToString(List<List<MatchUpModel>> rounds)
        {
            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<MatchUpModel> r in rounds)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupListToString(List<MatchUpModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (MatchUpModel m in matchups)
            {
                output += $"{ m.Id }^";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static TeamModel LookupTeamById(int id) 
        {
            List<string> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile();

            foreach (string team in teams)
            {
                string[] cols = team.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingTeams = new List<string>();
                    matchingTeams.Add(team);
                    return matchingTeams.ConvertToTeamModel().First();
                }
            }

            return null;
        }

        private static MatchUpModel LookupMatchupById(int id) 
        {
            List<string> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile();

            foreach (string matchup in matchups)
            {
                string[] cols = matchup.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingMatchups = new List<string>();
                    matchingMatchups.Add(matchup);
                    return matchingMatchups.ConvertToMatchupModel().First();
                }
            }

            return null;
        }

        public static void UpdateMatchupToFile(this MatchUpModel matchup)
        {
            List<MatchUpModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModel();

            MatchUpModel oldMatchup = new MatchUpModel();

            foreach (MatchUpModel m in matchups)
            {
                if (m.Id == matchup.Id)
                {
                    oldMatchup = m;
                }
            }

            matchups.Remove(oldMatchup);

            matchups.Add(matchup);

            foreach (MatchUpEntryModel entry in matchup.Entries)
            {
                entry.UpdateEntryToFile();
            }

            List<string> lines = new List<string>();

            foreach (MatchUpModel m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }

                lines.Add($"{ m.Id },{ ConvertMatchupEntriesToString(m.Entries) },{ winner },{ m.MatchupRound }");
            }

            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        public static void UpdateEntryToFile(this MatchUpEntryModel entry)
        {
            List<MatchUpEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModel();
            MatchUpEntryModel oldEntry = new MatchUpEntryModel();

            foreach (MatchUpEntryModel e in entries)
            {
                if (e.Id == entry.Id)
                {
                    oldEntry = e;
                }
            }

            entries.Remove(oldEntry);

            entries.Add(entry);

            List<string> lines = new List<string>();

            foreach (MatchUpEntryModel e in entries)
            {
                string parent = "";
                if (e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }
                string teamCompeting = "";
                if (e.TeamCompeting != null)
                {
                    teamCompeting = e.TeamCompeting.Id.ToString();
                }
                lines.Add($"{ e.Id },{ teamCompeting },{ e.Score },{ parent }");
            }

            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        private static void SaveMatchupToFile(this MatchUpModel matchup)
        {
            List<MatchUpModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModel();

            int currentId = 1;

            if (matchups.Count > 0)
            {
                currentId = matchups.OrderByDescending(x => x.Id).First().Id + 1;
            }

            matchup.Id = currentId;

            matchups.Add(matchup);

            foreach (MatchUpEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile();
            }

            List<string> lines = new List<string>();

            foreach (MatchUpModel m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }

                lines.Add($"{ m.Id },{ ConvertMatchupEntriesToString(m.Entries) },{ winner },{ m.MatchupRound }");
            }

            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        public static void SaveEntryToFile(this MatchUpEntryModel entry)
        {
            List<MatchUpEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModel();

            int currentId = 1;

            if (entries.Count > 0)
            {
                currentId = entries.OrderByDescending(x => x.Id).First().Id + 1;
            }

            entry.Id = currentId;

            entries.Add(entry);

            List<string> lines = new List<string>();

            foreach (MatchUpEntryModel me in entries)
            {
                string parent = "";
                if (me.ParentMatchup != null)
                {
                    parent = me.ParentMatchup.Id.ToString();
                }
                string teamCompeting = "";
                if (me.TeamCompeting != null)
                {
                    teamCompeting = me.TeamCompeting.Id.ToString();
                }
                lines.Add($"{ me.Id },{ teamCompeting },{ me.Score },{ parent }");
            }

            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(GlobalConfig.PrizesFile.FullFilePath(), lines);
        }

        public static void SaveToPeopleFile(this List<PersonModel> models)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.CellphoneNumber }");
            }

            File.WriteAllLines(GlobalConfig.PeopleFile.FullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPeopleListToString(t.TeamMembers) }");
            }

            File.WriteAllLines(GlobalConfig.TeamFile.FullFilePath(), lines);
        }

        public static void SaveToTournamentFile(this List<TournamentModel> models)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel tm in models)
            {
                lines.Add($"{ tm.Id },{ tm.TournamentName },{ tm.EntryFee },{ ConvertTeamListToString(tm.EnteredTeams) },{ ConvertPrizeListToString(tm.Prizes) },{ ConvertRoundListToString(tm.Rounds) }");
            }

            File.WriteAllLines(GlobalConfig.TournamentFile.FullFilePath(), lines);

        }

        public static void SaveRoundsToFile(this TournamentModel model)
        {
            foreach (List<MatchUpModel> round in model.Rounds)
            {
                foreach (MatchUpModel matchup in round)
                {
                    matchup.SaveMatchupToFile();
                }
            }
        }
    }
}
