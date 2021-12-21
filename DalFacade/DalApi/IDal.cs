using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        //----------------------- FIND FUNCTIONS -----------------------//

        Drone FindDroneById(int droneId);
        Station FindStationById(int stationId);
        DroneCharge FindDroneChargeByDroneId(int droneId);
        Customer FindCustomerById(int customerId);
        Parcel FindParcelById(int parcelId);

        //----------------------- SETTERS -----------------------//

        void SetNewDrone(Drone drone);
        void SetNewStation(Station station);
        void SetNewCustomer(Customer customer);
        void SetNewParcel(Parcel parcel);
        void AddDroneCharge(int droneId, int stationId);
        //----------------------- UPDATE FUNCTIONS -----------------------//

        void UpdateDroneIdOfParcel(int parcelId, int droneId);
        void UpdatePickedUpParcelById(int parcelId);
        void UpdateDeliveredParcelById(int parcelId);
        void UpdateDroneToCharging(int droneId, int stationId);
        void UpdateDroneFromCharging(int droneId);
        void UpdateDroneModel(int droneId, string newModel);
        void UpdateBaseStationDetails(int baseStationId, string baseStationNewName, int baseStationChargeSlots);
        void UpdateCustomerDetailes(int customerId, string newName, string newPhoneNumber);

        //--------------------------- GETTERS ---------------------------//

        IEnumerable<Station> GetBaseStationList();
        IEnumerable<Drone> GetDroneList();
        IEnumerable<Customer> GetCustomerList();
        IEnumerable<Parcel> GetParcelList();
        IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate);
        IEnumerable<Station> GetStations(Predicate<Station> predicate);
        IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate);

        double Distance(double lattitude1, double lattitude2, double longitude1, double longitude2);
        double[] ElectricityUseRequest();

        /// <summary>
        /// Return string of sexagesimal presentation.
        /// </summary>
        /// <param name="decimalNumber"></param>
        /// <returns> String of sexagesimal presentation </returns>
        public static string SexagesimalPresentation(double decimalNumber)
        {
            int degrees, minutes1, seconds;
            double minutes2;

            degrees = (int)decimalNumber;

            minutes2 = (decimalNumber - degrees) * 60;
            minutes1 = (int)minutes2;

            seconds = (int)((minutes2 - minutes1) * 60);

            return $"{degrees}°{minutes1}'{seconds}\"";
        }
    }
}
