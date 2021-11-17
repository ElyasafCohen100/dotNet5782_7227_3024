using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
   public partial class BL : IBL.IBL
    {
        public void AddNewParcelBL(Parcel parcel)
        {
            IDAL.DO.Parcel newParcel = new();
            IDAL.IDal dalObject = new DalObject.DalObject();

            newParcel.SenderId = parcel.senderCustomer.Id;
            newParcel.TargetId = parcel.receiverCustomer.Id;
            newParcel.Weight = (IDAL.DO.WeightCategories)parcel.Weight;
            newParcel.Priority = (IDAL.DO.Priorities)parcel.Priority;

            newParcel.Requested = DateTime.Now;
            newParcel.Scheduled = DateTime.MinValue;
            newParcel.PickedUp = DateTime.MinValue;
            newParcel.Delivered = DateTime.MinValue;
        }
    }
}
