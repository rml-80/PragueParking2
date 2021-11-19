using PragueParking2.Files;
using System;

namespace PragueParking2.Menus
{
    class Menu
    {
        /// <summary>
        /// Main menu for the application
        /// Returns here until application is closed
        public void MainMenu(FileContext FC)
        {
            CarPark CP = new();
            FC.ReadSavedParkingSpaces();
            bool loop = true;
            do
            {
                Console.Clear();
                CP.PrintCarPark();
                Console.WriteLine();
                PrintMenu();

                Console.Write("Choice: ");
                bool inputOK = int.TryParse(Console.ReadLine(), out int choice);
                if (inputOK)
                {
                    switch (choice)
                    {
                        case 0:
                            loop = false;
                            Console.Clear();
                            break;
                        case 1:
                            ClearRow(Console.WindowHeight - 4);
                            CP.CreateNewVehicle();
                            break;
                        case 2:
                            CP.RetriveVehicle(CP.SearchVehicle(out int? space), (int)space);
                            break;
                        case 3:
                            CP.MovingVehicle();
                            break;
                        case 4:
                            Vehicle vehicle = CP.SearchVehicle(out space);
                            vehicle.PrintVehicleInfo((int)space, vehicle);
                            break;
                        case 5:
                            ShowPrices(FC);
                            break;
                        case 9:
                            OptionsMenu(CP, FC);
                            break;
                        default:
                            break;
                    }
                }
            } while (loop);
        }
        private void PrintMenu()
        {
            Console.SetCursorPosition(0,Console.WindowHeight - 5);
            PrintLineForMenu();
            Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4,-15}{5, -15}{6, 29}", "1. Park", "2. Retrive", "3. Move", "4. Search", "5. Pricelist", "9. Options", "0. Close application");
            PrintLineForMenu();
        }
        //TODO SetCursorPositon Window Height - 4
        /// <summary>
        /// Output the prices to console
        /// </summary>
        /// <param name="FC"></param>
        private void ShowPrices(FileContext FC)
        {
            Console.Clear();
            Console.WriteLine($"Price list:");
            Console.WriteLine("***********");
            foreach (var vehicleType in Config.VehicleTypes)
            {
                double price = FC.GetPrice(vehicleType);
                Console.WriteLine($"{vehicleType}:\t {price} CZK for each hour started. ");
            }
            Console.WriteLine();
            Console.WriteLine("First 10 minutes are free.");
            Console.WriteLine();
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }
        /// <summary>
        /// Outputs option menu to console
        /// </summary>
        /// <param name="CP">instance of CarPark for access to methods</param>
        /// <param name="FC">instance of FileContext for access to methods</param>
        private void OptionsMenu(CarPark CP, FileContext FC)
        {
            ClearRow(Console.WindowHeight - 4);
            Console.Write($"1. Change prices\t2. Remove all vehicles");
            ClearChoice();
            bool inputOK = int.TryParse(Console.ReadLine(), out int choice);
            if (inputOK)
            {
                switch (choice)
                {
                    case 1:
                        FC.OpenPriceFile();
                        break;
                    case 2:
                        CP.RemoveAllVehicles();
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Outputs submenu (Car or bike) to console
        /// </summary>
        public static void SubMenu()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 4);

            for (int i = 0; i < Config.VehicleTypes.Length; i++)
            {
                Console.Write($"{i + 1}. {Config.VehicleTypes[i]}\t");
            }
            ClearChoice();
        }
        /// <summary>
        /// Clears choice input
        /// </summary>
        public static void ClearChoice()
        {
            Console.SetCursorPosition(8, Console.WindowHeight - 2);
            Console.Write("       ");
            Console.SetCursorPosition(8, Console.WindowHeight - 2);
        }
        /// <summary>
        /// Prints a line across the window
        /// </summary>
        private void PrintLineForMenu()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("─");
            }
        }
        /// <summary>
        /// Centers text on the console
        /// </summary>
        /// <param name="txt"></param>
        public static void CenterTxt(string txt)
        {
            int screenWidth = Console.WindowWidth;
            int txtWidth = txt.Length;
            int spaces = (screenWidth / 2) + (txtWidth / 2);
            Console.WriteLine(txt.PadLeft(spaces));
        }
        /// <summary>
        /// Clears a row in console
        /// </summary>
        /// <param name="row"></param>
        public static void ClearRow(int row)
        {
            Console.SetCursorPosition(0, row);
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, row);
        }
    }
}