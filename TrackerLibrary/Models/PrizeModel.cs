using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// The unique identifier for the prize.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Total reward to a player in dollars.
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// Description of position, i.e: "Winner", "RunnerUp", or "First Place"
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Position which player ended in the tournament. We only handle 1 (for first) and 2 (for second).
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// What percentage of the tournament's total income the plyer will get.
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;

            int placeNumberValue = 0;
            int.TryParse(placeNumber, out placeNumberValue);
            PlaceNumber = placeNumberValue;

            decimal prizeAmountValue = 0;
            decimal.TryParse(prizeAmount, out prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            double prizePercentageValue = 0;
            double.TryParse(prizePercentage, out prizePercentageValue);
            PrizePercentage = prizePercentageValue;
        }
    }
}
