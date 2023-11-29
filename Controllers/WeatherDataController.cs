using APIClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using APIClient.DTO;

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
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/WeatherData/GetWeatherData").Result;

          
                var json = await response.Content.ReadAsStringAsync();
                var weatherDataList = JsonConvert.DeserializeObject<List<WeatherDataDto>>(json);
            
            return View(weatherDataList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(WeatherData model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/WeatherData/PostWeatherData", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Weather data Created";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            WeatherData weatherdata = new WeatherData();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/WeatherData/GetWeatherData/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                weatherdata = JsonConvert.DeserializeObject<WeatherData>(data);
            }
            return View(weatherdata);
        }

        [HttpPost]
        public IActionResult Edit(WeatherData model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/WeatherData/PutWeatherData", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            WeatherData weatherdata = new WeatherData();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/WeatherData/GetWeatherData/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content?.ReadAsStringAsync().Result;
                weatherdata = JsonConvert.DeserializeObject<WeatherData>(data);
            }
            return View(weatherdata);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/WeatherData/DeleteWeatherData/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(response);
        }
    }
}
