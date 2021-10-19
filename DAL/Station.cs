using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public int ChargeSlots { get; set; }

            public override string ToString()
            {
                return $"Station: " +
                $"Id: {Id}, " +
                $"Name: {Name}, " +
                $"Longitude: {Longitude}, " +
                $"Lattitude: {Lattitude}," +
                $"ChargeSlots: {ChargeSlots}";
            }
        }
    }
}
