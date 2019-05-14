using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerUI;
using TrackerLibrary;

namespace TrackerUI
{
    public partial class TournamentResultForm : Form
    {
        public static string winnerTeam;
        public static string runnerUpTeam;
        public static string tournamentName;
        public static string prizeOne;
        public static string prizeTwo;

        public TournamentResultForm()
        {
            InitializeComponent();

            WireUpTournamentInformation();

            DisplayWinnerAndRunnerUpNames();

            if (prizeOne != null && PrizeValid(prizeOne))
            {
                DisplayWinnerPrize();
            }

            if (prizeTwo != null && PrizeValid(prizeTwo))
            {
                DisplayRunnerUpPrize();
            }

        }

        private static void WireUpTournamentInformation()
        {
            winnerTeam = TournamentLogic.winnerTeam;
            runnerUpTeam = TournamentLogic.runnerUpTeam;
            tournamentName = TournamentLogic.tournamentName;
            prizeOne = TournamentLogic.prizeWinner;
            prizeTwo = TournamentLogic.prizeRunnerUp;
        }

        private void DisplayWinnerAndRunnerUpNames()
        {
            whoWonText.Text = $"{winnerTeam} has won the {tournamentName} tournament!";
            runnerUpText.Text = $"{runnerUpTeam} has finished in second place.";
        }

        private void DisplayWinnerPrize()
        {
            prizeOneText.Visible = true;
            prizeOneText.Text = $"{winnerTeam} will receive ${prizeOne}.";
        }

        private void DisplayRunnerUpPrize()
        {
            prizeTwoText.Visible = true;
            prizeTwoText.Text = $"{runnerUpTeam} will receive ${prizeTwo}.";
        }

        private bool PrizeValid(string prize)
        {
            int intOutput = 0;
            decimal decOutput = 0;

            foreach (char character in prize)
            {
                if (character.Equals('.'))
                {
                    Decimal.TryParse(prize, out decOutput);

                    if (decOutput > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            Int32.TryParse(prize, out intOutput);

            if (intOutput > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
