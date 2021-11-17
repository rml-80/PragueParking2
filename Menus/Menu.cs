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
                Console.WriteLine("{0,-10}{1,-15}{2,-15}{3,-15}{4,-10}", "1. Park", "2. Retrive", "3. Move", "4. Search", "0. Close application");
                PrintLineForMenu();

                Console.Write("Choice: ");
                bool inputOK = int.TryParse(Console.ReadLine(), out int choice);
                if (inputOK)
                {
                    switch (choice)
                    {
                        case 0:
                            loop = false;
                            break;
                        case 1:
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
                        default:
                            break;
                    }
                }
            } while (loop);

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
    }
}
