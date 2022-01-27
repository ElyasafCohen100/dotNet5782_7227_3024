using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public static class BlFactory
    {
        public static BL.BL GetBl()
        {
            return BL.BL.BlObj;
        }
    }
}
