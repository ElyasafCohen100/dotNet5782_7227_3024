﻿using System.Collections.Generic;
using DO;

namespace Dal
{
    public partial class DalObject : DalApi.IDal
    {
        #region FIND
        //----------------------- FIND FUNCTIONS -----------------------//
        /// <summary>
        /// Finds Customer by specific Id.
        /// </summary>
        /// <param name="customerId"> Customer Id </param>
        /// <returns> Customer object </returns>
        public Customer FindCustomerById(int customerId)
        {
            Customer customer = DataSource.Customers.Find(x => x.Id == customerId);
            return customer.Id != customerId ? throw new ObjectNotFoundException(customer.GetType().ToString()) : customer;
        }
        #endregion

        #region SET
        //-------------------------- SETTERS --------------------------//
        /// <summary>
        /// Set new Customer.
        /// </summary>
        /// <param name="customer"> Customer object </param>
        public void SetNewCustomer(Customer Customer)
        {
            DataSource.Customers.Add(Customer);
        }
        #endregion

        #region UPDATE
        public void UpdateCustomerDetailes(int customerId, string newName, string newPhoneNumber)
        {
            int index = DataSource.Customers.FindIndex(x => x.Id == customerId);
            if (index == -1) throw new ObjectNotFoundException("customer");

            Customer customer = DataSource.Customers[index];
            customer.Name = newName;
            customer.Phone = newPhoneNumber;
            DataSource.Customers[index] = customer;
        }
        //-------------------------- GETTERS --------------------------//
        /// <summary>
        /// Return List of Customers.
        /// </summary>
        /// <returns> List of Customers </returns>
        public IEnumerable<Customer> GetCustomerList()
        {
            return DataSource.Customers;
        }
        #endregion
    }
}