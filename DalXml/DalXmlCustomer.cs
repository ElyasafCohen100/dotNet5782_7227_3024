using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
    {
        #region Find

        /// <summary>
        /// Finds Customer by specific Id.
        /// </summary>
        /// <param name="customerId"> Customer Id </param>
        /// <returns> Customer object </returns>
        public Customer FindCustomerById(int customerId)
        {
            string dalCustomerPath = @"Customers.xml";
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);
            Customer dalCustomer = (from customer in dalCustomersRoot.Element("customer").Elements()
                                    where XmlConvert.ToInt32(customer.Element("id").Value) == customerId
                                    select new Customer
                                    {
                                        Id = customerId,
                                        Name = customer.Element("name").Value,
                                        Phone = customer.Element("phone").Value,
                                        Longitude = XmlConvert.ToDouble(customer.Element("longitude").Value),
                                        Latitude = XmlConvert.ToDouble(customer.Element("latidue").Value),
                                        IsActive = XmlConvert.ToBoolean(customer.Element("isActive").Value),
                                        UserName = customer.Element("userName").Value,
                                        Password = customer.Element("password").Value
                                    }).FirstOrDefault();

            dalCustomersRoot.Save(dalCustomerPath);

            return dalCustomer.Id != customerId && !dalCustomer.IsActive ? throw new ObjectNotFoundException(dalCustomer.GetType().ToString()) : dalCustomer;
        }
        #endregion


        /// <summary>
        /// Set new Customer.
        /// </summary>
        /// <param name="customer"> Customer object </param>
        public void SetNewCustomer(Customer Customer)
        {
            string dalCustomerPath = @"Customers.xml";
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);
            Customer.IsActive = true;

            XElement customer = new XElement("customer",
                                new XElement("id", Customer.Id),
                                new XElement("name", Customer.Name),
                                new XElement("phone", Customer.Phone),
                                new XElement("longitude", Customer.Longitude),
                                new XElement("latitude", Customer.Latitude),
                                new XElement("isActive", Customer.IsActive),
                                new XElement("userName", Customer.UserName),
                                new XElement("password", Customer.Password));

            dalCustomersRoot.Add(customer);
            dalCustomersRoot.Save(dalCustomerPath);
        }
        public void UpdateCustomerDetailes(int customerId, string newName, string newPhoneNumber)
        {
            string dalCustomerPath = @"Customers.xml";
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);

            XElement dalCustomer = (from customer in dalCustomersRoot.Element("customer").Elements()
                                    where XmlConvert.ToInt32(customer.Element("id").Value) == customerId
                                    select customer).FirstOrDefault();
            dalCustomer.Element("customer").Element("name").Value = newName;
            dalCustomer.Element("customer").Element("phone").Value = newPhoneNumber;

            dalCustomersRoot.Save(dalCustomerPath);
        }

        #region Getters
        /// <summary>
        /// Return List of Customers.
        /// </summary>
        /// <returns> List of Customers </returns>
        public IEnumerable<Customer> GetCustomerList()
        {
            string dalCustomerPath = @"Customers.xml";
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);

            return from customer in dalCustomersRoot.Element("customer").Elements()
                   where XmlConvert.ToBoolean(customer.Element("isActive").Value)
                   select new Customer
                   {
                       Id = XmlConvert.ToInt32(customer.Element("id").Value),
                       Name = customer.Element("name").Value,
                       Phone = customer.Element("phone").Value,
                       Longitude = XmlConvert.ToDouble(customer.Element("longitude").Value),
                       Latitude = XmlConvert.ToDouble(customer.Element("latidue").Value),
                       IsActive = XmlConvert.ToBoolean(customer.Element("isActive").Value),
                       UserName = customer.Element("userName").Value,
                       Password = customer.Element("password").Value
                   };
        }
        #endregion
        public void DeleteCustomer(int customerId)
        {
            string dalCustomerPath = @"Customers.xml";
            XElement dalCustomersRoot = XElement.Load(dalCustomerPath);

            XElement dalCustomer = (from customer in dalCustomersRoot.Element("customer").Elements()
                                    where XmlConvert.ToInt32(customer.Element("id").Value) == customerId
                                    select customer).FirstOrDefault();
            dalCustomer.Remove();
            dalCustomersRoot.Save(dalCustomerPath);
        }
        public Customer FindCustomerByUserName(string username)
        {
            return (from customer in GetCustomerList() where customer.UserName == username select customer).FirstOrDefault();
        }
    }
}

