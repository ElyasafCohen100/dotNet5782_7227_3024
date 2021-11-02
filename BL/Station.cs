using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public int ChargeSlots { get; set; }

            /// <summary>
            /// Return describe of Station struct string
            /// </summary>
            /// <returns>describe of Station struct string</returns>
            public override string ToString()
            {
                return $"Station: " +
                       $"Id: {Id}, " +
                       $"Name: {Name}, " +
                       $"Longitude: {DalObject.DalObject.SexagesimalPresentation(Longitude)}, " +
                       $"Lattitude: {DalObject.DalObject.SexagesimalPresentation(Lattitude)}," +
                       $"ChargeSlots: {ChargeSlots}";
            }
        }
    }
}

