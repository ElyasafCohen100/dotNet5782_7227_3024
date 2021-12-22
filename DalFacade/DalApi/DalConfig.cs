using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalApi
{
    class DalConfig
    {
        internal static string DalName;
        internal static Dictionary<string, string> DalPackages;
        internal static string Class;
        internal static string Namespace;
        static DalConfig()
        {
            XElement dalConfig = XElement.Load(@"dal-config.xml");
            DalName = dalConfig.Element("dal").Value;
            DalPackages = (from pkg in dalConfig.Element("dal-packages").Elements()
                          select pkg).ToDictionary(p => "" + p.Name, p => p.Value);
            Class = dalConfig.Element("dal-packages").Element(DalName).Attribute("class").Value;
            Namespace = dalConfig.Element("dal-packages").Element(DalName).Attribute("namespace").Value;
        }
    }

    [Serializable]
    public class DalConfigException : Exception
    {
        public DalConfigException() { }
        public DalConfigException(string message) : base(message) { }
        public DalConfigException(string message, Exception inner) : base(message, inner) { }
    }
}