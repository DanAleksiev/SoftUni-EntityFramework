﻿using Newtonsoft.Json;

namespace CarDealer.DTOs
    {
    public class SalesDTO
        {
        [JsonProperty("carId")]
        public int CarId { get; set; }

        [JsonProperty("customerId")]
        public int CustomerId { get; set; }

        [JsonProperty("discount")]
        public decimal Discount { get; set; }
        }
    }
