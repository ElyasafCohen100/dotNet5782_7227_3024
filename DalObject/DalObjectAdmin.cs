using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DO;

namespace Dal
{
    public partial class DalObject : DalApi.IDal
    {
        #region Get
        /// <summary>
        /// get admin list
        /// </summary>
        /// <returns>the admin List</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Admin> GetAdminsList()
        {
            return from admin in DataSource.Admins select admin;
        }

        /// <summary>
        /// get the admin by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>the relevant admin </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Admin GetAdminByUserName(string userName)
        {
            return (from admin in GetAdminsList() where admin.UserName == userName select admin).FirstOrDefault();
        }
        #endregion


        #region Add
        /// <summary>
        /// add admin
        /// </summary>
        /// <param name="admin"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddAdmin(Admin admin)
        {
            DataSource.Admins.Add(admin);
        }
        #endregion


        #region Update
        /// <summary>
        /// update the admin's password
        /// </summary>
        /// <param name="newAdmin"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateAdminPassword(Admin newAdmin)
        {
            int index = DataSource.Admins.FindIndex(x => x.UserName == newAdmin.UserName);
            if (index == -1) throw new ObjectNotFoundException("Admin");
            DataSource.Admins[index] = newAdmin;
        }
        #endregion


        #region Delete
        /// <summary>
        /// delete admin
        /// </summary>
        /// <param name="userName"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteAdmin(string userName)
        {
            int index = DataSource.Admins.FindIndex(x => x.UserName == userName);
            if (index == -1) throw new ObjectNotFoundException("Admin");
            Admin admin = DataSource.Admins[index];
            DataSource.Admins.Remove(admin);
        }
        #endregion
    }
}
