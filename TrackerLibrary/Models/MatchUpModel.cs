using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchUpModel
    {
        /// <summary>
        /// The unique identifier for the matchup.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// List all of the current matchups.
        /// </summary>
        public List<MatchUpEntryModel> Entries { get; set; } = new List<MatchUpEntryModel>();

        /// <summary>
        /// the ID from the database that will be used to identify he winner.
        /// </summary>
        public int WinnerId { get; set; }

        /// <summary>
        /// Winner of a matchup. 
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// What current stage, or round, of our tournament we are in.
        /// </summary>
        public int MatchupRound { get; set; }

        public string DisplayName
        {
            get
            {
                string output = "";

                foreach (MatchUpEntryModel me in Entries)
                {
                    if (me.TeamCompeting != null)
                    {
                        if (output.Length == 0)
                        {
                            output = me.TeamCompeting.TeamName;
                        }
                        else
                        {
                            output += $" vs. { me.TeamCompeting.TeamName }";
                        }
                    }
                    else
                    {
                        output = "Match Not Yet Determined";
                        break;
                    }
                }

                return output;
            }
        }
    }   
}
