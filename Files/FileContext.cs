using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PragueParking2
{
    public class FileContext
    {
        string pathSavedParkingSpaces = @"../../../Files/SavedParkingSpaces.json";
        string pathConfigFile = @"../../../Files/ConfigFile.json";
        string pathPriceFile = @"../../../Files/PriceFile.txt";
        //TODO create a config file with number of parkingspaces in carpark and each vehicle parked

        public FileContext()
        {
            CheckFile();
        }
        public void CheckFile()
        {
            // Check if filesexists, if not create them
            //TODO when creating files set values to something
            if (!File.Exists(pathSavedParkingSpaces))
            {
                File.Create(pathSavedParkingSpaces).Close();

            }
            if (!File.Exists(pathConfigFile))
            {
                File.Create(pathConfigFile).Close();
            }
            if (!File.Exists(pathPriceFile))
            {
                File.Create(pathPriceFile).Close();
            }
        }
        public void WriteSavedParkingSpaces()
        {
            string parkingSpaces = JsonConvert.SerializeObject(CarPark.parkingSpaces);
            File.WriteAllText(pathSavedParkingSpaces, parkingSpaces);
        }
        //TODO change return type to List<ParkingSpace>
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
                            if (tempJsonList[i].ParkedVehicles[j].type == "CAR")
                            {
                                CAR car = new(tempJsonList[i].ParkedVehicles[j]);

                                CarPark.parkingSpaces[i].ParkedVehicles.Add(car);
                                CarPark.parkingSpaces[i].AvailableSpace -= car.Size;
                            }
                            else if (tempJsonList[i].ParkedVehicles[j].type == "MC")
                            {
                                MC mc = new(tempJsonList[i].ParkedVehicles[j].LicensePlate);
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
        public void ReadConfigFileJson()
        {
            //TODO read config file
            throw new System.NotImplementedException();
        }

        public void WriteConfigFileJson()
        {
            //TODO write to config file
            throw new System.NotImplementedException();
        }

        public void ReadPriceFile()
        {
            //TODO read price file, want a simple file format for price, so that the users can't change config 
            throw new System.NotImplementedException();
        }

        public void ReloadFiles()
        {
            //TODO read files again
            throw new System.NotImplementedException();
        }
    }
}