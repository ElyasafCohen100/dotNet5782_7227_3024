using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IBL
    {
        #region Add
        void AddNewStationBL(Station station);
        void AddNewDroneBL(Drone drone, int baseStationID);
        void AddNewCustomerBL(Customer customer);
        void AddNewParcelBL(Parcel parcel);
        void AddNewAdminBL(Admin admin);
        bool IsCustomerRegisered(string username, string password);
        bool IsAdminRegistered(string username, string password);
        bool IsAdminRegistered(string username);
        bool IsCustomerRegisered(string username);
        #endregion

        #region Delete
        void DeleteStation(int stationId);
        void DeleteParcel(int parcelId);
        void DeleteDrone(int droneId);
        void DeleteCustomer(int customerId);
        void DeleteAdminBL(string userName);
        #endregion

        #region Update
        void UpdateDroneModelBL(int droneId, string newName);
        void UpdateBaseStationDetailsBL(int baseStationId, string baseStationNewName, int baseStationChargeSlots);
        void UpdateCustomerDetailesBL(int customerId, string newName, string newPhoneNumber);
        void UpdateDroneToChargingBL(int droneID);
        void UpdateDroneFromChargingBL(int droneId);
        void AssociateDroneTofParcelBL(int droneId);
        void UpdateDeliveredParcelByDroneIdBL(int droneId);
        void UpdatePickedUpParcelByDroneIdBL(int droneId);
        #endregion

        #region Get
        Station GetStationByIdBL(int stationId);
        Drone GetDroneByIdBL(int droneId);
        Customer GetCustomerByIdBL(int customerId);
        Customer GetCustomerByUserName(string userName);
        Parcel GetParcelByIdBL(int parcelId);
        ParcelToList GetParcelToList(int parcelId);
        CustomerToList GetCustomerToList(int customerId);
        Admin GetAdminByUserName(string userName);
        #endregion

        #region Get List
        IEnumerable<StationToList> GetAllBaseStationsToList();
        IEnumerable<DroneToList> GetAllDroneToList();
        IEnumerable<CustomerToList> GetAllCustomerToList();
        IEnumerable<Parcel> GetAllParcels();
        IEnumerable<ParcelToList> GetAllParcelToList();
        IEnumerable<ParcelToList> GetNonAssociateParcelsListBL();
        IEnumerable<StationToList> GetStationsWithAvailableChargingSlotstBL();
        IEnumerable<DroneToList> GetDronesToList(Predicate<DroneToList> predicate);
        #endregion

        //void CheckStop(int droneId, Action action, Func<bool> func); 

        /// <summary>
        /// Return string of sexagesimal presentation.
        /// </summary>
        /// <param name="decimalNumber"></param>
        /// <returns> String of sexagesimal presentation </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
