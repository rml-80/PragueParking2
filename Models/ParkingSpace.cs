using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    public class ParkingSpace
    {
        public int SpaceNumber { get; }
        public int Size { get; } 
        public int AvailableSpace { get; set; }
        public List<Vehicle> ParkedVehicles { get; set; }

        public ParkingSpace(int spaceNumber, int size)
        {
            this.SpaceNumber = spaceNumber;
            this.Size = size;
            this.AvailableSpace = size;
        }
    }
}
