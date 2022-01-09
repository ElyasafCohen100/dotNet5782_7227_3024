﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public partial class BL
    {
        public Admin FindAdminByUserName(string userName)
        {
            return (from admin in GetAdminsListBL() where admin.UserName == userName select admin).FirstOrDefault();
        }
        public void AddNewAdminBL(Admin admin)
        {
            var exsitUser = (from user in dalObject.GetAdminsList() where user.UserName == admin.UserName select user).FirstOrDefault();
            if (exsitUser.UserName != String.Empty) throw new ObjectAlreadyExistException("Admin");

            DO.Admin newAdmin = new();
            newAdmin.UserName = admin.UserName;
            newAdmin.Password = admin.Password;
            try
            {
                dalObject.SetAdmin(newAdmin);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }
        public void DeleteAdminBL(string userName)
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
        public void UpdateAdminPasswordBL(Admin newAdmin)
        {
            DO.Admin admin = new();
            admin.UserName = newAdmin.UserName;
            admin.Password = newAdmin.Password;
            try
            {
                dalObject.UpdateAdminPassword(admin);
            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }
        }
        public IEnumerable<Admin> GetAdminsListBL()
        {
            return from admin in dalObject.GetAdminsList()
                   select new Admin
                   {
                       UserName = admin.UserName,
                       Password = admin.Password
                   };
        }

        public bool IsAdminRegistered(string username, string password)
        {
            DO.Admin admin = dalObject.FindAdminByUserName(username);
            if (admin.UserName == username && admin.Password == password)
                return true;
            return false;
        }
        public bool IsAdminRegistered(string username)
        {
            DO.Admin admin = dalObject.FindAdminByUserName(username);
            if (admin.UserName == username)
                return true;
            return false;
        }


    }
}
