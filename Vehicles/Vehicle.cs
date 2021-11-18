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
        /// <param name="space">Which space the vehicle is parked</param>
        /// <param name="vehicle">Which vehicle to print info about</param>
        public void PrintVehicleInfo(int space, Vehicle vehicle)
        {
            TimeSpan duration = CarPark.CalculateDuration(vehicle.TimeParked);
            double price = CarPark.CalculatePrice(vehicle);
            Console.Clear();
            Console.SetCursorPosition(0, Console.WindowHeight / 2 - 8);
            //TODO make a nicer output
            Menu.CenterTxt("Recipt\n");
            Menu.CenterTxt($"You had a {vehicle.GetType().Name} parked at Prague Parking\n");
            Menu.CenterTxt($"With plate number: {vehicle.LicensePlate}\n");
            Menu.CenterTxt($"It was parked: {vehicle.TimeParked:g}\n");
            Menu.CenterTxt($"It has been parked here for: {duration.Days} days {duration.Hours} hours and {duration.Minutes} minutes\n\n");
            Menu.CenterTxt("Cost: ");
            Menu.CenterTxt($"{price} CZK");
            Console.WriteLine();
            Menu.CenterTxt("Press any key to print recipt to customer...");
            Console.CursorVisible = false;
            Console.ReadKey();
            Console.CursorVisible = true;
        }
    }
}