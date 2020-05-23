using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection : IDisposable
    {
        void CreatePrize(PrizeModel model);

        void CreatePerson(PersonModel model);

        void CreateTeam(TeamModel model);

        void CreateTournament(TournamentModel model);

        void UpdateMatchup(MatchUpModel model);

        void CompleteTournament(TournamentModel model);

        List<TeamModel> GetTeam_All();

        List<PersonModel> GetPerson_All();

        List<TournamentModel> GetTournament_All();
    }
}
