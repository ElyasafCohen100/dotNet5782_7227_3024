using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DO;
using System.Threading.Tasks;

namespace Dal
{
    partial class DalXml : DalApi.IDal
    {
        public void SetAdmin(Admin admin)
        {
            try
            {
                List<Admin> adminList = XMLTools.LoadListFromXMLSerializer<Admin>(dalAdminPath);

                adminList.Add(admin);
                XMLTools.SaveListToXMLSerializer(adminList, dalAdminPath);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }
        public void DeleteAdmin(string userName)
        {
            List<Admin> adminList = XMLTools.LoadListFromXMLSerializer<Admin>(dalAdminPath);

            int index = adminList.FindIndex(x => x.UserName == userName);
            if (index == -1) throw new ObjectNotFoundException("Admin");

            Admin admin = adminList[index];
            adminList.Remove(admin);
            XMLTools.SaveListToXMLSerializer(adminList, dalAdminPath);
        }
        public IEnumerable<Admin> GetAdminsList()
        {
            List<Admin> adminList = XMLTools.LoadListFromXMLSerializer<Admin>(dalAdminPath);
            return from admin in adminList select admin;
        }
        public void UpdateAdminPassword(Admin newAdmin)
        {
            List<Admin> adminList = XMLTools.LoadListFromXMLSerializer<Admin>(dalAdminPath);

            int index = adminList.FindIndex(x => x.UserName == newAdmin.UserName);
            if (index == -1) throw new ObjectNotFoundException("Admin");
            Admin admin = adminList[index];
            XMLTools.SaveListToXMLSerializer(adminList, dalAdminPath);
        }
        public Admin FindAdminByUserName(string userName)
        {
            return (from admin in GetAdminsList() where admin.UserName == userName select admin).FirstOrDefault();
        }
    }
}
