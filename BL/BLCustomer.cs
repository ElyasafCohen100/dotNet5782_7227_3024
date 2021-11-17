using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL :IBL.IBL
    {
        public void AddNewCustomerBL(Customer customer)
        {
            IDAL.DO.Customer newCustomer = new();
            IDAL.IDal dalObject = new DalObject.DalObject();

            newCustomer.Id = customer.Id;
            newCustomer.Name = customer.Name;
            newCustomer.Phone = customer.Phone;
            newCustomer.Longitude = customer.location.Longitude;
            newCustomer.Lattitude = customer.location.Lattitude;

            dalObject.SetNewCustomer(newCustomer);
        }
    }
}
