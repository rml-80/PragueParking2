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
        public Vehicle(string licensePlate)
        {
            LicensePlate = licensePlate;
            TimeParked = DateTime.Now;
        }
        public Vehicle(Vehicle vehicle)
        {
            LicensePlate = vehicle.LicensePlate;
            TimeParked = vehicle.TimeParked;
        }
        public Vehicle()
        { }
        public void PrintVehicleInfo(int space, Vehicle vehicle)
        {
            TimeSpan duration = CarPark.ReturnDurationAndPrice(vehicle, out double price);
            Console.Clear();
            Console.SetCursorPosition(0, Console.WindowHeight / 2 - 8);
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
        //validate license plate, more than 4 chars and not more than 10 chars and dosen't contain special chars
        public bool ValidateLicensePlate(string numberPlate)
        {
            string pattern = "^[A-Z0-9]{4,10}$";
            bool valid = (Regex.IsMatch(numberPlate, pattern)) ? true : false;
            Console.SetCursorPosition(0, 28);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine((valid == false) ? "Invalid number plate..." : ""); ;
            Console.ResetColor();
            return valid;
        }
    }
}