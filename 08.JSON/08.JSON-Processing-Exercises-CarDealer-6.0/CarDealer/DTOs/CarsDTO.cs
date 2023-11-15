using Newtonsoft.Json;

namespace CarDealer.DTOs
    {
    public class CarsDTO
        {
        public CarsDTO(IEnumerable<int> partsID)
            {
            this.PartsId = new List<int>();
            }

        [JsonProperty("make")]
        public string Make { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("traveledDistance")]
        public int TravelDistance { get; set; }

        [JsonProperty("partsId")]
        public IEnumerable<int> PartsId { get; set; }
        }
    }
