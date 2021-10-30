using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


//---------------------------------------------- INTERFACE ------------------------------------------------//
namespace IDAL
{
   public interface IDal
    {
        //----------------------- FIND FUNCTIONS -----------------------//

        public static Drone FindDroneById(int droneId) { return new Drone(); }
        public static Station FindStationById(int stationId) { return new Station(); }
        public static DroneCharge FindDroneChargeByDroneId(int droneId) { return new DroneCharge(); }
        public static Customer FindCustomerById(int customerId) { return new Customer(); }
        public static Parcel FindParcelById(int parcelId) { return new Parcel(); }

        //----------------------- SETTERS -----------------------//

        public static void SetNewDrone(Drone drone) { }
        public static void SetNewStation(Station station) { }
        public static void SetNewCustomer(Customer Customer) { }
        public static void SetNewParcel(Parcel Parcel) { }

        //----------------------- UPDATE FUNCTIONS -----------------------//

        public static void UpdateDroneIdOfParcel(int parcelId, int droneId) { }
        public static void UpdatePickedUpParcelById(int parcelId) { }
        public static void UpdateDeliveredParcelById(int parcelId) { }
        public static void UpdateDroneToCharging(int droneId, int stationId) { }
        public static void UpdateDroneFromCharging(int droneId) { }

        //--------------------------- GETTERS ---------------------------//

        public static IEnumerable<Station> GetBaseStationList() { return new List<Station>(); }
        public static IEnumerable<Drone> GetDroneList() { return new List<Drone>(); }
        public static IEnumerable<Customer> GetCustomerList() { return new List<Customer>(); }
        public static IEnumerable<Parcel> GetParcelList() { return new List<Parcel>(); }
        public static IEnumerable<Parcel> GetNonAssociateParcelList() { return new List<Parcel>(); }
        public static IEnumerable<Station> GetStationsWithAvailableChargingSlots() { return new List<Station>(); }

        public static double[] ElectricityUseRequest() {return new double[5]{0,0,0,0,0};}
    }
}