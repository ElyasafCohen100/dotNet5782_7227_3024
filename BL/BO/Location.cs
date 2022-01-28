using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        /// <summary>
        /// Return describe of Station class string.
        /// </summary>
        /// <returns> Describe of Station class string </returns>
        public override string ToString()
        {
            return $"{BL.BL.BlObj.SexagesimalPresentation(Longitude)}, {BL.BL.BlObj.SexagesimalPresentation(Latitude)}";
        }
    }
}
