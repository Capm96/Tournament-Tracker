using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;
using Squirrel;
using System.Diagnostics;

namespace TrackerUI
{
    public partial class TournamentDashboardForm : Form
    {
        List<TournamentModel> tournaments = GlobalConfig.Connection.GetTournament_All();

        public TournamentDashboardForm()
        {
            InitializeComponent();
            WireUpLists();

            AddVersionNumber();
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            this.Text += $" v {versionInfo.FileVersion}";
        }

        private void WireUpLists()
        {
            loadExistingTournamentDropdown.DataSource = tournaments;
            loadExistingTournamentDropdown.DisplayMember = "TournamentName";
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm form = new CreateTournamentForm();
            form.Show();
        }

        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            if (loadExistingTournamentDropdown.SelectedItem != null)
            {
                TournamentModel tm = (TournamentModel)loadExistingTournamentDropdown.SelectedItem;

                tournamentViewerForm form = new tournamentViewerForm(tm);

                form.Show();
            }
            else
            {
                return;
            }
        }
    }
}
