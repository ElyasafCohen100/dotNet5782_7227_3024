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
        public void AddNewCustomerBL(Customer customer)
        {
            IDAL.DO.Customer newCustomer = new();

            newCustomer.Id = customer.Id;
            newCustomer.Name = customer.Name;
            newCustomer.Phone = customer.Phone;
            newCustomer.Longitude = customer.location.Longitude;
            newCustomer.Lattitude = customer.location.Lattitude;

            dalObject.SetNewCustomer(newCustomer);
        }

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

        public Customer FindCustomerByIdBL(int customerId)
        {

            IDAL.DO.Customer dalCustomer = dalObject.FindCustomerById(customerId);
            ParcelInCustomer ParcelInCustomer = new();

            Customer Customer = new();
            Customer.Id = dalCustomer.Id;
            Customer.Name = dalCustomer.Name;
            Customer.Phone = dalCustomer.Phone;
            Customer.location.Lattitude = dalCustomer.Lattitude;
            Customer.location.Longitude = dalCustomer.Longitude;


            foreach (var parcel in dalObject.GetParcelList())
            {
                if (customerId == parcel.SenderId || customerId == parcel.TargetId)
                {
                    ParcelInCustomer.Id = parcel.Id;
                    ParcelInCustomer.WeightCategory= (WeightCategories)parcel.Weight;
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
