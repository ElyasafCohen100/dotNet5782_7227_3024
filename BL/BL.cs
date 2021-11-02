using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    partial class BL : IBL
    {
        public BL()
        {
         IDAL.IDal dalObject = new DalObject.DalObject();
        }
    }
}

