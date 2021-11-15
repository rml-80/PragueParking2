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
            Size = (int)VehicleSize.CAR;
            type = "CAR";
        }

        public CAR(Vehicle vehicle) : base(vehicle)
        {
            Size = (int)VehicleSize.CAR;
            type = "CAR";
        }
    }
}
