using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Represent drone
    /// </summary>
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight;

        /// <summary>
        /// Return describe of Drone struct string
        /// </summary>
        /// <returns>describe of Drone struct string</returns>
        public override string ToString()
        {
            return $"Drone:\n" +
                   $"Id: {Id}\n" +
                   $"Model: {Model}\n" +
                   $"MaxWeight: {MaxWeight}";
        }
    }
}