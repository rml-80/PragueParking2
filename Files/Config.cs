using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2.Files
{
    class Config
    {
        [JsonProperty("NumberOfSpaces")]
        public static int NumberOfSpaces { get; set; }
        [JsonProperty("SpaceSize")]
        public static int SpaceSize { get; set; }
        [JsonProperty("CarSize")]
        public static int CarSize { get; set; }
        [JsonProperty("MCSize")]
        public static int MCSize { get; set; }
        [JsonProperty("VehileTypes")]
        public static string[] VehicleTypes { get; set; }
        [JsonConstructor]
        public Config(int numberOfSpaces,int spaceSize , int carSize, int mcSize, string [] vehicleTypes)
        {
            NumberOfSpaces = numberOfSpaces;
            SpaceSize = spaceSize;
            CarSize = carSize;
            MCSize = mcSize;
            VehicleTypes = vehicleTypes;
        }
    }
}
