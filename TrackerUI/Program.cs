using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using System.IO;

namespace TrackerUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize the database or textfile connections.
            string currentPath = Directory.GetCurrentDirectory();
            string TextFilesPath = $@"{currentPath}\Data";
            Directory.CreateDirectory(TextFilesPath);
            GlobalConfig.InitializeConnections(DatabaseType.Sql);

            Application.Run(new TournamentDashboardForm());
        }
    }
}
