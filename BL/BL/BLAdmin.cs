using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;

namespace BL
{
    public partial class BL
    {
        #region Get
        /// <summary>
        /// get the relevant admin from the list of admins by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>the relevant admin </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Admin GetAdminByUserName(string userName)
        {
            return (from admin in GetAdminsListBL() where admin.UserName == userName select admin).FirstOrDefault();
        }

        /// <summary>
        /// get the admin's list
        /// </summary>
        /// <returns>the admin's list</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Admin> GetAdminsListBL()
        {
            lock (dalObject)
            {
                return from admin in dalObject.GetAdminsList()
                       select new Admin
                       {
                           UserName = admin.UserName,
                           Password = admin.Password
                       };
            }
        }
        #endregion


        #region Add
        /// <summary>
        /// check if the admin is exsist by checking the username add his pasaword
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> true or false dependinfg on the case </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsAdminRegistered(string username, string password)
        {
            DO.Admin admin = dalObject.GetAdminByUserName(username);
            if (admin.UserName == username && admin.Password == password)
            {
                return true;
            }
            return false;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsAdminExsist(string username)
        {
            DO.Admin admin = dalObject.GetAdminByUserName(username);
            if (admin.UserName == username)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// add new admin to the admin's list
        /// </summary>
        /// <param name="admin"></param>
        /// <exception cref="ObjectAlreadyExistException">throw if the admin has alredy exist </exception>
        /// <exception cref="XMLFileLoadCreateException">throw if the XML file Failed to load file </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewAdminBL(Admin admin)
        {
            var exsitUser = (from user in dalObject.GetAdminsList() where user.UserName == admin.UserName select user).FirstOrDefault();
            if (exsitUser.UserName != String.Empty) throw new ObjectAlreadyExistException("Admin");

            DO.Admin newAdmin = new();
            newAdmin.UserName = admin.UserName;
            newAdmin.Password = admin.Password;
            try
            {
                dalObject.AddAdmin(newAdmin);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }
        #endregion


        #region Update
        /// <summary>
        /// update the admin's password
        /// </summary>
        /// <param name="newAdmin"></param>
        /// <exception cref="ObjectNotFoundException">throw if the object is not found</exception>
        public void UpdateAdminPasswordBL(Admin newAdmin)
        {
            DO.Admin admin = new();
            admin.UserName = newAdmin.UserName;
            admin.Password = newAdmin.Password;
            lock (dalObject)
            {
                try
                {
                    dalObject.UpdateAdminPassword(admin);
                }
                catch (DO.ObjectNotFoundException e)
                {
                    throw new ObjectNotFoundException(e.Message);
                }
            }
        }
        #endregion


        #region Delete
        /// <summary>
        /// delete admin frome the admin's list
        /// </summary>
        /// <param name="userName"></param>
        /// <exception cref="ObjectNotFoundException">throw if the object has't been found </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteAdminBL(string userName)
        {
            lock (dalObject)
            {
                try
                {
                    dalObject.DeleteAdmin(userName);
                }
                catch (DO.ObjectNotFoundException e)
                {
                    throw new ObjectNotFoundException(e.Message);
                }
            }
        }
        #endregion
    }
}
