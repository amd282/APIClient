using APIClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace APIClient.Controllers
{
    public class WeatherForecastController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7156/api");
        private readonly HttpClient _client;

        public WeatherForecastController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<WeatherForecast> weatherforecastlist = new List<WeatherForecast>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/WeatherForecast/Get").Result;

            if(response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                weatherforecastlist = JsonConvert.DeserializeObject<List<WeatherForecast>>(data);
            }
            return View(weatherforecastlist);
        }
    }
}
