using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IBL
    {
        //-------------------- ADD FUNCTOINS ---------------------//

        void AddNewStationBL(Station station);
        void AddNewDroneBL(Drone drone, int baseStationID);
        void AddNewCustomerBL(Customer customer);
        void AddNewParcelBL(Parcel parcel);
        void AddNewAdminBL(Admin admin);

        //------------------- DELETE FANCTIONS ------------------//
        void DeleteStation(int stationId);
        void DeleteParcel(int parcelId);
        void DeleteDrone(int droneId);
        void DeleteCustomer(int customerId);
        void DeleteAdminBL(string userName);

        //------------------- UPDATE FANCTIONS ------------------//

        void UpdateDroneModelBL(int droneId, string newName);
        void UpdateBaseStationDetailsBL(int baseStationId, string baseStationNewName, int baseStationChargeSlots);
        void UpdateCustomerDetailesBL(int customerId, string newName, string newPhoneNumber);
        void UpdateDroneToChargingBL(int droneID);
        void UpdateDroneFromChargingBL(int droneId);
        void UpdateDroneIdOfParcelBL(int droneId);
        void UpdateDeliveredParcelByDroneIdBL(int droneId);
        void UpdatePickedUpParcelByDroneIdBL(int droneId);

        //------------------- FIND FANCTIONS ------------------//

        Station FindStationByIdBL(int stationId);
        Drone FindDroneByIdBL(int droneId);
        Customer FindCustomerByIdBL(int customerId);
        Parcel FindParcelByIdBL(int parcelId);
        ParcelToList FindParcelToList(int parcelId);
        CustomerToList FindCustomerToList(int customerId);
        Admin FindAdminByUserName(string userName);

        //------------------- VIEW FANCTIONS ------------------//

        IEnumerable<StationToList> ViewBaseStationsToList();
        IEnumerable<DroneToList> ViewDroneToList();
        IEnumerable<CustomerToList> ViewCustomerToList();
        IEnumerable<Parcel> ViewParcelsList();
        IEnumerable<ParcelToList> ViewParcelToList();
        IEnumerable<ParcelToList> ViewNonAssociateParcelsListBL();
        IEnumerable<StationToList> ViewStationsWithAvailableChargingSlotstBL();
        IEnumerable<DroneToList> ViewDronesToList(Predicate<DroneToList> predicate);

        bool IsCustomerRegisered(string username, string password);
        bool IsAdminRegistered(string username, string password);


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
