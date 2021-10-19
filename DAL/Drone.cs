using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategiries MaxWeight;
            public DroneStatuses Status;
            public double Battery { get; set; }

            public override string ToString()
            {
                return $"Drone: " +
                $"Id: {Id}, " +
                $"Model: {Model}, " +
                $"WeightCategiries: {MaxWeight}, " +
                $"DroneStatuses: {Status}," +
                $"Battery: {Battery}";
            }
        }
    }
}