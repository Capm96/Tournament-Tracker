using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;

            if (t != null)
            {
                availableTeams.Remove(t);
                selectedTeams.Add(t);

                WireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // Call the CreatePrizeForm.
            CreatePrizeForm form = new CreatePrizeForm(this);
            form.Show();
        }

        public void PrizeComplete(PrizeModel model)
        {
            // Get back from the form a PrizeModel.
            // Take that PrizeModel and put it into our list of Selected Prizes.
            selectedPrizes.Add(model);
            WireUpLists();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void createNewTeamLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            createTeamForm form = new createTeamForm(this);
            form.Show();
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;

            if (p != null)
            {
                selectedPrizes.Remove(p);

                WireUpLists();
            }
        }

        private void removeSelectedPlayerButton_Click_1(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)tournamentTeamsListBox.SelectedItem;

            if (t != null)
            {
                selectedTeams.Remove(t);
                availableTeams.Add(t);

                WireUpLists();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // Validate Data.
            decimal fee = 0;
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out fee);
            
            if (!feeAcceptable || fee < 0)
            {
                MessageBox.Show("You need to enter a valid Entry Fee.", "Invalid Fee");
                return;
            }

            // Create our tournament model.
            TournamentModel tm = new TournamentModel();

            if (tournamentNameValue.Text.Length > 0)
            {
                tm.TournamentName = tournamentNameValue.Text;

            }
            else
            {
                MessageBox.Show("Please enter a valid Tournament Name.", "Invalid Tournament Name");
                return;
            }

            tm.EntryFee = fee;
            tm.Prizes = selectedPrizes;

            if (selectedTeams.Count < 2)
            {
                MessageBox.Show("Please register at least two teams in the tournament", "Invalid Team Count");
                return;
            }
            else
            {
                tm.EnteredTeams = selectedTeams;
            }

            // Create our matchups.
            TournamentLogic.CreateRounds(tm);

            // Create Tournament Entry.
            // Create all of the prizes entries.
            // Create all of the team entries.
            GlobalConfig.Connection.CreateTournament(tm);

            // Emailing feature currently disabled.
            //tm.AlertUsersToNewRound();

            tournamentViewerForm form = new tournamentViewerForm(tm);
            form.Show();
            this.Close();
        }
    }
}
