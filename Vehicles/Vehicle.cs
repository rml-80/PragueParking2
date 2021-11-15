using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            CarPark CP = new CarPark();
            // TODO prehaps have one method in CarPark that gets duration and price?
            TimeSpan duration = CP.CalculateDuration();
            float price = CP.CalculatePrice();

            //TimeSpan duration = CP.ReturnDurationAndPrice(out float price);    //??
            
            Console.WriteLine($"{vehicle.LicensePlate} is parked on space: {space + 1}");
            Console.WriteLine($"It was parked: {vehicle.TimeParked:g}");
            Console.WriteLine($"It has been parked for: {duration}");
            Console.WriteLine($"Price for the parking so far: {price} CZK");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public bool ValidateLicensePlate()  //TODO needs licens plate
        {
            //TODO validate license plate, more than 4 chars and not more than 10 chars
            //TODO dosen't contain special chars
            throw new System.NotImplementedException();
        }

        public string AskForLicensePlate()
        {
            // ask for license plate, no need for validation
            throw new System.NotImplementedException();
        }

        public string NewLicensePlate()
        {
            //TODO ask for license plate and validate? or validate some where else
            //TODO if validateLicensPlate returns false loop
            throw new System.NotImplementedException();
        }
    }
}
