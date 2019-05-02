using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public interface IPrizeRequester
    {
        void PrizeComplete(PrizeModel model);
    }
}
