﻿using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Dal
{
    public sealed partial class DalXml : DalApi.IDal
    {
        private static class LoadDalObj
        {
            internal static readonly DalXml dalObj = new();
        }

        public static DalXml DalObj { get { return LoadDalObj.dalObj; } }

        public DalXml()
        {
        }

        public double[] ElectricityUseRequest()
        {
            string dalConfigPath = @"dal-config.xml";
            XElement dalConfigRoot = XElement.Load(dalConfigPath);

            double[] result = (from status in dalConfigRoot.Element("ElectricityUseRequest").Elements() select XmlConvert.ToDouble(status.Value)).ToArray();
            dalConfigRoot.Save(dalConfigPath);

            return result;
        }


        //------------------------ BONUS FUNCTIONS ---------------------------//

        /// <summary>
        /// Convert the receive angle to radian degree
        /// </summary>
        /// <param name="angleIn10thofDegree"></param>
        /// <returns> Radian degree (double)</returns>
        double ToRadians(double angleIn10thofDegree)
        {
            //Angle in 10th of a degree
            return (angleIn10thofDegree * Math.PI) / 180;
        }

        /// <summary>
        /// Calcute and return the distance between two points on earth (using longitude and lattitude).
        /// </summary>
        /// <param name="lattitude1">Lattitude point A</param>
        /// <param name="lattitude2">Lattitude point B</param>
        /// <param name="longitude1">Longitude point A</param>
        /// <param name="longitude2">Longitude point B</param>
        /// <returns> Distance between the points </returns>
        public double Distance(double lattitude1, double lattitude2, double longitude1, double longitude2)
        {
            //Convert longitude and lattitude values to radians.
            longitude1 = ToRadians(longitude1);
            longitude2 = ToRadians(longitude2);
            lattitude1 = ToRadians(lattitude1);
            lattitude2 = ToRadians(lattitude2);

            //Haversine formula.
            double distanceLongitude = longitude2 - longitude1;
            double distanceLattitude = lattitude2 - lattitude1;
            //a = (sin(distanceLattitude / 2)) ^ 2 + (cos(lattitude1) * cos(lattitude2)) * (sin(distanceLongitude / 2)) ^ 2.
            double a = Math.Pow(Math.Sin(distanceLattitude / 2), 2) +
                       Math.Cos(lattitude1) * Math.Cos(lattitude2) *
                       Math.Pow(Math.Sin(distanceLongitude / 2), 2);

            //c = 2\arcsin(sqrt(a)).
            double c = 2 * Math.Asin(Math.Sqrt(a));

            //Radius of earth in kilometers.
            double r = 6371;

            //Calculate the result.
            return c * r;
        }
    }
}
