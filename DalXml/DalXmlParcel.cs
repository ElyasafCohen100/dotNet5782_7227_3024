using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
    {
        #region Get
        /// <summary>
        /// Finds Parcel by specific Id.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        /// <returns> Parcel object </returns>
        /// <exception cref="ObjectNotFoundException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcelById(int parcelId)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            Parcel parcel = parcelList.Find(x => x.Id == parcelId);
            return parcel.Id != parcelId || parcel.IsActive == false ? throw new ObjectNotFoundException("parcel") : parcel;
        }


        /// <summary>
        /// Return List of Parcels.
        /// </summary>
        /// <returns>List of Parcels </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcelList()
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            return from parcel in parcelList where parcel.IsActive select parcel;
        }


        /// <summary>
        /// Return List of non associate Parcels.
        /// </summary>
        /// <returns> List of non associate Parcels </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            return parcelList.FindAll(predicate).FindAll(x => x.IsActive);
        }
        #endregion


        #region Add
        /// <summary>
        /// Set new Parcel.
        /// </summary>
        /// <param name="parcel"> Parcel object </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewParcel(Parcel parcel)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(dalParcelPath);
            XElement dalConfigRoot = XElement.Load(dalConfigPath);

            parcel.Id = Convert.ToInt32(dalConfigRoot.Element("SerialNum").Value);
            parcel.IsActive = true;
            parcelList.Add(parcel);

            dalConfigRoot.Element("SerialNum").Value = (parcel.Id + 1).ToString();
            XMLTools.SaveListToXMLSerializer(parcelList, dalParcelPath);
            dalConfigRoot.Save(dalConfigPath);
        }
        #endregion


        #region Update
        /// <summary>
        /// Update Parcel status to picked up.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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


        #region Delete
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        #endregion
    }
}
