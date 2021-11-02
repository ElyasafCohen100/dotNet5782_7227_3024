using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        //----------------------- FIND FUNCTIONS -----------------------//

        static Drone FindDroneById(int droneId) { return new Drone(); }
        static Station FindStationById(int stationId) { return new Station(); }
        static DroneCharge FindDroneChargeByDroneId(int droneId) { return new DroneCharge(); }
        static Customer FindCustomerById(int customerId) { return new Customer(); }
        static Parcel FindParcelById(int parcelId) { return new Parcel(); }

        //----------------------- SETTERS -----------------------//

        static void SetNewDrone(Drone drone) { }
        static void SetNewStation(Station station) { }
        static void SetNewCustomer(Customer Customer) { }
        static void SetNewParcel(Parcel Parcel) { }

        //----------------------- UPDATE FUNCTIONS -----------------------//

        static void UpdateDroneIdOfParcel(int parcelId, int droneId) { }
        static void UpdatePickedUpParcelById(int parcelId) { }
        static void UpdateDeliveredParcelById(int parcelId) { }
        static void UpdateDroneToCharging(int droneId, int stationId) { }
        static void UpdateDroneFromCharging(int droneId) { }

        //--------------------------- GETTERS ---------------------------//

        static IEnumerable<Station> GetBaseStationList() { return new List<Station>(); }
        static IEnumerable<Drone> GetDroneList() { return new List<Drone>(); }
        static IEnumerable<Customer> GetCustomerList() { return new List<Customer>(); }
        static IEnumerable<Parcel> GetParcelList() { return new List<Parcel>(); }
        static IEnumerable<Parcel> GetNonAssociateParcelList() { return new List<Parcel>(); }
        static IEnumerable<Station> GetStationsWithAvailableChargingSlots() { return new List<Station>(); }

        static double[] ElectricityUseRequest() { return new double[5] { 0, 0, 0, 0, 0 }; }
    }
}