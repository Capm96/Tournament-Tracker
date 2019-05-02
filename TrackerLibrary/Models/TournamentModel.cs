using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TournamentModel
    {
        public event EventHandler<DateTime> OnTournamentComplete;
        
        /// <summary>
        /// The unique identifier for the tournament.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of our tournament.
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// What teams need to pay in order to participate.
        /// </summary>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// Teams engaged in the tournament.
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();

        /// <summary>
        /// What the prizes in the tournament will be.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();

        /// <summary>
        /// The list of all matchups in all rounds.w
        /// </summary>
        public List<List<MatchUpModel>> Rounds { get; set; } = new List<List<MatchUpModel>>();

        public void CompleteTournament()
        {
            OnTournamentComplete?.Invoke(this, DateTime.Now);
        }
    }
}
