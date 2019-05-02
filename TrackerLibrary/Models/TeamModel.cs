using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TeamModel
    {
        /// <summary>
        /// The unique identifier for the team.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The list of persons that are part of this team.
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

        /// <summary>
        /// The team's name.
        /// </summary>
        public string TeamName { get; set; }
    }
}

