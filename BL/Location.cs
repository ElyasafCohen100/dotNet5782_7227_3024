using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Location
        {
            public double Longitude { get; set; }
            public double Lattitude { get; set; }

            /// <summary>
            /// Return describe of Station struct string
            /// </summary>
            /// <returns>describe of Station struct string</returns>
            public override string ToString()
            {
                return $"Location: " +
                       $"Longitude: {DalObject.DalObject.SexagesimalPresentation(Longitude)}, " +
                       $"Lattitude: {DalObject.DalObject.SexagesimalPresentation(Lattitude)}";
            }
        }
    }
}
