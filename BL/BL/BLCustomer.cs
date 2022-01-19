using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using BO;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        #region Get
        /// <summary>
        /// check if the customer is registered in the Customer's list
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True or false depends on the case</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsCustomerRegistered(string username, string password)
        {
            lock (dalObject)
            {
                DO.Customer customer = dalObject.GetCustomerByUserName(username);
                if (customer.UserName == username && customer.Password == password)
                    return true;
                return false;
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsCustomerExsist(string username)
        {
            lock (dalObject)
            {
                DO.Customer customer = dalObject.GetCustomerByUserName(username);
                if (customer.UserName == username)
                    return true;
                return false;
            }
        }


        /// <summary>
        /// get the customer by the username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>The relevant customer</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomerByUserName(string userName)
        {
            lock (dalObject)
            {
                DO.Customer dalCustomer = dalObject.GetCustomerByUserName(userName);
                Customer customer = GetCustomerByIdBL(dalCustomer.Id);
                customer.UserName = dalCustomer.UserName;
                customer.Password = dalCustomer.Password;
                return customer;
            }
        }


        /// <summary>
        /// Find BL customer by ID by using DAL.
        /// </summary>
        /// <param name="customerId"> Customer Id </param>
        /// <returns> BL customer object </returns>
        /// <exception cref="InvalidInputException"> Thrown if customer id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Throw if customer with such id has not found </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomerByIdBL(int customerId)
        {
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("Id");

            Customer Customer = new();
            try
            {
                lock (dalObject)
                {
                    DO.Customer dalCustomer = dalObject.GetCustomerById(customerId);
                    Customer.Id = dalCustomer.Id;
                    Customer.Name = dalCustomer.Name;
                    Customer.Phone = dalCustomer.Phone;
                    Customer.Location.Latitude = dalCustomer.Latitude;
                    Customer.Location.Longitude = dalCustomer.Longitude;
                    Customer.UserName = dalCustomer.UserName;
                    Customer.Password = dalCustomer.Password;
                }
            }
            catch (DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("Customer");
            }
            lock (dalObject)
            {
                IEnumerable<DO.Parcel> dalParcelsList = dalObject.GetParcelList();
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

                            if (parcel.Delivered != null)
                                ParcelInCustomer.ParcelStatus = ParcelStatus.Delivered;
                            else if (parcel.PickedUp != null)
                                ParcelInCustomer.ParcelStatus = ParcelStatus.PickedUp;
                            else if (parcel.Scheduled != null)
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
            }
            return Customer;
        }


        /// <summary>
        /// get the object "custome to list" by customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>the relevant customerToList</returns>
        /// <exception cref="InvalidInputException">throw if we get invalid input</exception>
        /// <exception cref="ObjectNotFoundException">throw if the customer has't been found</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public CustomerToList GetCustomerToList(int customerId)
        {
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("Id");

            CustomerToList customer = new();
            try
            {
                lock (dalObject)
                {
                    DO.Customer dalCustomer = dalObject.GetCustomerById(customerId);
                    customer.Id = dalCustomer.Id;
                    customer.Name = dalCustomer.Name;
                    customer.Phone = dalCustomer.Phone;
                }
            }
            catch (DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("Customer");
            }

            // Calculate the rest of the fields of the CoustomerToList object
            int sendAndDelivered = 0;
            int sendAndNotDelivered = 0;
            int pickedUpParcels = 0;
            int deliveredParcels = 0;

            lock (dalObject)
            {
                IEnumerable<DO.Parcel> dalParcelsList = dalObject.GetParcelList();
                if (dalParcelsList.Count() > 0)
                {
                    foreach (var parcel in dalParcelsList)
                    {
                        if (customerId == parcel.SenderId)
                        {
                            if (parcel.Delivered != null)
                                sendAndDelivered++;
                            else if (parcel.PickedUp != null)
                                sendAndNotDelivered++;
                        }
                        else if (customerId == parcel.TargetId)
                        {
                            if (parcel.Delivered != null)
                                deliveredParcels++;
                            else if (parcel.PickedUp != null)
                                pickedUpParcels++;
                        }
                    }
                }

                customer.SendAndDeliveredParcels = sendAndDelivered;
                customer.SendAndNotDeliveredParcels = sendAndNotDelivered;
                customer.PickedUpParcels = pickedUpParcels;
                customer.DeliveredParcels = deliveredParcels;
                
                return customer;
            }
        }


        /// <summary>
        /// View list of detailes of BL customer.
        /// </summary>
        /// <returns> List of detailes of BL customer </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetAllCustomerToList()
        {
            List<CustomerToList> myCustomerList = new();
            lock (dalObject)
            {
                IEnumerable<DO.Customer> dalCustomers = dalObject.GetCustomerList();
                if (dalCustomers.Count() > 0)
                {
                    IEnumerable<DO.Parcel> dalParcels = dalObject.GetParcelList();

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
                                    if (parcel.Delivered != null)
                                    {
                                        myCustomer.SendAndDeliveredParcels++;
                                    }
                                    else if (parcel.PickedUp != null)
                                    {
                                        myCustomer.SendAndNotDeliveredParcels++;
                                    }
                                }
                                else if (myCustomer.Id == parcel.TargetId)
                                {
                                    if (parcel.Delivered != null)
                                    {
                                        myCustomer.DeliveredParcels++;
                                    }
                                    else if (parcel.PickedUp != null)
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
        #endregion
        
        
        #region Add
        /// <summary>
        /// Add new BL customer by using DAL.
        /// </summary>
        /// <param name="customer"> Customer object </param>
        /// <exception cref="InvalidInputException"> Thrown if one of the customer details are invalid </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewCustomerBL(Customer customer)
        {
            if (customer.Id < 100000000 || customer.Id >= 1000000000) throw new InvalidInputException("Id");
            if (customer.Phone == null) throw new InvalidInputException("phone number");
            long.TryParse(customer.Phone, out long phoneNumber);
            if (phoneNumber < 1000000000 || phoneNumber > 1000000000000) throw new InvalidInputException("phone number");
            IfExistCustomer(customer);
            if (customer.Name == null) throw new InvalidInputException("name");
            if (customer.Location.Longitude == 0.0) throw new InvalidInputException("longitude");
            if (customer.Location.Latitude == 0.0) throw new InvalidInputException("lattitude");

            DO.Customer dalCustomer = new();
            dalCustomer.Id = customer.Id;
            dalCustomer.Name = customer.Name;
            dalCustomer.Phone = customer.Phone;
            dalCustomer.Longitude = customer.Location.Longitude;
            dalCustomer.Latitude = customer.Location.Latitude;
            dalCustomer.UserName = customer.UserName;
            dalCustomer.Password = customer.Password;

            lock (dalObject)
            {
                dalObject.AddNewCustomer(dalCustomer);
            }
        }


        /// <summary>
        /// Check if the customer is already exist.
        /// </summary>
        /// <param name="customer">Customer object </param>
        /// <exception cref="ObjectAlreadyExistException"> Thrown if customer id or cusomer phone is already exist </exception>
        static void IfExistCustomer(Customer customer)
        {
            lock (dalObject)
            {
                foreach (var myCustomer in dalObject.GetCustomerList())
                {
                    if (customer.Id == myCustomer.Id) throw new ObjectAlreadyExistException("customer");
                    if (customer.Phone == myCustomer.Phone) throw new ObjectAlreadyExistException("phone");
                }
            }
        }
        #endregion

        
        #region Update
        /// <summary>
        /// Update customer detailes.
        /// </summary>
        /// <param name="customerId"> Customer Id </param>
        /// <param name="newName"> New name of the customer </param>
        /// <param name="newPhoneNumber"> New phone of the customer</param>
        /// <exception cref="InvalidInputException"> Thrown if customer id or cusomer phone or customer name is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Throw if customer with such id has not found </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerDetailesBL(int customerId, string newName, string newPhoneNumber)
        {
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("Id");
            if (newName == null) throw new InvalidInputException("Name");
            if (newPhoneNumber == null) throw new InvalidInputException("Phone number");

            try
            {
                lock (dalObject)
                {
                    dalObject.UpdateCustomerDetailes(customerId, newName, newPhoneNumber);
                }

            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }
        }
        #endregion


        #region Delete
        /// <summary>
        /// Delete customer by received Id.
        /// </summary>
        /// <param name="customerId"> customer Id</param>
        /// <exception cref="InvalidOperationException">throw if we do invalid operation</exception>
        /// <exception cref="ObjectIsNotActiveException">throw if the Customer has been daleted </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int customerId)
        {
            try
            {
                Customer customer = GetCustomerByIdBL(customerId);
                var parcelList1 = from parcel in customer.ParcelFromCustomerList
                                  where parcel.ParcelStatus == ParcelStatus.Scheduled ||
                                          parcel.ParcelStatus == ParcelStatus.PickedUp
                                  select parcel;

                var parcelList2 = from parcel in customer.ParcelToCustomerList
                                  where parcel.ParcelStatus == ParcelStatus.Scheduled ||
                                         parcel.ParcelStatus == ParcelStatus.PickedUp
                                  select parcel;

                if (parcelList1.Count() > 0 || parcelList2.Count() > 0)
                    throw new InvalidOperationException("Could not delete customer,because the customer have parcels in shipment");
                lock (dalObject)
                {
                    //Delete all the sendered parcels by this customer.
                    foreach (var parcel in customer.ParcelFromCustomerList)
                    {
                        dalObject.DeleteParcel(parcel.Id);
                    }

                    //Delete all the received parcels to this customer.
                    foreach (var parcel in customer.ParcelToCustomerList)
                    {
                        dalObject.DeleteParcel(parcel.Id);
                    }

                    dalObject.DeleteCustomer(customerId);
                }
            }
            catch (DO.ObjectIsNotActiveException e)
            {
                throw new ObjectIsNotActiveException(e.Message);
            }
        }
        #endregion
    }
}
