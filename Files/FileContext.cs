using Newtonsoft.Json;
using PragueParking2.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PragueParking2
{
    public class FileContext
    {
        //path to files
        public string pathSavedParkingSpaces = @"../../../Files/SavedParkingSpaces.json";
        string pathConfigFile = @"../../../Files/ConfigFile.json";
        string pathPriceFile = @"../../../Files/PriceFile.txt";

        public FileContext()
        {
            CheckFile();
        }
        //check if files exist, if not create new ones
        public void CheckFile()
        {
            // Check if filesexists, if not create them
            if (!File.Exists(pathConfigFile))
            {
                File.Create(pathConfigFile).Close();    //Create file if missing
                CreateConfigFile();                     //add standard values to file when created
            }
            else
            {
                ReadConfigFileJson();
            }
            if (!File.Exists(pathSavedParkingSpaces))
            {
                //Create empty file for parking spaces, no need to save empty carpark.
                File.Create(pathSavedParkingSpaces).Close();
                CarPark CP = new();
                WriteSavedParkingSpaces();
            }
            if (!File.Exists(pathPriceFile))
            {
                File.Create(pathPriceFile).Close();
                CreatePriceList();
            }
        }

        private void CreatePriceList()
        {
            string[] temp = new string[] { $"MC:{((int)PriceList.MC)}",$"CAR:{(int)PriceList.Car}" };
            File.WriteAllLines(pathPriceFile, temp);
        }

        //Create new config file
        private void CreateConfigFile()
        {
            string[] vehicleTypes = { "CAR", "MC" };
            Config config = new Config((int)Sizes.ParkingSpaces, (int)Sizes.ParkingSpaceSize, (int)Sizes.CAR, (int)Sizes.MC, vehicleTypes);
            string jsonData = JsonConvert.SerializeObject(config);
            File.WriteAllText(pathConfigFile, jsonData);
        }
        //save parking spaces to json file
        public void WriteSavedParkingSpaces()
        {
            string parkingSpaces = JsonConvert.SerializeObject(CarPark.parkingSpaces);
            File.WriteAllText(pathSavedParkingSpaces, parkingSpaces);
        }
        //read saved parking spaces
        public bool ReadSavedParkingSpaces()
        {
            try
            {
                string jsonData = File.ReadAllText(pathSavedParkingSpaces);
                List<ParkingSpace> tempJsonList = JsonConvert.DeserializeObject<List<ParkingSpace>>(jsonData);
                for (int i = 0; i < tempJsonList.Count; i++)
                {
                    if (tempJsonList[i].ParkedVehicles != null)
                    {
                        if (CarPark.parkingSpaces[i].ParkedVehicles == null)
                        {
                            CarPark.parkingSpaces[i].ParkedVehicles = new();
                        }
                        for (int j = 0; j < tempJsonList[i].ParkedVehicles.Count; j++)
                        {
                            //create new objects of right type
                            if (tempJsonList[i].ParkedVehicles[j].type == "CAR")
                            {
                                CAR car = new(tempJsonList[i].ParkedVehicles[j]);

                                CarPark.parkingSpaces[i].ParkedVehicles.Add(car);
                                CarPark.parkingSpaces[i].AvailableSpace -= car.Size;
                            }
                            else if (tempJsonList[i].ParkedVehicles[j].type == "MC")
                            {
                                MC mc = new(tempJsonList[i].ParkedVehicles[j]);
                                CarPark.parkingSpaces[i].ParkedVehicles.Add(mc);
                                CarPark.parkingSpaces[i].AvailableSpace -= mc.Size;
                            }
                        }
                    }
                }
                tempJsonList.Clear(); //clear list no use of it any more
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong." + e.Message);
            }
            return true;
        }
        //read config file
        public void ReadConfigFileJson()
        {
            string jsonData = File.ReadAllText(pathConfigFile);
            _ = JsonConvert.DeserializeObject<Config>(jsonData);
        }
        //read price file, wanted a simple file format for price, so that the users can't change config
        //TODO needs to be reloadable, need to get prices in a better way
        public double GetPrice(Vehicle vehicleType)
        {
            string[] temp = File.ReadAllLines(pathPriceFile);
            string priceString = (vehicleType.GetType() == typeof(CAR)) ? temp.FirstOrDefault(x => x.Contains("CAR")) : temp.FirstOrDefault(x => x.Contains("MC")) ;
            string[] priceArray = priceString.Split(":");
            double price = Convert.ToDouble( priceArray[1]);
            return price;
        }
    }
}