using PragueParking2.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2.Menus
{
    class Menu
    {
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
                PrintLineForMenu();
                Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4,-15}{5, -15}{6, 29}", "1. Park", "2. Retrive", "3. Move", "4. Search", "5. Pricelist", "9. Options", "0. Close application");
                PrintLineForMenu();

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
                            ClearRow(26);
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
                            OptionsMenu(CP);
                            break;
                        default:
                            break;
                    }
                }
            } while (loop);
        }
        //TODO WIP
        private void ShowPrices(FileContext FC)
        {
            foreach (var vehicleType in Config.VehicleTypes)
            {
                FC.GetPrice(vehicleType);
            }
        }

        private void OptionsMenu(CarPark CP)
        {
            ClearRow(26);
            Console.Write($"1. Reload pricefile\t2. Remove all vehicles");
            ClearChoice();
            bool inputOK = int.TryParse(Console.ReadLine(), out int choice);
            if (inputOK)
            {
                switch (choice)
                {
                    case 1:
                        FileContext FC = new();
                        break;
                    case 2:
                        CP.RemoveAllVehicles();
                        break;
                    default:
                        break;
                }
            }
        }

        public static void SubMenu()
        {
            for (int i = 0; i < Config.VehicleTypes.Length; i++)
            {
                Console.Write($"{i + 1}. {Config.VehicleTypes[i]}\t");
            }
            ClearChoice();
        }

        public static void ClearChoice()
        {
            Console.SetCursorPosition(8, 28);
            Console.Write("       ");
            Console.SetCursorPosition(8, 28);
        }

        private void PrintLineForMenu()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("─");
            }
        }
        public static void CenterTxt(string txt)
        {
            int screenWidth = Console.WindowWidth;
            int txtWidth = txt.Length;
            int spaces = (screenWidth / 2) + (txtWidth / 2);
            Console.WriteLine(txt.PadLeft(spaces));
        }
        //public static void ClearMenu()
        //{
        //    Console.SetCursorPosition(0, 26);
        //    for (int i = 0; i < Console.WindowWidth; i++)
        //    {
        //        Console.Write(" ");
        //    }
        //    Console.SetCursorPosition(0, 26);
        //}
        public static void ClearRow(int row)
        {
            Console.SetCursorPosition(0, row);
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, row);

        }
        //public static void ClearErrorMsg()
        //{
        //    Console.SetCursorPosition(0, 28);
        //    for (int i = 0; i < Console.WindowWidth; i++)
        //    {
        //        Console.Write(" ");
        //    }
        //    Console.SetCursorPosition(0, 28);

        //}
    }
}
