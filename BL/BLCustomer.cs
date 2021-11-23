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
        //--------------------------------- FIND FUNCTIONS ---------------------------------------//

        /// <summary>
        /// find BLcustomer by ID by using DAL
        /// </summary>
        /// <param name="customerId"> the id of customer </param>
        /// <returns> return BL customer object </returns>
        public Customer FindCustomerByIdBL(int customerId)
        {

            IDAL.DO.Customer dalCustomer = dalObject.FindCustomerById(customerId);
            ParcelInCustomer ParcelInCustomer = new();

            Customer Customer = new();
            Customer.Id = dalCustomer.Id;
            Customer.Name = dalCustomer.Name;
            Customer.Phone = dalCustomer.Phone;
            Customer.Location.Lattitude = dalCustomer.Lattitude;
            Customer.Location.Longitude = dalCustomer.Longitude;


            foreach (var parcel in dalObject.GetParcelList())
            {
                if (customerId == parcel.SenderId || customerId == parcel.TargetId)
                {
                    ParcelInCustomer.Id = parcel.Id;
                    ParcelInCustomer.WeightCategory = (WeightCategories)parcel.Weight;
                    ParcelInCustomer.Priority = (Priorities)parcel.Priority;

                    if (parcel.Delivered != DateTime.MinValue)
                        ParcelInCustomer.ParcelStatus = ParcelStatus.Delivered;
                    else if (parcel.PickedUp != DateTime.MinValue)
                        ParcelInCustomer.ParcelStatus = ParcelStatus.PickedUp;
                    else if (parcel.Scheduled != DateTime.MinValue)
                        ParcelInCustomer.ParcelStatus = ParcelStatus.Scheduled;
                    else
                        ParcelInCustomer.ParcelStatus = ParcelStatus.Requested;

                    ParcelInCustomer.Customer.Id = Customer.Id;
                    ParcelInCustomer.Customer.Name = Customer.Name;

                    if (customerId == parcel.SenderId)
                        Customer.ParcelFromCustomerList.Add(ParcelInCustomer);
                    else
                        Customer.ParcelToCustomerList.Add(ParcelInCustomer);
                }
            }
            return Customer;
        }

        //---------------------------------- ADD FUNCTIONS ----------------------------------------//

        /// <summary>
        /// add BL customer by using DAL
        /// </summary>
        /// <param name="customer"> the customer </param>
        public void AddNewCustomerBL(Customer customer)
        {
            IDAL.DO.Customer dalCustomer = new();

            dalCustomer.Id = customer.Id;
            dalCustomer.Name = customer.Name;
            dalCustomer.Phone = customer.Phone;
            dalCustomer.Longitude = customer.Location.Longitude;
            dalCustomer.Lattitude = customer.Location.Lattitude;

            dalObject.SetNewCustomer(dalCustomer);
        }

        //--------------------------------- UPDATE FUNCTIONS --------------------------------------//

        /// <summary>
        /// update customer detailes
        /// </summary>
        /// <param name="customerId"> the ID of customer </param>
        /// <param name="newName"> the update name of the customer </param>
        /// <param name="newPhoneNumber"> the new phone of the customer</param>
        public void UpdateCustomerDetailes(int customerId, string newName, string newPhoneNumber)
        {
            IDAL.DO.Customer customer = dalObject.FindCustomerById(customerId);

            if (newName != null)
            {
                customer.Name = newName;
            }

            if (newPhoneNumber != null)
            {
                customer.Phone = newPhoneNumber;
            }
        }

        //---------------------------------- VIEW FUNCTIONS ---------------------------------------//

        /// <summary>
        /// view list of detailes of BL customer
        /// </summary>
        /// <returns>list of detailes of BL customer</returns>
        public IEnumerable<CustomerToList> ViewCustomerToList()
        {
            List<CustomerToList> myCustomerList = new();

            foreach (var customer in dalObject.GetCustomerList())
            {
                CustomerToList myCustomer = new();

                myCustomer.Id = customer.Id;
                myCustomer.Name = customer.Name;
                myCustomer.Phone = customer.Phone;

                foreach (var parcel in dalObject.GetParcelList())
                {
                    if (myCustomer.Id == parcel.SenderId)
                    {
                        if (parcel.Delivered != DateTime.MinValue)
                        {
                            myCustomer.SendAndDeliveredParcels++;
                        }
                        else if (parcel.PickedUp != DateTime.MinValue)
                        {
                            myCustomer.SendAndNotDeliveredParcels++;
                        }
                    }
                    else if (myCustomer.Id == parcel.TargetId)
                    {
                        if (parcel.Delivered != DateTime.MinValue)
                        {
                            myCustomer.DeliveredParcels++;
                        }
                        else if (parcel.PickedUp != DateTime.MinValue)
                        {
                            myCustomer.PickedUpParcels++;
                        }
                    }

                }
                myCustomerList.Add(myCustomer);
            }
            return myCustomerList;
        }
    }
}
