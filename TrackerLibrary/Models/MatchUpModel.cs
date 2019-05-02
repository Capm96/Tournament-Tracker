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
        /// List of the entries in this matchup and their respective properties (team name, score, etc).
        /// </summary>
        public List<MatchUpEntryModel> Entries { get; set; } = new List<MatchUpEntryModel>();

        /// <summary>
        /// The ID that will be used to identify this matchup's winner.
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

        /// <summary>
        /// Creates the display name for the matchup in our tournament viewer form.
        /// </summary>
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
