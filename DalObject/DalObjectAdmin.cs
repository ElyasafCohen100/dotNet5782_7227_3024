using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;

namespace Dal
{
    public partial class DalObject : DalApi.IDal
    {
        #region Get
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Admin> GetAdminsList()
        {
            return from admin in DataSource.Admins select admin;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Admin GetAdminByUserName(string userName)
        {
            return (from admin in GetAdminsList() where admin.UserName == userName select admin).FirstOrDefault();
        }
        #endregion


        #region Add
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddAdmin(Admin admin)
        {
            DataSource.Admins.Add(admin);
        }
        #endregion


        #region Delete
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteAdmin(string userName)
        {
            int index = DataSource.Admins.FindIndex(x => x.UserName == userName);
            if (index == -1) throw new ObjectNotFoundException("Admin");
            Admin admin = DataSource.Admins[index];
            DataSource.Admins.Remove(admin);
        }
        #endregion


        #region Update
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateAdminPassword(Admin newAdmin)
        {
            int index = DataSource.Admins.FindIndex(x => x.UserName == newAdmin.UserName);
            if (index == -1) throw new ObjectNotFoundException("Admin");
            DataSource.Admins[index] = newAdmin;
        }
        #endregion
    }
}
