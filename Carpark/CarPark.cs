using PragueParking2.Files;
using PragueParking2.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PragueParking2
{
    class CarPark
    {
        static FileContext FC = new();
        public int Size { get; set; } = Config.NumberOfSpaces;
        public static List<ParkingSpace> parkingSpaces = new();
        private ConsoleColor EmptyColor = ConsoleColor.Green;
        private ConsoleColor CarColor = ConsoleColor.Yellow;
        private ConsoleColor MCColor = ConsoleColor.DarkRed;
        private ConsoleColor WarningColor = ConsoleColor.Red;
        public CarPark()
        {
            if (!parkingSpaces.Count.Equals(Size))
            {
                int cols = 4;
                double rows = (double)Size / cols;
                for (int i = 0; i < Math.Ceiling(rows); i++)
                {
                    double k = i + 1;
                    for (int j = 0; j < cols; j++, k += rows)
                    {
                        parkingSpaces.Add(new ParkingSpace((int)k, Config.SpaceSize));
                    }
                }
            }
        }
        /// <summary>
        /// Outputs parking lot to console
        /// </summary>
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
                    Console.Write(string.Format("{0,3} | ", parkingSpaces[i].SpaceNumber));
                    Console.ForegroundColor = EmptyColor;
                    Console.Write(string.Format("{0,-24}", "Empty"));
                    Console.ResetColor();
                    space++;
                }
                else if (parkingSpaces[i].ParkedVehicles[0].GetType() == typeof(CAR))
                {
                    Console.Write(string.Format("{0,3} | ", parkingSpaces[i].SpaceNumber));
                    Console.ForegroundColor = CarColor;
                    Console.Write(string.Format("{0,-24}", parkingSpaces[i].ParkedVehicles[0].GetType().Name));
                    Console.ResetColor();
                    space++;
                }
                else if (parkingSpaces[i].ParkedVehicles.Count == 2)
                {
                    Console.Write(string.Format("{0,3} | ", parkingSpaces[i].SpaceNumber));
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(string.Format("{0,-3} {1,-20}", parkingSpaces[i].ParkedVehicles[0].GetType().Name, parkingSpaces[i].ParkedVehicles[0].GetType().Name));
                    Console.ResetColor();
                    space++;
                }
                else
                {
                    Console.Write(string.Format("{0,3} | ", parkingSpaces[i].SpaceNumber));
                    Console.ForegroundColor = MCColor;
                    Console.Write(string.Format("{0,-24}", parkingSpaces[i].ParkedVehicles[0].GetType().Name));
                    Console.ResetColor();
                    space++;
                }
            }
        }
        public bool ValidateLicensePlate(string numberPlate)
        {
            string pattern = "^[A-Z0-9]{4,10}$";
            bool valid = (Regex.IsMatch(numberPlate, pattern)) ? true : false;
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.ForegroundColor = WarningColor;
            Console.WriteLine((valid == false) ? "Invalid number plate..." : ""); ;
            Console.ResetColor();
            return valid;
        }
        /// <summary>
        /// Creates new vehicle
        /// </summary>
        /// <returns>
        /// A new vehicle
        /// </returns>
        public bool CreateNewVehicle()
        {
            bool loop = true;
            Menu.SubMenu();
            while (loop)
            {
                Menu.ClearChoice();
                bool inputOk = int.TryParse(Console.ReadLine(), out int choice);
                if (inputOk)
                {
                    Menu.ClearRow(Console.WindowHeight - 2);
                    Menu.ClearRow(Console.WindowHeight - 1);
                    bool validNumberPlate;
                    do
                    {
                        string numberPlate = EnterNumberPlate();
                        validNumberPlate = ValidateLicensePlate(numberPlate);
                        if (validNumberPlate)
                        {
                            switch (choice)
                            {
                                case 1:
                                    CAR car = new CAR(numberPlate, DateTime.Now);
                                    loop = false;
                                    ParkingVehicle(car, null);
                                    break;
                                case 2:
                                    MC mc = new MC(numberPlate, DateTime.Now);
                                    loop = false;
                                    ParkingVehicle(mc, null);
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
                    Console.ForegroundColor = WarningColor;
                    Console.Write("Invalid input detected. Please try again.");
                    Console.ResetColor();
                }
            }
            return true;
        }
        public int FindSpace(int size)
        {
            int emptyspace = 0;
            for (int spaceNumber = 1; spaceNumber <= parkingSpaces.Count; spaceNumber++)
            {
                int spaceIndex = GetIndex(spaceNumber);
                if (parkingSpaces[spaceIndex].AvailableSpace >= size)
                {
                    if (parkingSpaces[spaceIndex].AvailableSpace == size) return spaceIndex;
                    if (emptyspace == 0) emptyspace = spaceIndex;
                }
            }
            return emptyspace;
        }
        /// <summary>
        /// Search for a specific vehicle
        /// </summary>
        /// <param name="spaceNumber"> 
        /// returns space index
        /// </param>
        /// <returns>
        /// The vehicle that was saearch for
        /// </returns>
        public Vehicle SearchVehicle(out int? spaceNumber)
        {
            Menu.ClearRow(28);
            spaceNumber = null;
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
                                spaceNumber = parkingSpaces[i].SpaceNumber;
                                vehicle = parkingSpaces[i].ParkedVehicles[j];
                                Menu.ClearRow(28);
                                return vehicle;
                            }
                        }
                    }
                }
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.ForegroundColor = WarningColor;
                Console.Write("No vehicle parked here with that license plate!");
                Console.ResetColor();
            }
            return null;
        }
        /// <summary>
        /// Asks for License plate
        /// </summary>
        /// <returns>
        /// string for numberplate
        /// </returns>
        private static string EnterNumberPlate()
        {
            Menu.ClearRow(Console.WindowHeight - 4);
            Console.Write("Enter license plate: ");
            string licensePlate = Console.ReadLine().ToUpper();
            return licensePlate;
        }
        /// <summary>
        /// Parkeds vehicle on space
        /// </summary>
        /// <param name="vehicle">Needs a vehicle to park</param>
        /// <param name="space">If provided parks on the space, 
        /// if not provided it uses FindSpace method to find first best space </param>
        /// <returns>
        /// bool 
        /// </returns>
        [Obsolete]
        public bool ParkVehicle(Vehicle vehicle, int? space)
        {
            bool moved = (space == null) ? false : true;

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
            int idx = parkingSpaces.FindIndex(x => x.SpaceNumber == space);
            if (!moved)
            {
                PrintTicket(vehicle, (int)space);
            }
            return true;
        }
        public bool ParkingVehicle(Vehicle vehicle, int? spaceNumber)
        {
            bool moved = (spaceNumber == null) ? false : true;
            int? idx;
            if (spaceNumber == null)
            {
                //Get the index of next empty space
                idx = FindSpace(vehicle.Size);
            }
            else
            {
                //Get the index of spacenumber x
                idx = GetIndex((int)spaceNumber);
            }
            if (idx != -1)
            {
                if (parkingSpaces[(int)idx].ParkedVehicles == null)
                {
                    parkingSpaces[(int)idx].ParkedVehicles = new List<Vehicle>();
                }
                parkingSpaces[(int)idx].ParkedVehicles.Add(vehicle);
                parkingSpaces[(int)idx].AvailableSpace -= vehicle.Size;
                FC.WriteSavedParkingSpaces();
                if (!moved)
                {
                    PrintTicket(vehicle, parkingSpaces[(int)idx].SpaceNumber);
                }
                return true;
            }
            else
            {
                WrongMsg();
                return false;
            }
        }
        /// <summary>
        /// Removes specified vehicle from space
        /// </summary>
        /// <param name="vehicle">Which vehicle to remove</param>
        /// <param name="space">From which space</param>
        /// <param name="moved">If true don't print ticket and recipe</param>
        /// <returns>
        /// bool
        /// </returns>
        public bool RetriveVehicle(Vehicle vehicle, int space, bool moved = false)
        {
            int idx = GetIndex(space);
            if (!moved)
            {
                PrintRecipe(vehicle);
            }
            parkingSpaces[idx].ParkedVehicles.Remove(vehicle);
            parkingSpaces[idx].AvailableSpace += vehicle.Size;
            if (parkingSpaces[idx].ParkedVehicles.Count == 0)
            {
                parkingSpaces[idx].ParkedVehicles = null;
            }
            FC.WriteSavedParkingSpaces();
            return true;
        }
        /// <summary>
        /// Parks and removes vehicle
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
        public bool MovingVehicle()
        {
            Vehicle vehicle = SearchVehicle(out int? space);
            if (vehicle != null)
            {
                while (true)
                {
                    Console.SetCursorPosition(35, Console.WindowHeight - 4);
                    Console.Write("                          ");
                    Console.SetCursorPosition(35, Console.WindowHeight - 4);

                    Console.Write("Move to: ");
                    bool inputOK = int.TryParse(Console.ReadLine(), out int newSpace);
                    int idx = GetIndex(newSpace);
                    if (inputOK && idx < Size)
                    {
                        if (parkingSpaces[idx].AvailableSpace >= vehicle.Size)
                        {
                            ParkingVehicle(vehicle, newSpace);
                            RetriveVehicle(vehicle, (int)space, true);
                            return true;
                        }
                        else
                        {
                            Console.SetCursorPosition(0, Console.WindowHeight - 2);
                            Console.ForegroundColor = WarningColor;
                            Console.WriteLine("Space occupied.");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.WindowHeight - 2);
                        Console.ForegroundColor = WarningColor;
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
        }
        /// <summary>
        /// Get index for a space number
        /// </summary>
        /// <param name="Space">Which space</param>
        /// <returns>int index for space number</returns>
        private static int GetIndex(int Space)
        {
            return parkingSpaces.FindIndex(s => s.SpaceNumber == Space);
        }
        /// <summary>
        /// Outputs ticket to console
        /// </summary>
        /// <param name="vehicle">Which vehicle</param>
        /// <param name="spaceNumber">Which space number</param>
        public void PrintTicket(Vehicle vehicle, int spaceNumber)
        {
            Console.Clear();
            Console.WriteLine($"Park vehical on space: {spaceNumber}");
            Console.SetCursorPosition(0, Console.WindowHeight / 2 - 5);
            Menu.CenterTxt("Ticket");
            Menu.CenterTxt($"You have parked a {vehicle.GetType().Name} at Prague Parking\n");
            Menu.CenterTxt($"Number plate: {vehicle.LicensePlate}\n");
            Menu.CenterTxt($"It's parked on space number: {spaceNumber}");
            Menu.CenterTxt($"Time parked: {vehicle.TimeParked:g}"); // using :g to not display seconds
            Console.WriteLine();
            Menu.CenterTxt("Press any key to print ticket to customer.");
            Console.CursorVisible = false;
            Console.ReadKey();
            Console.CursorVisible = true;
        }
        /// <summary>
        /// Print recipe to console
        /// </summary>
        /// <param name="vehicle">Which vehicle</param>
        public void PrintRecipe(Vehicle vehicle)
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
        /// <summary>
        /// Calculates the duration of the parking
        /// </summary>
        /// <param name="dt">The time the vehicle was parked</param>
        /// <returns>
        /// TimeSpan
        /// </returns>
        public static TimeSpan CalculateDuration(DateTime dt)
        {
            return DateTime.Now - dt;
        }
        /// <summary>
        /// Calculates price
        /// </summary>
        /// <param name="vehicle">For getting the right price</param>
        /// <returns>
        /// double
        /// </returns>
        public static double CalculatePrice(Vehicle vehicle)
        {
            TimeSpan duration = CalculateDuration(vehicle.TimeParked);
            TimeSpan time = duration.Subtract(TimeSpan.FromMinutes(10));
            double price = 0;
            double cost = 0;
            if (time > new TimeSpan(0))
            {
                FileContext FC = new();
                price = FC.GetPrice(vehicle.GetType().Name);
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
        /// <summary>
        /// Outputs msg to console
        /// </summary>
        void WrongMsg()
        {
            Console.WriteLine("Something went wrong...");
        }
        /// <summary>
        /// Removes all vehicles from the car park
        /// </summary>
        public void RemoveAllVehicles()
        {
            foreach (var space in parkingSpaces)
            {
                space.AvailableSpace = Config.SpaceSize;
                space.ParkedVehicles = null;
            }
            FC.WriteSavedParkingSpaces();
        }
    }
}