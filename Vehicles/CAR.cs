using PragueParking2.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    class CAR : Vehicle
    {
        public CAR(string licensePlate) : base(licensePlate)
        {
            Size = Config.CarSize;
            type = "CAR";
        }

        public CAR(Vehicle vehicle) : base(vehicle)
        {
            Size = Config.CarSize;
            type = "CAR";
        }
    }
}
