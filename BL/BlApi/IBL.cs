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
        bool IsCustomerRegistered(string username, string password);
        bool IsAdminRegistered(string username, string password);
        bool IsAdminExsist(string username);
        bool IsCustomerExsist(string username);
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
        double BatteryCalac(DroneToList droneToList, DroneCharge droneCharge);

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
        DroneCharge FindDroneChargeByDroneIdBL(int droneId);
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

        #region Simulator
        void StartSimulator(int droneId, Action UpdateAction, Func<bool> checkStopFunc);
        #endregion

        #region Sexagesimal
        string SexagesimalPresentation(double decimalNumber);
        #endregion
    }
}