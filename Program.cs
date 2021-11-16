using PragueParking2.Menus;
using System;
using System.Collections.Generic;

namespace PragueParking2
{
    class Program
    {
        static void Main(string[] args)
        {
            CarPark CP = new();
            FileContext FC = new FileContext();
            //FC.WriteSavedParkingSpaces();
            bool succes = FC.ReadSavedParkingSpaces();    //Read saved file and populate parkingspaces list
            if (succes)
            {
                bool loop = true;
                do
                {
                    Console.Clear();
                    CP.PrintCarPark();
                    Console.WriteLine();
                    //TODO move to Menu class
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
                                CP.ParkVehicle();
                                break;
                            case 2:
                                CP.RetriveVehicle();
                                break;
                            case 3:
                                CP.MoveVehicle();
                                break;
                            case 4:
                                CP.SearchVehicle(out int? space);
                                break;
                            default:
                                break;
                        }
                    }
                } while (loop);

            }
            else
            {
                Console.WriteLine("Something went very wrong. Restart application.");
            }
            void PrintLineForMenu()     //TODO move to menu class
            {
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    Console.Write("─");
                }
            }
        }
    }
}
