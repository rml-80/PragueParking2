using PragueParking2.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PragueParking2
{
    public class Vehicle
    {
        public string LicensePlate { get; set; }
        public int Size { get; set; }
        public DateTime TimeParked { get; set; }
        public string type { get; set; }
        public Vehicle(string licensePlate, DateTime timeParked)
        {
            LicensePlate = licensePlate;
            TimeParked = timeParked;
        }
        /// <summary>
        /// Outputs information about vehicle, also used for recipes at the moment
        /// </summary>
        /// <param name="spaceNumber">Which space the vehicle is parked</param>
        /// <param name="vehicle">Which vehicle to print info about</param>
        public void PrintVehicleInfo(int spaceNumber, Vehicle vehicle)
        {
            TimeSpan duration = CarPark.CalculateDuration(vehicle.TimeParked);
            double price = CarPark.CalculatePrice(vehicle);
            Menu.ClearRow(Console.WindowHeight - 4);
            Console.Write($"Vehicle is parked on space: {spaceNumber}. It was parked: {vehicle.TimeParked:g} Cost so far: {price} CZK");
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            //TODO print search to menu row
        }
    }
}