﻿using Newtonsoft.Json;

namespace APIClient.Models
{
    public class WeatherData
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Conditions { get; set; }
        public decimal Temperature { get; set; }
        public decimal WindSpeed { get; set; }
        public DateTime Date { get; set; }
        [JsonProperty("cityName")]
        public Cities city { get; set; }
    }
}
