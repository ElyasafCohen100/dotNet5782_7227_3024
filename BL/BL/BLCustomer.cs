using System;
using System.Collections.Generic;
using System.Linq;
using BO;

namespace BL
{
    public partial class BL : BlApi.IBL
    {

        #region Add

        /// <summary>
        /// Add new BL customer by using DAL.
        /// </summary>
        /// <param name="customer"> Customer object </param>
        /// <exception cref="InvalidInputException"> Thrown if one of the customer details are invalid </exception>
        public void AddNewCustomerBL(Customer customer)
        {
            if (customer.Id < 100000000 || customer.Id >= 1000000000) throw new InvalidInputException("Id");
            if (customer.Phone == null) throw new InvalidInputException("Phone number");
            IfExistCustomer(customer);
            if (customer.Name == null) throw new InvalidInputException("Name");
            if (customer.Location.Longitude == 0.0) throw new InvalidInputException("Longitude");
            if (customer.Location.Latitude == 0.0) throw new InvalidInputException("Lattitude");

            DO.Customer dalCustomer = new();
            dalCustomer.Id = customer.Id;
            dalCustomer.Name = customer.Name;
            dalCustomer.Phone = customer.Phone;
            dalCustomer.Longitude = customer.Location.Longitude;
            dalCustomer.Lattitude = customer.Location.Latitude;

            dalObject.SetNewCustomer(dalCustomer);
        }

        /// <summary>
        /// Check if the customer is already exist.
        /// </summary>
        /// <param name="customer">Customer object </param>
        /// <exception cref="ObjectAlreadyExistException"> Thrown if customer id or cusomer phone is already exist </exception>
        static void IfExistCustomer(Customer customer)
        {
            foreach (var myCustomer in dalObject.GetCustomerList())
            {
                if (customer.Id == myCustomer.Id) throw new ObjectAlreadyExistException("customer");
                if (customer.Phone == myCustomer.Phone) throw new ObjectAlreadyExistException("phone");
            }

        }
        #endregion

        public void DeleteCustomer(int customerId)
        {
            try
            {
                dalObject.DeleteCustomer(customerId);
            }
            catch (DO.ObjectIsNotActiveException e)
            {
                throw new ObjectIsNotActiveException(e.Message);
            }
        }

        #region Update

        /// <summary>
        /// Update customer detailes.
        /// </summary>
        /// <param name="customerId"> Customer Id </param>
        /// <param name="newName"> New name of the customer </param>
        /// <param name="newPhoneNumber"> New phone of the customer</param>
        /// <exception cref="InvalidInputException"> Thrown if customer id or cusomer phone or customer name is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Throw if customer with such id has not found </exception>
        public void UpdateCustomerDetailesBL(int customerId, string newName, string newPhoneNumber)
        {
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("Id");
            if (newName == null) throw new InvalidInputException("Name");
            if (newPhoneNumber == null) throw new InvalidInputException("Phone number");

            try
            {
                dalObject.UpdateCustomerDetailes(customerId, newName, newPhoneNumber);

            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }
        }
        #endregion

        #region Find

        /// <summary>
        /// Find BL customer by ID by using DAL.
        /// </summary>
        /// <param name="customerId"> Customer Id </param>
        /// <returns> BL customer object </returns>
        /// <exception cref="InvalidInputException"> Thrown if customer id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Throw if customer with such id has not found </exception>
        public Customer FindCustomerByIdBL(int customerId)
        {
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("Id");

            Customer Customer = new();
            try
            {
                DO.Customer dalCustomer = dalObject.FindCustomerById(customerId);
                Customer.Id = dalCustomer.Id;
                Customer.Name = dalCustomer.Name;
                Customer.Phone = dalCustomer.Phone;
                Customer.Location.Latitude = dalCustomer.Lattitude;
                Customer.Location.Longitude = dalCustomer.Longitude;
            }
            catch (DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("Customer");
            }

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
            return Customer;
        }

        public bool IsCustomerRegisered(string username)
        {
            if (dalObject.FindCustomerByUserName(username).UserName == username)
                return true;
            return false;
        }

        public CustomerToList FindCustomerToList(int customerId)
        {
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("Id");

            CustomerToList customer = new();
            try
            {
                DO.Customer dalCustomer = dalObject.FindCustomerById(customerId);
                customer.Id = dalCustomer.Id;
                customer.Name = dalCustomer.Name;
                customer.Phone = dalCustomer.Phone;
            }
            catch (DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("Customer");
            }
            int sendAndDelivered = 0;
            int sendAndNotDelivered = 0;
            int pickedUpParcels = 0;
            int deliveredParcels = 0;

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
                customer.SendAndDeliveredParcels = sendAndDelivered;
                customer.SendAndNotDeliveredParcels = sendAndNotDelivered;
                customer.PickedUpParcels = pickedUpParcels;
                customer.DeliveredParcels = deliveredParcels;
            }
            return customer;
        }

        #endregion

        #region View

        /// <summary>
        /// View list of detailes of BL customer.
        /// </summary>
        /// <returns> List of detailes of BL customer </returns>
        public IEnumerable<CustomerToList> ViewCustomerToList()
        {
            List<CustomerToList> myCustomerList = new();

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
        #endregion
    }
}
