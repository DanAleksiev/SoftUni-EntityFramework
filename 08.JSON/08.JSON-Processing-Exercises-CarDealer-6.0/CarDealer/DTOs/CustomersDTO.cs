using Newtonsoft.Json;

namespace CarDealer.DTOs
    {
    public class CustomersDTO
        {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("isYoungDriver")]
        public bool isYoungDriver { get; set; }
        }
    }
