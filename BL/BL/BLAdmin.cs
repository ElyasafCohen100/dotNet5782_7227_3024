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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Admin GetAdminByUserName(string userName)
        {
            return (from admin in GetAdminsListBL() where admin.UserName == userName select admin).FirstOrDefault();
        }


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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsAdminRegistered(string username, string password)
        {
            lock (dalObject)
            {
                DO.Admin admin = dalObject.GetAdminByUserName(username);
                if (admin.UserName == username && admin.Password == password)
                    return true;
                return false;
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsAdminRegistered(string username)
        {
            lock (dalObject)
            {
                DO.Admin admin = dalObject.GetAdminByUserName(username);
                if (admin.UserName == username)
                    return true;
                return false;
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewAdminBL(Admin admin)
        {
            lock (dalObject)
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
        }
        #endregion


        #region Update
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
