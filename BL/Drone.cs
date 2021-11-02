using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategiries MaxWeight;

            /// <summary>
            /// Return describe of Drone struct string
            /// </summary>
            /// <returns>describe of Drone struct string</returns>
            public override string ToString()
            {
                return $"Drone: " +
                       $"Id: {Id}, " +
                       $"Model: {Model}, " +
                       $"WeightCategiries: {MaxWeight}";
            }
        }
    }
}