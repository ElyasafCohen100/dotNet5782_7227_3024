using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double BatteryStatus { get; set; }

            public DroneStatuses DroneStatus;
            public ParcelInDelivery ParcelInDelivery;
            public Location CurrentLocation;

            /// <summary>
            /// Return describe of Drone struct string
            /// </summary>
            /// <returns>describe of Drone struct string</returns>
            public override string ToString()
            {
                return $"Drone: \n" +
                       $"Id: {Id}\n " +
                       $"Model: {Model}\n " +
                       $"WeightCategories: {MaxWeight}\n" +
                       $"Battery status: {BatteryStatus}\n" +
                       $"Drone status: {DroneStatus}\n" +
                       ParcelInDelivery.ToString()+
                       $"Current location: {CurrentLocation}\n";
            }
        }
    }
}