using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    class MC : Vehicle
    {
        public MC(string licensePlate) : base(licensePlate)
        {
            Size = (int)VehicleSize.MC;
            type = "MC";
        }
    }
}
