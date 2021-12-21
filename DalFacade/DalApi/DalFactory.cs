using System;
using System.Reflection;

namespace DalApi
{
    public class DalFactory
    {
        public static IDal GetDal()
        {
            string dalType = DalConfig.DalName;
            string dalPkj = DalConfig.DalPackages[dalType];
            string dalClass = DalConfig.Class;
            string dalNamespace = DalConfig.Namespace;
            if (dalPkj == null) throw new DalConfigExeption($"Package {dalType} is not found in packes list in dal-config.xml");

            try { Assembly.Load(dalPkj); }
            catch (Exception) { throw new DalConfigExeption($"Failed to load the dal-config.xml file"); }

            Type type = Type.GetType($"{dalNamespace}.{dalClass},{dalClass}");
            if (type == null) throw new DalConfigExeption($"Class {dalPkj} was not found in the {dalPkj}.dll");

            IDal dal = (IDal)type.GetProperty("DalObj",
                BindingFlags.Public | BindingFlags.Static).GetValue(null);
            if (dal == null) throw new DalConfigExeption($"Class {dalPkj} is not a singelton or wrong prperty for dalObj");

            return dal;
        }
    }
}
