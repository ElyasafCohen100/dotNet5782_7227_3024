using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DalObject;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        IDAL.IDal dalObject = new DalObject.DalObject();
        public void SetNewStationBL(Station station)
        {
            IDAL.DO.Station newStation = new();

            newStation.Id = station.Id;
            newStation.Name = station.Name;
            newStation.Lattitude = station.Location.Lattitude;
            newStation.Longitude = station.Location.Longitude;
            newStation.ChargeSlots = station.AvailableChargeSlots;

            dalObject.SetNewStation(newStation);
        }

    }
}
