using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        void SetNewStationBL(Station station) { }

        void SetNewDroneBL(Drone drone, int baseStationID) { }
        
    }
}
