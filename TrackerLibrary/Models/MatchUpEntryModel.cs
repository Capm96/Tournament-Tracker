using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchUpEntryModel
    {
        /// <summary>
        /// The unique identifier for the matchup entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier for the team.
        /// </summary>
        public int TeamCompetingId { get; set; }

        /// <summary>
        /// One of the teams currently competing.
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Current score of the game.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// The unique identifier for the parent matchup (Team).
        /// </summary>
        public int parentMatchupId { get; set; }

        /// <summary>
        /// Previous matchup that the teamcompeting was in.
        /// </summary>
        public MatchUpModel ParentMatchup { get; set; }
    }
}
