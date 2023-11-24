using APIClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIClient.Controllers
{
    public class WeatherDataController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7156/api");
        private readonly HttpClient _client;

        public WeatherDataController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<WeatherData> weatherdatalist = new List<WeatherData>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/WeatherData/GetWeatherData").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                weatherdatalist = JsonConvert.DeserializeObject<List<WeatherData>>(data);
            }
            return View(weatherdatalist);
        }
    }
}
