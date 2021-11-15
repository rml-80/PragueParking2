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
                    CP.PrintCarPark();
                    PrintLine();
                    Console.Write("1. Park\t2. Retrive\t3. Move\t4. Search\t0. Quit\n");
                    PrintLine();
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
                                //CP.SearchVehicle();
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
        void PrintLine()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }
        }
        }
    }
}
