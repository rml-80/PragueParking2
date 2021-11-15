using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PragueParking2
{
    class CarPark
    {
        static FileContext FC = new();
        private int Size { get; } = 100; //TODO move to config file
        public static List<ParkingSpace> parkingSpaces = new();
        public CarPark()
        {
            for (int i = 0; i < Size; i++)
            {
                parkingSpaces.Add(new ParkingSpace(i + 1, (int)VehicleSize.ParkingSpace));
            }
        }
        public void PrintCarPark()
        {
            //TODO print out all parking spaces to console in a prettier way
            foreach (var space in CarPark.parkingSpaces)
            {
                if (space.ParkedVehicles != null)
                {
                    foreach (var vehicle in space.ParkedVehicles)
                    {
                        Console.WriteLine($"{space.SpaceNumber}: {vehicle.GetType().Name} {vehicle.LicensePlate}");
                    }
                }
                else
                {
                    Console.WriteLine($"{space.SpaceNumber}: Empty");
                }
            }
        }

        public Vehicle CreateNewVehicle()   //TODO needs licensplate
        {
            Console.WriteLine("1. Car\t2. MC");
            bool inputOk = int.TryParse(Console.ReadLine(), out int choice);
            Vehicle vehicle;
            Console.Write("Enter license plate: ");
            switch (choice)
            {
                case 1:
                    return vehicle = new CAR(Console.ReadLine());
                    break;
                case 2:
                    return vehicle = new MC(Console.ReadLine());
                    break;
                default:
                    return null;
                    break;
            }
        }
        //public CAR CreateNewCar()           //TODO needs license plate
        //{
        //    //TODO create a new car
        //    Console.Write("Enter license plate: ");
        //    CAR car = new(Console.ReadLine());
        //    return car;

        //}
        //public MC CreateNewMC()             //TODO needs license plate
        //{
        //    //TODO create a new MC
        //    throw new System.NotImplementedException();
        //}
        public int FindSpace(int size)
        {
            int? space = null;
            //TODO find the first space with enough of available space

            for (int i = 0; i < parkingSpaces.Count; i++)
            {
                if (parkingSpaces[i].AvailableSpace >= size)
                {
                    return (int)(space = i);
                }
            }
            return (int)space;
        }

        public Vehicle SearchVehicle(out int? space)         //TODO needs licens plate, out int space
        {
            //TODO search for a vehicle
            space = null;
            Vehicle vehicle = null;
            while (vehicle == null)
            {
                Console.Write("Enter license plate: ");
                string licensePlate = Console.ReadLine();
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
                                return vehicle;
                            }
                        }

                    }
                }
                Console.WriteLine("No vehicle parked here with that license plate!");
            }
            return null;
        }

        public bool ParkVehicle()           //TODO needs vehicle and space number
        {
            //TODO park vehicle to the specified spce number
            Vehicle vehicle = CreateNewVehicle();
            int space = FindSpace(vehicle.Size);
            if (parkingSpaces[space].ParkedVehicles == null)
            {
                parkingSpaces[space].ParkedVehicles = new List<Vehicle>();
            }
            parkingSpaces[space].ParkedVehicles.Add(vehicle);
            parkingSpaces[space].AvailableSpace -= vehicle.Size;
            FC.WriteSavedParkingSpaces();
            return true;

        }

        public bool RetriveVehicle()        //TODO needs vehicle, or license plate?
        {
            //TODO search for vehicle
            //TODO retrive vehicle

            Vehicle vehicle = SearchVehicle(out int? space);

            parkingSpaces[(int)space].ParkedVehicles.Remove(vehicle);
            parkingSpaces[(int)space].AvailableSpace += vehicle.Size;
            if (parkingSpaces[(int)space].ParkedVehicles.Count == 0)
            {
                parkingSpaces[(int)space].ParkedVehicles = null;
            }
            FC.WriteSavedParkingSpaces();
            return true;
        }

        public bool MoveVehicle()           //TODO needs vehicle, or license plate, space and new space
        {
            //TODO move specified vehicle from one space to another
            Vehicle vehicle = SearchVehicle(out int? space);
            parkingSpaces[(int)space].ParkedVehicles.Remove(vehicle);
            parkingSpaces[(int)space].AvailableSpace += vehicle.Size;
            if (parkingSpaces[(int)space].ParkedVehicles.Count == 0)
            {
                parkingSpaces[(int)space].ParkedVehicles = null;
            }
            do
            {
                Console.Write("Move to: ");
                bool inputOK = int.TryParse(Console.ReadLine(), out int newSpace);
                if (inputOK && newSpace < Size)
                {
                    if (parkingSpaces[newSpace].AvailableSpace >= vehicle.Size)
                    {
                        if (parkingSpaces[newSpace].ParkedVehicles == null)
                        {
                            parkingSpaces[newSpace].ParkedVehicles = new();
                        }
                        parkingSpaces[newSpace].ParkedVehicles.Add(vehicle);
                        parkingSpaces[newSpace].AvailableSpace -= vehicle.Size;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Space occupied.");
                    }
                }
                else
                {
                    Console.WriteLine("Input type was wrong.");
                }
            } while (true);
            return false;
        }

        public void PrintTicket()   //TODO needs vehicle and space number
        {
            //TODO print ticket to customer
            throw new System.NotImplementedException();
        }
        public void PrintRecipe()   //TODO needs vehicle
        {
            //TODO print recipe to customer
            TimeSpan duration = CalculateDuration();
            float price = CalculatePrice();
            throw new System.NotImplementedException();
        }
        public TimeSpan CalculateDuration() //TODO needs vehicle.TimeParked
        {
            throw new System.NotImplementedException();
        }
        public float CalculatePrice()       //TODO needs duration
        {
            throw new System.NotImplementedException();
        }
    }
}
