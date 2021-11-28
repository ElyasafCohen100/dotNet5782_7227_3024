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
       
        //---------------------------------- ADD FUNCTIONS ----------------------------------------//

        /// <summary>
        /// add BL customer by using DAL
        /// </summary>
        /// <param name="customer"> the customer </param>
        /// <exception cref="InvalidInputException">Thrown if one of the customer details are invalid</exception>
        public void AddNewCustomerBL(Customer customer)
        {
            IDAL.DO.Customer dalCustomer = new();
            if (dalCustomer.Id < 100000000 || dalCustomer.Id >= 1000000000) throw new InvalidInputException("Id");
            if (dalCustomer.Phone == null) throw new InvalidInputException("Phone number");
            IfExistCustomer(customer);
            if (dalCustomer.Name == null) throw new InvalidInputException("Name");
            if (dalCustomer.Longitude == 0.0) throw new InvalidInputException("Longitude");
            if (dalCustomer.Lattitude == 0.0) throw new InvalidInputException("Lattitude");

            dalCustomer.Id = customer.Id;
            dalCustomer.Name = customer.Name;
            dalCustomer.Phone = customer.Phone;
            dalCustomer.Longitude = customer.Location.Longitude;
            dalCustomer.Lattitude = customer.Location.Latitude;

            dalObject.SetNewCustomer(dalCustomer);
        }

        /// <summary>
        /// check if the customer is already exist
        /// </summary>
        /// <param name="customer">The customer</param>
        /// <exception cref="ObjectAlreadyExistException">Thrown if customer id or cusomer phone is already exist</exception>
        static void IfExistCustomer(Customer customer)
        {
            foreach (var myCustomer in dalObject.GetCustomerList())
            {
                if (customer.Id == myCustomer.Id) throw new ObjectAlreadyExistException("customer");
                if (customer.Phone == myCustomer.Phone) throw new ObjectAlreadyExistException("phone");
            }
        }

        //--------------------------------- UPDATE FUNCTIONS --------------------------------------//

        /// <summary>
        /// update customer detailes
        /// </summary>
        /// <param name="customerId"> the ID of customer </param>
        /// <param name="newName"> the update name of the customer </param>
        /// <param name="newPhoneNumber"> the new phone of the customer</param>
        /// <exception cref="InvalidInputException">Thrown if customer id or cusomer phone or customer name is invalid</exception>
        /// <exception cref="ObjectNotFoundException">Throw if customer with such id has not found</exception>
        public void UpdateCustomerDetailes(int customerId, string newName, string newPhoneNumber)
        {
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("Id");
            if (newName == null) throw new InvalidInputException("Name");
            if (newPhoneNumber == null) throw new InvalidInputException("Phone number");

            try
            {
                IDAL.DO.Customer customer = dalObject.FindCustomerById(customerId);
                customer.Name = newName;
                customer.Phone = newPhoneNumber;

            }
            catch (IDAL.DO.ObjectNotFoundException)
            {

                throw new ObjectNotFoundException("Customer");
            }
        }

        //--------------------------------- FIND FUNCTIONS ---------------------------------------//

        /// <summary>
        /// find BLcustomer by ID by using DAL
        /// </summary>
        /// <param name="customerId"> the id of customer </param>
        /// <returns> return BL customer object </returns>
        /// <exception cref="InvalidInputException">Thrown if customer id is invalid</exception>
        /// <exception cref="ObjectNotFoundException">Throw if customer with such id has not found</exception>
        public Customer FindCustomerByIdBL(int customerId)
        {
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("Id");

            Customer Customer = new();
            try
            {
                IDAL.DO.Customer dalCustomer = dalObject.FindCustomerById(customerId);
                Customer.Id = dalCustomer.Id;
                Customer.Name = dalCustomer.Name;
                Customer.Phone = dalCustomer.Phone;
                Customer.Location.Latitude = dalCustomer.Lattitude;
                Customer.Location.Longitude = dalCustomer.Longitude;
            }
            catch (IDAL.DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("Customer");
            }

            IEnumerable<IDAL.DO.Parcel> dalParcelsList = dalObject.GetParcelList();
            if (dalParcelsList.Count() > 0)
            {
                foreach (var parcel in dalParcelsList)
                {
                    if (customerId == parcel.SenderId || customerId == parcel.TargetId)
                    {
                        ParcelInCustomer ParcelInCustomer = new();
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
            }
            return Customer;
        }

        //---------------------------------- VIEW FUNCTIONS ---------------------------------------//

        /// <summary>
        /// view list of detailes of BL customer
        /// </summary>
        /// <returns>list of detailes of BL customer</returns>
        public IEnumerable<CustomerToList> ViewCustomerToList()
        {
            List<CustomerToList> myCustomerList = new();

            IEnumerable<IDAL.DO.Customer> dalCustomers = dalObject.GetCustomerList();
            if (dalCustomers.Count() > 0)
            {
                IEnumerable<IDAL.DO.Parcel> dalParcels = dalObject.GetParcelList();
                
                foreach (var customer in dalCustomers)
                {
                    CustomerToList myCustomer = new();

                    myCustomer.Id = customer.Id;
                    myCustomer.Name = customer.Name;
                    myCustomer.Phone = customer.Phone;

                    if (dalParcels.Count() > 0)
                    {
                        foreach (var parcel in dalParcels)
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
                    }
                    myCustomerList.Add(myCustomer);
                }
            }
            return myCustomerList;
        }
    }
}
