﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Represent drone base-station with charge slots 
    /// </summary>
    public struct Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
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
                   $"Longitude: {DalApi.IDal.SexagesimalPresentation(Longitude)}, " +
                   $"Lattitude: {DalApi.IDal.SexagesimalPresentation(Latitude)}," +
                   $"ChargeSlots: {ChargeSlots}";
        }
    }
}