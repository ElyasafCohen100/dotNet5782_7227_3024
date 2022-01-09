using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
    {
        //#region Find

        ///// <summary>
        ///// Finds Parcel by specific Id.
        ///// </summary>
        ///// <param name="parcelId"> Parcel Id </param>
        ///// <returns> Parcel object </returns>
        ///// <exception cref="ObjectDisposedException"></exception>
        //public Parcel FindParcelById(int parcelId)
        //{
        //    XElement dalParcelsRoot = XElement.Load(dalParcelPath);

        //    Parcel dalParcel = (from parcel in dalParcelsRoot.Element("Parcel").Elements()
        //                        where Convert.ToInt32(parcel.Element("Id").Value) == parcelId
        //                        select new Parcel
        //                        {
        //                            Id = Convert.ToInt32(parcel.Element("Id").Value),
        //                            SenderId = Convert.ToInt32(parcel.Element("SenderId").Value),
        //                            TargetId = Convert.ToInt32(parcel.Element("TargetId").Value),
        //                            DroneId = Convert.ToInt32(parcel.Element("DroneId").Value),
        //                            Priority = (Priorities)Convert.ToInt32(parcel.Element("Priority").Value),
        //                            Weight = (WeightCategories)Convert.ToInt32(parcel.Element("Weight").Value),
        //                            Requested = Convert.ToDateTime(parcel.Element("Requested").Value),
        //                            Scheduled = Convert.ToDateTime(parcel.Element("Scheduledd").Value),
        //                            PickedUp = Convert.ToDateTime(parcel.Element("PickedUp").Value),
        //                            Delivered = Convert.ToDateTime(parcel.Element("Delivered").Value),
        //                            IsActive = Convert.ToBoolean(parcel.Element("IsActive").Value)
        //                        }).FirstOrDefault();
        //    dalParcelsRoot.Save(dalParcelPath);

        //    return dalParcel.Id != parcelId && dalParcel.IsActive == false ? throw new ObjectNotFoundException("parcel") : dalParcel;
        //}
        //#endregion

        //#region Setters
        ///// <summary>
        ///// Set new Parcel.
        ///// </summary>
        ///// <param name="parcel"> Parcel object </param>
        //public void SetNewParcel(Parcel parcel)
        //{

        //    XElement dalParcelsRoot = XElement.Load(dalParcelPath);
        //    XElement dalConfigRoot = XElement.Load(dalConfigPath);

        //    parcel.Id = Convert.ToInt32(dalConfigRoot.Element("SerialNum").Value);
        //    parcel.IsActive = true;

        //    dalParcelsRoot.Add(parcel);

        //    dalConfigRoot.Element("SerialNum").Value = (parcel.Id + 1).ToString();

        //    dalConfigRoot.Save(dalConfigPath);
        //    dalParcelsRoot.Save(dalParcelPath);
        //}
        //#endregion

        //#region Update
        ///// <summary>
        ///// Update Parcel status to picked up.
        ///// </summary>
        ///// <param name="parcelId"> Parcel Id </param>
        //public void UpdatePickedUpParcelById(int parcelId)
        //{
        //    try
        //    {
        //        XElement dalParcelsRoot = XElement.Load(dalParcelPath);

        //        XElement dalParcel = (from parcel in dalParcelsRoot.Element("Parcel").Elements()
        //                              where XmlConvert.ToInt32(parcel.Element("Id").Value) == parcelId
        //                              select parcel).FirstOrDefault();

        //        if (dalParcel == null) throw new ObjectNotFoundException("Parcel");

        //        DateTime? pickedUp = DateTime.Now;
        //        dalParcel.Element("PickedUp").Value = pickedUp.ToString();
        //        dalParcelsRoot.Save(dalParcelPath);
        //    }
        //    catch (ObjectNotFoundException)
        //    {
        //        throw;
        //    }
        //}


        ///// <summary>
        ///// Update Parcel status to Delivered. 
        ///// </summary>
        ///// <param name="parcelId"> Parcel Id</param>
        //public void UpdateDeliveredParcelById(int parcelId)
        //{
        //    try
        //    {
        //        XElement dalParcelsRoot = XElement.Load(dalParcelPath);

        //        XElement dalParcel = (from parcel in dalParcelsRoot.Element("Parcel").Elements()
        //                              where XmlConvert.ToInt32(parcel.Element("Id").Value) == parcelId
        //                              select parcel).FirstOrDefault();

        //        if (dalParcel == null) throw new ObjectNotFoundException("parcel");

        //        DateTime? delivered = DateTime.Now;
        //        dalParcel.Element("Delivered").Value = delivered.ToString();
        //        dalParcelsRoot.Save(dalParcelPath);
        //    }
        //    catch (ObjectNotFoundException)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Update Drone Id of Parcel.
        ///// </summary>
        ///// <param name="parcelId"> Id of Parcel </param>
        ///// <param name="droneId"> Id of Drone </param>
        //public void UpdateDroneIdOfParcel(int parcelId, int droneId)
        //{
        //    try
        //    {
        //        XElement dalParcelsRoot = XElement.Load(dalParcelPath);

        //        XElement dalParcel = (from parcel in dalParcelsRoot.Element("Parcel").Elements()
        //                              where XmlConvert.ToInt32(parcel.Element("Id").Value) == parcelId
        //                              select parcel).FirstOrDefault();

        //        if (dalParcel == null) throw new ObjectNotFoundException("parcel");

        //        dalParcel.Element("DroneId").Value = droneId.ToString();
        //        DateTime? scheduled = DateTime.Now;
        //        dalParcel.Element("Scheduled").Value = scheduled.ToString();

        //        dalParcelsRoot.Save(dalParcelPath);
        //    }
        //    catch (ObjectNotFoundException)
        //    {
        //        throw;
        //    }
        //}
        //#endregion

        //#region Getters
        ///// <summary>
        ///// Return List of Parcels.
        ///// </summary>
        ///// <returns>List of Parcels </returns>
        //public IEnumerable<Parcel> GetParcelList()
        //{
        //    XElement dalParcelsRoot = XElement.Load(dalParcelPath);

        //    var v = from parcel in dalParcelsRoot.Elements()
        //            where parcel.Element("IsActive").Value == "true"
        //            select new Parcel
        //            {
        //                Id = int.Parse(parcel.Element("Id").Value),
        //                SenderId = int.Parse(parcel.Element("SenderId").Value),
        //                TargetId = int.Parse(parcel.Element("TargetId").Value),
        //                DroneId = int.Parse(parcel.Element("DroneId").Value),
        //                Priority = (Priorities)Enum.Parse(typeof(Priorities), parcel.Element("Priority").Value),
        //                Weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), parcel.Element("Weight").Value),
        //                Requested = DateTime.ParseExact(parcel.Element("Requested").Value, "O", null),
        //                Scheduled = DateTime.ParseExact(parcel.Element("Scheduled").Value, "O", null),
        //                PickedUp = DateTime.ParseExact(parcel.Element("PickedUp").Value, "O", null),
        //                Delivered = DateTime.ParseExact(parcel.Element("Delivered").Value, "O", null),
        //                IsActive = parcel.Element("IsActive").Value == "false" ? false : true
        //            };
        //    return v;
        //}

        ///// <summary>
        ///// Return List of non associate Parcels.
        ///// </summary>
        ///// <returns> List of non associate Parcels </returns>
        //public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        //{
        //    XElement dalParcelsRoot = XElement.Load(dalParcelPath);
        //    IEnumerable<Parcel> parcels = from parcel in dalParcelsRoot.Element("Parcel").Elements()
        //                                  where XmlConvert.ToBoolean(parcel.Element("IsActive").Value) && predicate.Equals(true)
        //                                  select new Parcel
        //                                  {
        //                                      Id = Convert.ToInt32(parcel.Element("Id").Value),
        //                                      SenderId = Convert.ToInt32(parcel.Element("SenderId").Value),
        //                                      TargetId = Convert.ToInt32(parcel.Element("TargetId").Value),
        //                                      DroneId = Convert.ToInt32(parcel.Element("DroneId").Value),
        //                                      Priority = (Priorities)Convert.ToInt32(parcel.Element("Priority").Value),
        //                                      Weight = (WeightCategories)Convert.ToInt32(parcel.Element("Weight").Value),
        //                                      Requested = Convert.ToDateTime(parcel.Element("Requested").Value),
        //                                      Scheduled = Convert.ToDateTime(parcel.Element("Scheduledd").Value),
        //                                      PickedUp = Convert.ToDateTime(parcel.Element("PickedUp").Value),
        //                                      Delivered = Convert.ToDateTime(parcel.Element("Delivered").Value),
        //                                      IsActive = Convert.ToBoolean(parcel.Element("IsActive").Value)
        //                                  };
        //    dalParcelsRoot.Save(dalParcelPath);
        //    return parcels;
        //}
        //#endregion

        //#region Delete
        //public void DeleteParcel(int parcelId)
        //{
        //    XElement dalParcelsRoot = XElement.Load(dalParcelPath);

        //    XElement dalParcel = (from customer in dalParcelsRoot.Element("Parcel").Elements()
        //                          where XmlConvert.ToInt32(customer.Element("Id").Value) == parcelId
        //                          select customer).FirstOrDefault();
        //    dalParcel.Remove();
        //    dalParcelsRoot.Save(dalParcelPath);
        //}
        //#endregion



        #region Find

        /// <summary>
        /// Finds Parcel by specific Id.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        /// <returns> Parcel object </returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public Parcel FindParcelById(int parcelId)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            Parcel parcel = parcelList.Find(x => x.Id == parcelId);
            return parcel.Id != parcelId && parcel.IsActive == false ? throw new ObjectNotFoundException("parcel") : parcel;
        }
        #endregion

        #region Setters
        /// <summary>
        /// Set new Parcel.
        /// </summary>
        /// <param name="parcel"> Parcel object </param>
        public void SetNewParcel(Parcel parcel)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            XElement dalConfigRoot = XElement.Load(dalConfigPath);

            parcel.Id = Convert.ToInt32(dalConfigRoot.Element("SerialNum").Value);
            parcel.IsActive = true;
            parcelList.Add(parcel);

            dalConfigRoot.Element("SerialNum").Value = (parcel.Id + 1).ToString();
            dalConfigRoot.Save(dalConfigPath);
            XMLTools.SaveListToXMLSerializer(parcelList, dalParcelPath);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Parcel status to picked up.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        public void UpdatePickedUpParcelById(int parcelId)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            int index = parcelList.FindIndex(x => x.Id == parcelId);
            if (index == -1) throw new ObjectNotFoundException("parcel");

            Parcel parcel = parcelList[index];
            parcel.PickedUp = DateTime.Now;
            parcelList[index] = parcel;
            XMLTools.SaveListToXMLSerializer(parcelList, dalParcelPath);
        }

        /// <summary>
        /// Update Drone Id of Parcel.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        /// <param name="droneId"> Id of Drone </param>
        public void UpdateDroneIdOfParcel(int parcelId, int droneId)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            int index = parcelList.FindIndex(x => x.Id == parcelId);
            if (index == -1) throw new ObjectNotFoundException("parcel");
            Parcel parcel = parcelList[index];
            parcel.DroneId = droneId;
            parcel.Scheduled = DateTime.Now;
            parcelList[index] = parcel;
            XMLTools.SaveListToXMLSerializer(parcelList, dalParcelPath);
        }

        /// <summary>
        /// Update Parcel status to Delivered. 
        /// </summary>
        /// <param name="parcelId"> Parcel Id</param>
        public void UpdateDeliveredParcelById(int parcelId)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            int index = parcelList.FindIndex(x => x.Id == parcelId);
            if (index == -1) throw new ObjectNotFoundException("parcel");

            Parcel parcel = parcelList[index];
            parcel.Delivered = DateTime.Now;
            parcelList[index] = parcel;
            XMLTools.SaveListToXMLSerializer(parcelList, dalParcelPath);
        }
        #endregion

        #region Getters
        /// <summary>
        /// Return List of Parcels.
        /// </summary>
        /// <returns>List of Parcels </returns>
        public IEnumerable<Parcel> GetParcelList()
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            return from parcel in parcelList where parcel.IsActive select parcel;
        }

        /// <summary>
        /// Return List of non associate Parcels.
        /// </summary>
        /// <returns> List of non associate Parcels </returns>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            return parcelList.FindAll(predicate).FindAll(x => x.IsActive);
        }
        #endregion

        public void DeleteParcel(int parcelId)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            int index = parcelList.FindIndex(x => x.Id == parcelId);
            if (index == -1) throw new ObjectNotFoundException("parcel");
            Parcel parcel = parcelList[index];
            parcel.IsActive = false;
            parcelList[index] = parcel;
            XMLTools.SaveListToXMLSerializer(parcelList, dalParcelPath);
        }
    }
}
