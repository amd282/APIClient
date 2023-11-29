using APIClient.DTO;
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
        public async Task<IActionResult> IndexAsync()
        {
            List<WeatherData> weatherdatalist = new List<WeatherData>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/WeatherData/GetWeatherData").Result;
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var weatherDataList = JsonConvert.DeserializeObject<List<WeatherDataDto>>(json);

            return View(weatherDataList);
        }
    }
}
