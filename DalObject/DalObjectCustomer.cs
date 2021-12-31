using System;
using System.Collections.Generic;
using System.Linq;
using DO;

namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        #region Find
        /// <summary>
        /// Finds Customer by specific Id.
        /// </summary>
        /// <param name="customerId"> Customer Id </param>
        /// <returns> Customer object </returns>
        public Customer FindCustomerById(int customerId)
        {
            Customer customer = DataSource.Customers.Find(x => x.Id == customerId);
            return customer.Id != customerId && !customer.IsActive ? throw new ObjectNotFoundException(customer.GetType().ToString()) : customer;
        }
        #endregion

        #region Setters
        /// <summary>
        /// Set new Customer.
        /// </summary>
        /// <param name="customer"> Customer object </param>
        public void SetNewCustomer(Customer Customer)
        {
            Customer.IsActive = true;
            DataSource.Customers.Add(Customer);
        }
        public void UpdateCustomerDetailes(int customerId, string newName, string newPhoneNumber)
        {
            int index = DataSource.Customers.FindIndex(x => x.Id == customerId);
            if (index == -1) throw new ObjectNotFoundException("customer");

            Customer customer = DataSource.Customers[index];
            customer.Name = newName;
            customer.Phone = newPhoneNumber;
            DataSource.Customers[index] = customer;
        }
        #endregion

        #region Getters
        /// <summary>
        /// Return List of Customers.
        /// </summary>
        /// <returns> List of Customers </returns>
        public IEnumerable<Customer> GetCustomerList()
        {
            return from customer in DataSource.Customers where customer.IsActive select customer;
        }
        #endregion
        public void DeleteCustomer(int customerId)
        {
            int index = DataSource.Customers.FindIndex(x => x.Id == customerId);
            if (index == -1) throw new ObjectNotFoundException("customer");
            Customer customer = DataSource.Customers[index];
            customer.IsActive = false;
            DataSource.Customers[index] = customer;
        }

        public Customer FindCustomerByUserName(string username)
        {
            return (from customer in GetCustomerList() where customer.UserName == username select customer).FirstOrDefault();
        }
    }
}