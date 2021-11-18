using PragueParking2.Files;
using PragueParking2.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PragueParking2
{
    class CarPark
    {
        static FileContext FC = new();
        public int Size { get; set; } = Config.NumberOfSpaces;
        public static List<ParkingSpace> parkingSpaces = new();
        public CarPark()
        {
            if (!parkingSpaces.Count.Equals(Size))
            {
                for (int i = 0; i < Size; i++)
                {
                    parkingSpaces.Add(new ParkingSpace(i + 1, Config.SpaceSize));
                }
            }
        }
        public void PrintCarPark()
        {
            int columns = 5;
            int space = 1;
            for (int i = 0; i < parkingSpaces.Count; i++)
            {
                if (space <= columns && space % columns == 0)
                {
                    Console.WriteLine();
                    space = 1;
                }
                if (parkingSpaces[i].ParkedVehicles == null)
                {
                    Console.Write(string.Format("{0,3} | ", i + 1));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(string.Format("{0,-24}", "Empty"));
                    Console.ResetColor();
                    space++;
                }
                else if (parkingSpaces[i].ParkedVehicles[0].GetType() == typeof(CAR))
                {
                    Console.Write(string.Format("{0,3} | ", i + 1));
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(string.Format("{0,-24}", parkingSpaces[i].ParkedVehicles[0].LicensePlate));
                    Console.ResetColor();
                    space++;
                }
                else if (parkingSpaces[i].ParkedVehicles.Count == 2)
                {
                    Console.Write(string.Format("{0,3} | ", i + 1));
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(string.Format("{0,-3} {1,-20}", parkingSpaces[i].ParkedVehicles[0].GetType().Name, parkingSpaces[i].ParkedVehicles[0].GetType().Name));
                    Console.ResetColor();
                    space++;
                }
                else
                {
                    Console.Write(string.Format("{0,3} | ", i + 1));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(string.Format("{0,-24}", parkingSpaces[i].ParkedVehicles[0].GetType().Name));
                    Console.ResetColor();
                    space++;
                }
            }
        }
        public Vehicle CreateNewVehicle()
        {
            bool loop = true;
            Vehicle vehicle = new();
            Menu.SubMenu();
            while (loop)
            {
                Menu.ClearChoice();
                bool inputOk = int.TryParse(Console.ReadLine(), out int choice);
                if (inputOk)
                {
                    Menu.ClearRow(28);
                    Menu.ClearRow(29);
                    bool validNumberPlate;
                    do
                    {
                        string numberPlate = EnterNumberPlate();
                        validNumberPlate = vehicle.ValidateLicensePlate(numberPlate);
                        if (validNumberPlate)
                        {
                            switch (choice)
                            {
                                case 1:
                                    vehicle = new CAR(numberPlate);
                                    loop = false;
                                    break;
                                case 2:
                                    vehicle = new MC(numberPlate);
                                    loop = false;
                                    break;
                                default:
                                    Console.WriteLine("Not a valid choice.");
                                    break;
                            }
                        }
                    } while (!validNumberPlate);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid input detected. Please try again.");
                    Console.ResetColor();
                }
            }
            ParkVehicle(vehicle, null);
            return vehicle;
        }
        public int FindSpace(int size)      //TODO check if mc stands alone
        {
            int? space = null;
            for (int i = 0; i < parkingSpaces.Count; i++)
            {
                if (parkingSpaces[i].AvailableSpace >= size)
                {
                    return (int)(space = i);
                }
            }
            return (int)space;
        }
        public Vehicle SearchVehicle(out int? space)
        {
            Menu.ClearRow(28);
            space = null;
            Vehicle vehicle = null;
            while (vehicle == null)
            {
                string licensePlate = EnterNumberPlate();
                for (int i = 0; i < parkingSpaces.Count; i++)
                {
                    if (parkingSpaces[i].ParkedVehicles != null)
                    {
                        for (int j = 0; j < parkingSpaces[i].ParkedVehicles.Count; j++)
                        {
                            if (parkingSpaces[i].ParkedVehicles[j].LicensePlate == licensePlate)
                            {
                                space = i;
                                vehicle = parkingSpaces[i].ParkedVehicles[j];
                                Menu.ClearRow(28);
                                return vehicle;
                            }
                        }
                    }
                }
                Console.SetCursorPosition(0, 28);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No vehicle parked here with that license plate!");
                Console.ResetColor();
            }
            return null;
        }
        private static string EnterNumberPlate()
        {

            Menu.ClearRow(26);
            Console.Write("Enter license plate: ");
            string licensePlate = Console.ReadLine().ToUpper();
            return licensePlate;
        }
        public bool ParkVehicle(Vehicle vehicle, int? space)
        {
            bool moved = (space == null) ? false : true;
            if (vehicle != null)
            {
                if (space == null)
                {
                    space = FindSpace(vehicle.Size);
                }
                if (parkingSpaces[(int)space].ParkedVehicles == null)
                {
                    parkingSpaces[(int)space].ParkedVehicles = new List<Vehicle>();
                }
                parkingSpaces[(int)space].ParkedVehicles.Add(vehicle);
                parkingSpaces[(int)space].AvailableSpace -= vehicle.Size;
                FC.WriteSavedParkingSpaces();

                if (!moved)
                {
                    PrintTicket(vehicle, (int)space);
                }
                return true;
            }
            else
            {
                WrongMsg();
                return false;
            }
        }
        public bool RetriveVehicle(Vehicle vehicle, int space, bool moved = false)
        {
            if (!moved)
            {
                vehicle.PrintVehicleInfo((int)space, vehicle);
            }
            parkingSpaces[(int)space].ParkedVehicles.Remove(vehicle);
            parkingSpaces[(int)space].AvailableSpace += vehicle.Size;
            if (parkingSpaces[(int)space].ParkedVehicles.Count == 0)
            {
                parkingSpaces[(int)space].ParkedVehicles = null;
            }
            FC.WriteSavedParkingSpaces();
            return true;
        }
        public bool MovingVehicle()
        {
            Vehicle vehicle = SearchVehicle(out int? space);
            if (vehicle != null)
            {
                while (true)
                {
                    Console.SetCursorPosition(35, 26);
                    Console.Write("                          ");
                    Console.SetCursorPosition(35, 26);

                    Console.Write("Move to: ");
                    bool inputOK = int.TryParse(Console.ReadLine(), out int newSpace);
                    if (inputOK && newSpace < Size)
                    {
                        if (parkingSpaces[newSpace].AvailableSpace >= vehicle.Size)
                        {
                            ParkVehicle(vehicle, newSpace);
                            RetriveVehicle(vehicle, (int)space, true);
                            return true;
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 28);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Space occupied.");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(0, 28);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input type was wrong.");
                        Console.ResetColor();

                    }
                }
            }
            else
            {
                WrongMsg();
                return false;
            }
            return true;
        }
        public void PrintTicket(Vehicle vehicle, int spaceNumber)
        {
            Console.Clear();
            Console.WriteLine($"Park vehical on space: {spaceNumber + 1}");
            Console.SetCursorPosition(0, Console.WindowHeight / 2 - 5);
            Menu.CenterTxt("Ticket");
            Menu.CenterTxt($"You have parked a {vehicle.GetType().Name} at Prague Parking\n");
            Menu.CenterTxt($"Number plate: {vehicle.LicensePlate}\n");
            Menu.CenterTxt($"It's parked on space number: {spaceNumber + 1}");
            Menu.CenterTxt($"Time parked: {vehicle.TimeParked:g}"); // using :g to not display seconds
            Console.WriteLine();
            Menu.CenterTxt("Press any key to print ticket to customer.");
            Console.CursorVisible = false;
            Console.ReadKey();
            Console.CursorVisible = true;
        }
        public static TimeSpan CalculateDuration(DateTime dt)
        {
            return DateTime.Now - dt;
        }
        public static double CalculatePrice(Vehicle vehicle)
        {
            TimeSpan duration = CalculateDuration(vehicle.TimeParked);
            TimeSpan time = duration.Subtract(TimeSpan.FromMinutes(10));
            double price = 0;
            double cost = 0;
            if (time > new TimeSpan(0))
            {
                FileContext FC = new();
                price = FC.GetPrice(vehicle);
                if (time.Minutes >= 0)
                {
                    time = time.Add(TimeSpan.FromHours(1));
                }
                if (time.Days >= 1)
                {
                    cost = (time.Days * 24) * price;

                }
                cost += time.Hours * price;
            }
            return cost;
        }
        public static TimeSpan ReturnDurationAndPrice(Vehicle vehicle, out double price)
        {
            TimeSpan duration = CalculateDuration(vehicle.TimeParked);
            price = CalculatePrice(vehicle);
            return duration;
        }
        void WrongMsg()
        {
            Console.WriteLine("Something went wrong...");
        }
        public void RemoveAllVehicles()
        {
            foreach (var space in parkingSpaces)
            {
                space.ParkedVehicles = null;
            }
            FC.WriteSavedParkingSpaces();
        }
    }
}
