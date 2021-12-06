﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        //-------------------- ADD FUNCTOINS ---------------------//

        void AddNewStationBL(Station station);
        void AddNewDroneBL(Drone drone, int baseStationID);
        void AddNewCustomerBL(Customer customer);
        void AddNewParcelBL(Parcel parcel);

        //------------------- UPDATE FANCTIONS ------------------//

        void UpdateDroneModelBL(int droneId, string newName);
        void UpdateBaseStationDetails(int baseStationId, string baseStationNewName, int baseStationChargeSlots);
        void UpdateCustomerDetailes(int customerId, string newName, string newPhoneNumber);
        void UpdateDroneToChargingBL(int droneID);
        void UpdateDroneFromChargingBL(int droneId, double chargeTime);
        void UpdateDroneIdOfParcelBL(int droneId);
        void UpdateDeliveredParcelByDroneIdBL(int droneId);
        void UpdatePickedUpParcelByDroneIDBL(int droneId);

        //------------------- FIND FANCTIONS ------------------//

        Station FindStationByIdBL(int stationId);
        Drone FindDroneByIdBL(int droneId);
        Customer FindCustomerByIdBL(int customerId);
        Parcel FindParcelByIdBL(int parcelId);

        //------------------- VIEW FANCTIONS ------------------//

        IEnumerable<StationToList> ViewBaseStationsToList();
        IEnumerable<DroneToList> ViewDroneToList();
        IEnumerable<CustomerToList> ViewCustomerToList();
        IEnumerable<ParcelToList> ViewParcelToList();
        IEnumerable<ParcelToList> ViewNonAssociateParcelsListBL();
        IEnumerable<StationToList> ViewStationsWithAvailableChargingSlotstBL();
        IEnumerable<DroneToList> ViewDronesToList(Predicate<DroneToList> predicate);
    }
}
