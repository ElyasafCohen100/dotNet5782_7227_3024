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
        #region Find

        /// <summary>
        /// Finds Parcel by specific Id.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        /// <returns> Parcel object </returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public Parcel FindParcelById(int parcelId)
        {
            string dalParcelPath = @"Parcels.xml";
            XElement dalParcelsRoot = XElement.Load(dalParcelPath);

            Parcel dalParcel = (from parcel in dalParcelsRoot.Element("parcel").Elements()
                                where Convert.ToInt32(parcel.Element("id").Value) == parcelId
                                select new Parcel
                                {
                                    Id = Convert.ToInt32(parcel.Element("id").Value),
                                    SenderId = Convert.ToInt32(parcel.Element("senderId").Value),
                                    TargetId = Convert.ToInt32(parcel.Element("targetId").Value),
                                    DroneId = Convert.ToInt32(parcel.Element("droneId").Value),
                                    Priority = (Priorities)Convert.ToInt32(parcel.Element("priority").Value),
                                    Weight = (WeightCategories)Convert.ToInt32(parcel.Element("weight").Value),
                                    Requested = Convert.ToDateTime(parcel.Element("requested").Value),
                                    Scheduled = Convert.ToDateTime(parcel.Element("scheduledd").Value),
                                    PickedUp = Convert.ToDateTime(parcel.Element("pickedUp").Value),
                                    Delivered = Convert.ToDateTime(parcel.Element("delivered").Value),
                                    IsActive = Convert.ToBoolean(parcel.Element("isActive").Value)
                                }).FirstOrDefault();
            dalParcelsRoot.Save(dalParcelPath);

            return dalParcel.Id != parcelId && dalParcel.IsActive == false ? throw new ObjectNotFoundException("parcel") : dalParcel;
        }
        #endregion

        #region Setters
        /// <summary>
        /// Set new Parcel.
        /// </summary>
        /// <param name="parcel"> Parcel object </param>
        public void SetNewParcel(Parcel parcel)
        {
            string dalParcelPath = @"Parcels.xml";
            string dalConfigPath = @"dal-config.xml";

            XElement dalParcelsRoot = XElement.Load(dalParcelPath);
            XElement dalConfigRoot = XElement.Load(dalConfigPath);

            parcel.Id = Convert.ToInt32(dalConfigRoot.Element("SerialNum").Value);
            parcel.IsActive = true;

            dalParcelsRoot.Add(parcel);

            int serialNum = Convert.ToInt32(dalConfigRoot.Element("serialNum").Value);
            serialNum++;
            dalConfigRoot.Element("serialNum").Value = serialNum.ToString();

            dalConfigRoot.Save(dalConfigPath);
            dalParcelsRoot.Save(dalParcelPath);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Parcel status to picked up.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        public void UpdatePickedUpParcelById(int parcelId)
        {
            try
            {
                string dalParcelPath = @"Parcels.xml";
                XElement dalParcelsRoot = XElement.Load(dalParcelPath);

                XElement dalParcel = (from parcel in dalParcelsRoot.Element("parcel").Elements()
                                      where XmlConvert.ToInt32(parcel.Element("id").Value) == parcelId
                                      select parcel).FirstOrDefault();

                if (dalParcel == null) throw new ObjectNotFoundException("parcel");

                DateTime? pickedUp = DateTime.Now;
                dalParcel.Element("pickedUp").Value = pickedUp.ToString();
                dalParcelsRoot.Save(dalParcelPath);
            }
            catch (ObjectNotFoundException)
            {
                throw;
            }
        }


        /// <summary>
        /// Update Parcel status to Delivered. 
        /// </summary>
        /// <param name="parcelId"> Parcel Id</param>
        public void UpdateDeliveredParcelById(int parcelId)
        {
            try
            {
                string dalParcelPath = @"Parcels.xml";
                XElement dalParcelsRoot = XElement.Load(dalParcelPath);

                XElement dalParcel = (from parcel in dalParcelsRoot.Element("parcel").Elements()
                                      where XmlConvert.ToInt32(parcel.Element("id").Value) == parcelId
                                      select parcel).FirstOrDefault();

                if (dalParcel == null) throw new ObjectNotFoundException("parcel");

                DateTime? delivered = DateTime.Now;
                dalParcel.Element("delivered").Value = delivered.ToString();
                dalParcelsRoot.Save(dalParcelPath);
            }
            catch (ObjectNotFoundException)
            {
                throw;
            }
        }

        /// <summary>
        /// Update Drone Id of Parcel.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        /// <param name="droneId"> Id of Drone </param>
        public void UpdateDroneIdOfParcel(int parcelId, int droneId)
        {
            try
            {
                string dalParcelPath = @"Parcels.xml";
                XElement dalParcelsRoot = XElement.Load(dalParcelPath);

                XElement dalParcel = (from parcel in dalParcelsRoot.Element("parcel").Elements()
                                      where XmlConvert.ToInt32(parcel.Element("id").Value) == parcelId
                                      select parcel).FirstOrDefault();

                if (dalParcel == null) throw new ObjectNotFoundException("parcel");

                dalParcel.Element("droneId").Value = droneId.ToString();
                DateTime? scheduled = DateTime.Now;
                dalParcel.Element("scheduled").Value = scheduled.ToString();

                dalParcelsRoot.Save(dalParcelPath);
            }
            catch (ObjectNotFoundException)
            {
                throw;
            }
        }
        #endregion

        #region Getters
        /// <summary>
        /// Return List of Parcels.
        /// </summary>
        /// <returns>List of Parcels </returns>
        public IEnumerable<Parcel> GetParcelList()
        {
            string dalParcelPath = @"Parcels.xml";
            XElement dalParcelsRoot = XElement.Load(dalParcelPath);

            return from parcel in dalParcelsRoot.Element("parcel").Elements()
                   where XmlConvert.ToBoolean(parcel.Element("isActive").Value)
                   select new Parcel
                   {
                       Id = Convert.ToInt32(parcel.Element("id").Value),
                       SenderId = Convert.ToInt32(parcel.Element("senderId").Value),
                       TargetId = Convert.ToInt32(parcel.Element("targetId").Value),
                       DroneId = Convert.ToInt32(parcel.Element("droneId").Value),
                       Priority = (Priorities)Convert.ToInt32(parcel.Element("priority").Value),
                       Weight = (WeightCategories)Convert.ToInt32(parcel.Element("weight").Value),
                       Requested = Convert.ToDateTime(parcel.Element("requested").Value),
                       Scheduled = Convert.ToDateTime(parcel.Element("scheduledd").Value),
                       PickedUp = Convert.ToDateTime(parcel.Element("pickedUp").Value),
                       Delivered = Convert.ToDateTime(parcel.Element("delivered").Value),
                       IsActive = Convert.ToBoolean(parcel.Element("isActive").Value)
                   };
        }

        /// <summary>
        /// Return List of non associate Parcels.
        /// </summary>
        /// <returns> List of non associate Parcels </returns>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        {
            string dalParcelPath = @"Parcels.xml";
            XElement dalParcelsRoot = XElement.Load(dalParcelPath);
            IEnumerable<Parcel> parcels = from parcel in dalParcelsRoot.Element("parcel").Elements()
                                          where XmlConvert.ToBoolean(parcel.Element("isActive").Value) && predicate.Equals(true)
                                          select new Parcel
                                          {
                                              Id = Convert.ToInt32(parcel.Element("id").Value),
                                              SenderId = Convert.ToInt32(parcel.Element("senderId").Value),
                                              TargetId = Convert.ToInt32(parcel.Element("targetId").Value),
                                              DroneId = Convert.ToInt32(parcel.Element("droneId").Value),
                                              Priority = (Priorities)Convert.ToInt32(parcel.Element("priority").Value),
                                              Weight = (WeightCategories)Convert.ToInt32(parcel.Element("weight").Value),
                                              Requested = Convert.ToDateTime(parcel.Element("requested").Value),
                                              Scheduled = Convert.ToDateTime(parcel.Element("scheduledd").Value),
                                              PickedUp = Convert.ToDateTime(parcel.Element("pickedUp").Value),
                                              Delivered = Convert.ToDateTime(parcel.Element("delivered").Value),
                                              IsActive = Convert.ToBoolean(parcel.Element("isActive").Value)
                                          };
            dalParcelsRoot.Save(dalParcelPath);
            return parcels;
        }
        #endregion

        #region Delete
        public void DeleteParcel(int parcelId)
        {
            string dalParcelPath = @"Parcels.xml";
            XElement dalParcelsRoot = XElement.Load(dalParcelPath);

            XElement dalParcel = (from customer in dalParcelsRoot.Element("parcel").Elements()
                                  where XmlConvert.ToInt32(customer.Element("id").Value) == parcelId
                                  select customer).FirstOrDefault();
            dalParcel.Remove();
            dalParcelsRoot.Save(dalParcelPath);
        }
        #endregion
    }
}
