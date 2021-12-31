using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    public partial class DalObject : DalApi.IDal
    {
        public void SetAdmin(Admin admin)
        {
            DataSource.Admins.Add(admin);
        }
        public void DeleteAdmin(string userName)
        {
            int index = DataSource.Admins.FindIndex(x => x.UserName == userName);
            if (index == -1) throw new ObjectNotFoundException("Admin");
            Admin admin = DataSource.Admins[index];
            DataSource.Admins.Remove(admin);
        }
        public IEnumerable<Admin> GetAdminsList()
        {
            return from admin in DataSource.Admins select admin;
        }
        public void UpdateAdminPassword(Admin newAdmin)
        {
            int index = DataSource.Admins.FindIndex(x => x.UserName == newAdmin.UserName);
            if (index == -1) throw new ObjectNotFoundException("Admin");
            Admin admin = DataSource.Admins[index];
        }

        public Admin FindAdminByUserName(string userName)
        {
            return (from admin in GetAdminsList() where admin.UserName == userName select admin).FirstOrDefault();
        }
    }
}
