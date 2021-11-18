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
        public void AddNewStationBL(Station station)
        {
            IDAL.DO.Station newStation = new();

            newStation.Id = station.Id;
            newStation.Name = station.Name;
            newStation.Lattitude = station.Location.Lattitude;
            newStation.Longitude = station.Location.Longitude;
            newStation.ChargeSlots = station.AvailableChargeSlots;

            dalObject.SetNewStation(newStation);
        }

        public void UpdateBaseStationDetailes(int baseStationId, string baseStationNewName, int baseStationChargeSlots)
        {
            IDAL.DO.Station baseStation = dalObject.FindStationById(baseStationId);

            if (baseStationNewName != "")
            {
                baseStation.Name = baseStationNewName;
            }

            if (baseStationChargeSlots != 0)
            {
                baseStation.ChargeSlots = baseStationChargeSlots;
            }
        }
    }
}
