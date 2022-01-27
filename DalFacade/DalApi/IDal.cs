using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region Get
        Drone GetDroneById(int droneId);
        Station GetStationById(int stationId);
        DroneCharge GetDroneChargeByDroneId(int droneId);
        Customer GetCustomerById(int customerId);
        Customer GetCustomerByUserName(string username);
        Admin GetAdminByUserName(string userName);
        Parcel GetParcelById(int parcelId);
        IEnumerable<Station> GetBaseStationList();
        IEnumerable<Drone> GetDroneList();
        IEnumerable<Customer> GetCustomerList();
        IEnumerable<Parcel> GetParcelList();
        IEnumerable<Admin> GetAdminsList();
        IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate);
        IEnumerable<Station> GetStations(Predicate<Station> predicate);
        IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate);
        #endregion


        #region Add
        void AddNewDrone(Drone drone);
        void AddNewStation(Station station);
        void AddNewCustomer(Customer customer);
        void AddNewParcel(Parcel parcel);
        void AddAdmin(Admin admin);
        void AddDroneCharge(int droneId, int stationId);
        #endregion


        #region Update
        void UpdateDroneIdOfParcel(int parcelId, int droneId);
        void UpdatePickedUpParcelById(int parcelId);
        void UpdateDeliveredParcelById(int parcelId);
        void UpdateDroneToCharging(int droneId, int stationId);
        void UpdateDroneFromCharging(int droneId);
        void UpdateDroneModel(int droneId, string newModel);
        void UpdateBaseStationDetails(int baseStationId, string baseStationNewName, int baseStationChargeSlots);
        void UpdateCustomerDetailes(int customerId, string newName, string newPhoneNumber);
        void UpdateAdminPassword(Admin newAdmin);
        #endregion


        #region Delete 
        void DeleteParcel(int parcelId);
        void DeleteStation(int parcelId);
        void DeleteDrone(int droneId);
        void DeleteCustomer(int customerId);
        void DeleteAdmin(string userName);
        void DeleteDroneCharge(int droneId);
        #endregion


        #region Relese Drone Charges
        [MethodImpl(MethodImplOptions.Synchronized)]
        void ReleseDroneCharges() { }
        #endregion


        #region Electricity Use Request
        double[] ElectricityUseRequest();
        #endregion


        #region Distance

        [MethodImpl(MethodImplOptions.Synchronized)]
        double Distance(double lattitude1, double lattitude2, double longitude1, double longitude2);
        #endregion


        #region Sexagesimal
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
        #endregion
    }
}