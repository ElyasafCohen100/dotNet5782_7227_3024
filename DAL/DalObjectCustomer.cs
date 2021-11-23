﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        //----------------------- FIND FUNCTIONS -----------------------//
        /// <summary>
        /// Finds Customer by specific Id.
        /// </summary>
        /// <param name="customerId"> Id of customer </param>
        /// <returns> Customer object </returns>
        public Customer FindCustomerById(int customerId)
        {
            Customer customer = DataSource.Customers.Find(x => x.Id == customerId);
            if (customer.Id != customerId) throw new IDAL.DO.RequiredObjectIsNotFoundException();
            return customer;
        }

        //-------------------------- SETTERS --------------------------//
        /// <summary>
        /// Set new Customer.
        /// </summary>
        /// <param name="customer"> Customer object </param>
        public void SetNewCustomer(Customer Customer)
        {
            DataSource.Customers.Add(Customer);
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
    }
}