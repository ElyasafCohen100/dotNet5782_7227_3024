using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace Dal
{
    public sealed partial class DalXml : DalApi.IDal
    {
        #region Xml Files
        public static string dalParcelPath = @"Data\Parcels.xml";
        public static string dalConfigPath = @"Data\Config.xml";
        public static string dalAdminPath = @"Data\Admins.xml";
        public static string dalCustomerPath = @"Data\Customers.xml";
        public static string dalDronePath = @"Data\Drones.xml";
        public static string dalDroneChargePath = @"Data\DroneCharges.xml";
        public static string dalStationPath = @"Data\Stations.xml";
        #endregion


        #region Singleton
        private static class LoadDalObj
        {
            internal static readonly DalXml dalObj = new();
        }

        public static DalXml DalObj { get { return LoadDalObj.dalObj; } }
        #endregion


        #region Constructor
        public DalXml()
        {
        }
        #endregion


        #region Electricity Use Request
        /// <summary>
        /// Request of eletricity use by drone
        /// </summary>
        /// <returns> Return array with electricity use request </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] ElectricityUseRequest()
        {
            XElement dalConfigRoot = XElement.Load(dalConfigPath);

            double[] Electricity = (from status in dalConfigRoot.Element("ElectricityUseRequest").Elements() select XmlConvert.ToDouble(status.Value)).ToArray();
            dalConfigRoot.Save(dalConfigPath);
            return Electricity;
        }
        #endregion


        #region Distance

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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        #endregion
    }
}
