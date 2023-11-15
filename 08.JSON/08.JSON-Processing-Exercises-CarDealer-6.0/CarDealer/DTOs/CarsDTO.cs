using Newtonsoft.Json;

namespace CarDealer.DTOs
    {
    public class CarsDTO
        {
        public CarsDTO()
            {
            this.PartsIds = new HashSet<int>();
            }

        [JsonProperty("make")]
        public string Make { get; set; } = null!;

        [JsonProperty("model")]
        public string Model { get; set; } = null!;

        [JsonProperty("traveledDistance")]
        public int TraveledDistance { get; set; }

        [JsonProperty("partsId")]
        public virtual ICollection<int> PartsIds { get; set; }
        }
    }
