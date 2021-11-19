using PragueParking2.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    class MC : Vehicle
    {
        public MC(string licensePlate, DateTime timeParked) : base(licensePlate, timeParked)
        {
            Size = Config.MCSize;
            type = "MC";
        }
    }
}
