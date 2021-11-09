using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class CustomerInParcel
        {
            public int Id { set; get; }
            public string Name { set; get; }

            public override string ToString()
            {
                return $"Customer in parcel: \n" +
                    $"Id: {Id} \n"+
                    $"Name: {Name} \n";
            }
        }
    }
}
