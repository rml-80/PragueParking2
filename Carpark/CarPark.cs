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
        private ConsoleColor EmptyColor = ConsoleColor.Green;
        private ConsoleColor CarColor = ConsoleColor.Yellow;
        private ConsoleColor MCColor = ConsoleColor.DarkRed;
        private ConsoleColor WarningColor = ConsoleColor.Red;
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
                //TODO can this be done in a better way?
                if (parkingSpaces[i].ParkedVehicles == null)
                {
                    Console.Write(string.Format("{0,3} | ", i + 1));
                    Console.ForegroundColor = EmptyColor;
                    Console.Write(string.Format("{0,-24}", "Empty"));
                    Console.ResetColor();
                    space++;
                }
                else if (parkingSpaces[i].ParkedVehicles[0].GetType() == typeof(CAR))
                {
                    Console.Write(string.Format("{0,3} | ", i + 1));
                    Console.ForegroundColor = CarColor;
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
                    Console.ForegroundColor = MCColor;
                    Console.Write(string.Format("{0,-24}", parkingSpaces[i].ParkedVehicles[0].GetType().Name));
                    Console.ResetColor();
                    space++;
                }
            }
        }
        /// <summary>
        /// validate license plate, more than 4 chars and not more than 10 chars and dosen't contain special chars
        /// </summary>
        /// <param name="numberPlate">Which plate to check</param>
        /// <returns>
        /// bool
        /// </returns>
        public bool ValidateLicensePlate(string numberPlate)
        {
            string pattern = "^[A-Z0-9]{4,10}$";
            bool valid = (Regex.IsMatch(numberPlate, pattern)) ? true : false;
            Console.SetCursorPosition(0, 28);
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
                    Menu.ClearRow(28);
                    Menu.ClearRow(29);
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
                                    ParkVehicle(car, null);
                                    break;
                                case 2:
                                    MC mc = new MC(numberPlate, DateTime.Now);
                                    loop = false;
                                    ParkVehicle(mc, null);
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
        /// <summary>
        /// Finds the first parking space with enough available space for vehicle
        /// </summary>
        /// <param name="size"></param>
        /// <returns>
        /// int spacenumber
        /// </returns>
        //TODO return SpaceNumber instead?
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
        /// <summary>
        /// Search for a specific vehicle
        /// </summary>
        /// <param name="space"> 
        /// returns space index
        /// </param>
        /// <returns>
        /// The vehicle that was saearch for
        /// </returns>
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
                Console.ForegroundColor = WarningColor;
                Console.WriteLine("No vehicle parked here with that license plate!");
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
            Menu.ClearRow(26);
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
                            Console.ForegroundColor = WarningColor;     //TODO store somewhere else?
                            Console.WriteLine("Space occupied.");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(0, 28);
                        Console.ForegroundColor = WarningColor;     //TODO store somewhere else?
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
        /// Outputs ticket to console
        /// </summary>
        /// <param name="vehicle">Which vehicle</param>
        /// <param name="spaceNumber">Which space number</param>
        public void PrintTicket(Vehicle vehicle, int spaceNumber)   //TODO in parameter should be spaceNumber
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
                space.ParkedVehicles = null;
            }
            FC.WriteSavedParkingSpaces();
        }
    }
}
