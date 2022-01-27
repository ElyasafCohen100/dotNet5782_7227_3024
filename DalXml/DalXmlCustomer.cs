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
        /// Finds Customer by specific Id.
        /// </summary>
        /// <param name="customerId"> Customer Id </param>
        /// <returns> Customer object </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomerById(int customerId)
        {
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);

            Customer dalCustomer = (from customer in dalCustomersRoot.Elements()
                                    where customer.Element("Id").Value == customerId.ToString()
                                    select new Customer
                                    {
                                        Id = customerId,
                                        Name = customer.Element("Name").Value,
                                        Phone = customer.Element("Phone").Value,
                                        Longitude = double.Parse(customer.Element("Longitude").Value),
                                        Latitude = double.Parse(customer.Element("Latitude").Value),
                                        IsActive = bool.Parse(customer.Element("IsActive").Value),
                                        UserName = customer.Element("UserName").Value,
                                        Password = customer.Element("Password").Value
                                    }).FirstOrDefault();

            dalCustomersRoot.Save(dalCustomerPath);

            return dalCustomer.Id != customerId || dalCustomer.IsActive == false ? throw new ObjectNotFoundException(dalCustomer.GetType().ToString()) : dalCustomer;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomerByUserName(string username)
        {
            return (from customer in GetCustomerList() where customer.UserName == username select customer).FirstOrDefault();
        }


        /// <summary>
        /// Return List of Customers.
        /// </summary>
        /// <returns> List of Customers </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomerList()
        {
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);

            return from customer in dalCustomersRoot.Elements()
                   where XmlConvert.ToBoolean(customer.Element("IsActive").Value)
                   select new Customer
                   {
                       Id = XmlConvert.ToInt32(customer.Element("Id").Value),
                       Name = customer.Element("Name").Value,
                       Phone = customer.Element("Phone").Value,
                       Longitude = XmlConvert.ToDouble(customer.Element("Longitude").Value),
                       Latitude = XmlConvert.ToDouble(customer.Element("Latitude").Value),
                       IsActive = XmlConvert.ToBoolean(customer.Element("IsActive").Value),
                       UserName = customer.Element("UserName").Value,
                       Password = customer.Element("Password").Value
                   };
        }
        #endregion


        #region Add
        /// <summary>
        /// Set new Customer.
        /// </summary>
        /// <param name="customer"> Customer object </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewCustomer(Customer Customer)
        {
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);
            Customer.IsActive = true;

            XElement customer = new XElement("Customer",
                                new XElement("Id", Customer.Id),
                                new XElement("Name", Customer.Name),
                                new XElement("Phone", Customer.Phone),
                                new XElement("Longitude", Customer.Longitude),
                                new XElement("Latitude", Customer.Latitude),
                                new XElement("IsActive", Customer.IsActive),
                                new XElement("UserName", Customer.UserName),
                                new XElement("Password", Customer.Password));

            dalCustomersRoot.Add(customer);
            dalCustomersRoot.Save(dalCustomerPath);
        }
        #endregion


        #region Update
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerDetailes(int customerId, string newName, string newPhoneNumber)
        {
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);

            XElement dalCustomer = (from customer in dalCustomersRoot.Elements()
                                    where customer.Element("Id").Value == customerId.ToString()
                                    select customer).FirstOrDefault();
            if (dalCustomer == null) throw new ObjectNotFoundException("Customer");

            dalCustomer.Element("Name").SetValue(newName);
            dalCustomer.Element("Phone").SetValue(newPhoneNumber);

            dalCustomersRoot.Save(dalCustomerPath);
        }
        #endregion


        #region Delete
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int customerId)
        {
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);

            XElement dalCustomer = (from customer in dalCustomersRoot.Elements()
                                    where customer.Element("Id").Value == customerId.ToString()
                                    select customer).FirstOrDefault();
            dalCustomer.Element("IsActive").SetValue(false);
            dalCustomersRoot.Save(dalCustomerPath);
        }
        #endregion
    }
}

