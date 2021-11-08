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
            double[] tempArray = dalObject.ElectricityUseRequest();
            double droneChargingRate = tempArray[4];
            double[] electricityUse = new double[4];

            for(int i=0;i< tempArray.Length; i++)
            {
                electricityUse[i] = tempArray[i];
            }


            BL bl = new BL();




        }
    }
}

