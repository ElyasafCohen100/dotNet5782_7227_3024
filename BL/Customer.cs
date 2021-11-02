using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }

            /// <summary>
            /// Return describe of Customer struct string
            /// </summary>
            /// <returns>describe of Customer struct string</returns>
            public override string ToString()
            {
                return $"Customer name: {Name}\n" +
                       $"Id: {Id}\n" +
                       $"Phone: {Phone}\n" +
                       $"Longitude: {DalObject.DalObject.SexagesimalPresentation(Longitude)}, " +
                       $"Lattitude: {DalObject.DalObject.SexagesimalPresentation(Lattitude)},";
            }
        }
    }
}
