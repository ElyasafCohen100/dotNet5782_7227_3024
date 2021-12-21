using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class DroneToList
    {
        public int Id { get; set; }
        public string Model { get; set; }

        public WeightCategories MaxWeight;
        public double BatteryStatus { get; set; }

        public DroneStatuses DroneStatus;

        public int DeliveryParcelId { get; set; }

        public Location CurrentLocation = new();

        /// <summary>
        /// Return describe of Drone class string.
        /// </summary>
        /// <returns> Describe of Drone class string </returns>
        public override string ToString()
        {
            return $"Drone: \n" +
                   $"Id: {Id}\n" +
                   $"Model: {Model}\n" +
                   $"WeightCategories: {MaxWeight}\n" +
                   $"Battery status: {BatteryStatus}%\n" +
                   $"Drone status: {DroneStatus}\n" +
                   $"Delivery parcel id: {DeliveryParcelId}\n" +
                   $"Current location: {CurrentLocation}\n";
        }
    }
}
