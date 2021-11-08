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

        Drone FindDroneById(int droneId) { return new Drone(); }
        Station FindStationById(int stationId) { return new Station(); }
        DroneCharge FindDroneChargeByDroneId(int droneId) { return new DroneCharge(); }
        Customer FindCustomerById(int customerId) { return new Customer(); }
        Parcel FindParcelById(int parcelId) { return new Parcel(); }

        //----------------------- SETTERS -----------------------//

        void SetNewDrone(Drone drone) { }
        void SetNewStation(Station station) { }
        void SetNewCustomer(Customer Customer) { }
        void SetNewParcel(Parcel Parcel) { }

        //----------------------- UPDATE FUNCTIONS -----------------------//

        void UpdateDroneIdOfParcel(int parcelId, int droneId) { }
        void UpdatePickedUpParcelById(int parcelId) { }
        void UpdateDeliveredParcelById(int parcelId) { }
        void UpdateDroneToCharging(int droneId, int stationId) { }
        void UpdateDroneFromCharging(int droneId) { }

        //--------------------------- GETTERS ---------------------------//

        IEnumerable<Station> GetBaseStationList() { return new List<Station>(); }
        IEnumerable<Drone> GetDroneList() { return new List<Drone>(); }
        IEnumerable<Customer> GetCustomerList() { return new List<Customer>(); }
        IEnumerable<Parcel> GetParcelList() { return new List<Parcel>(); }
        IEnumerable<Parcel> GetNonAssociateParcelList() { return new List<Parcel>(); }
        IEnumerable<Station> GetStationsWithAvailableChargingSlots() { return new List<Station>(); }

        double[] ElectricityUseRequest() { return new double[5] { 0, 0, 0, 0, 0 }; }
    }
}