using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class BlFactory
    {
        public static BL GetBl()
        {
            return BL.BlObj;
        }
    }
}