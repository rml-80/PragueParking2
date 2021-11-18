using Newtonsoft.Json;
using PragueParking2.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PragueParking2
{
    public class FileContext
    {
        //paths to files
        const string pathSavedParkingSpaces = @"../../../Files/SavedParkingSpaces.json";
        const string pathConfigFile = @"../../../Files/ConfigFile.json";
        const string pathPriceFile = @"../../../Files/PriceFile.txt";

        public FileContext()
        {
            CheckFile();
        }
        /// <summary>
        /// check if files exist, if not create new ones
        /// </summary>
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
                File.Create(pathSavedParkingSpaces).Close();    //Create file if missing
                CarPark CP = new();                             //Create new Car park
                WriteSavedParkingSpaces();                      //Save car park
            }
            if (!File.Exists(pathPriceFile))
            {
                File.Create(pathPriceFile).Close();     //Create file if missing
                CreatePriceList();                      //add standard values to file when created
            }
        }
        /// <summary>
        /// Create a pricelist file if missing
        /// Gets values from PriceLIst enum
        /// </summary>
        private void CreatePriceList()
        {
            string[] temp = new string[] { $"MC:{((int)PriceList.MC)}",$"CAR:{(int)PriceList.Car}" };
            File.WriteAllLines(pathPriceFile, temp);
        }
        /// <summary>
        /// Creates a new config file if missing
        /// Gets values from VehicleSize enum
        /// </summary>
        private void CreateConfigFile()
        {
            string[] vehicleTypes = { "CAR", "MC" };
            Config config = new Config((int)Sizes.ParkingSpaces, (int)Sizes.ParkingSpaceSize, (int)Sizes.CAR, (int)Sizes.MC, vehicleTypes);
            string jsonData = JsonConvert.SerializeObject(config);
            File.WriteAllText(pathConfigFile, jsonData);
        }
        /// <summary>
        /// Save parking spaces to json file
        /// </summary>
        public void WriteSavedParkingSpaces()
        {
            string parkingSpaces = JsonConvert.SerializeObject(CarPark.parkingSpaces);
            File.WriteAllText(pathSavedParkingSpaces, parkingSpaces);
        }
        /// <summary>
        /// Reads saved parking spaces from json file
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
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
                                CAR car = new(tempJsonList[i].ParkedVehicles[j].LicensePlate, tempJsonList[i].ParkedVehicles[j].TimeParked);

                                CarPark.parkingSpaces[i].ParkedVehicles.Add(car);
                                CarPark.parkingSpaces[i].AvailableSpace -= car.Size;
                            }
                            else if (tempJsonList[i].ParkedVehicles[j].type == "MC")
                            {
                                MC mc = new(tempJsonList[i].ParkedVehicles[j].LicensePlate, tempJsonList[i].ParkedVehicles[j].TimeParked);
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
                return false;
            }
            return true;
        }
        /// <summary>
        /// Reads config file on startup
        /// </summary>
        public void ReadConfigFileJson()
        {
            string jsonData = File.ReadAllText(pathConfigFile);
            _ = JsonConvert.DeserializeObject<Config>(jsonData);
        }
        /// <summary>
        /// Reads price file, wanted a simple file format for price, so that the users can't change config
        /// reads the file every time GetPrice is called. No need to reload.</summary>
        /// <param name="vehicleType"></param>
        /// <returns>
        /// double price
        /// </returns>
        public double GetPrice(string vehicleType)
        {
            string[] temp = File.ReadAllLines(pathPriceFile);
            string priceString = (vehicleType == "CAR") ? temp.FirstOrDefault(x => x.Contains("CAR")) : temp.FirstOrDefault(x => x.Contains("MC")) ;
            string[] priceArray = priceString.Split(":");
            double price = Convert.ToDouble( priceArray[1]);
            return price;
        }
        /// <summary>
        /// Opens notepad for editing prices
        /// </summary>
        public void OpenPriceFile()
        {
            Process.Start("notepad.exe", pathPriceFile);
        }
    }
}