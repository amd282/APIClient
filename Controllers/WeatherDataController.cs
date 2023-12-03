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
        
        public async Task<IActionResult> Search(int? cityId)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/WeatherData/GetWeatherDataByCityId/{cityId}");
            var json = await response.Content.ReadAsStringAsync();
            if (json.Contains("NaN")) {
                return RedirectToAction("Index","WeatherData");
            }

           
            var weatherDataList = JsonConvert.DeserializeObject<List<WeatherDataDto>>(json);


            return View(weatherDataList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(WeatherDataDto model)
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
            WeatherDataDto weatherdata = new WeatherDataDto();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/WeatherData/GetWeatherData/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                weatherdata = JsonConvert.DeserializeObject<WeatherDataDto>(data);
            }
            return View(weatherdata);
        }

        [HttpPost]
        public IActionResult Edit(WeatherDataDto model)
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
            WeatherDataDto weatherdata = new WeatherDataDto();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/WeatherData/GetWeatherData/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content?.ReadAsStringAsync().Result;
                weatherdata = JsonConvert.DeserializeObject<WeatherDataDto>(data);
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

        [HttpGet]
        public IActionResult Patch(int id)
        {
            WeatherDataDto originalWeatherData = GetWeatherDataById(id);

            if (originalWeatherData == null)
            {
                return NotFound($"Weather data not found with id: {id}");
            }

            return View(originalWeatherData);
        }

        [HttpPost]
        public IActionResult Patch(int id, WeatherDataDto patchDto)
        {
            try
            {
                if (patchDto == null)
                {
                    return BadRequest("Patch data is null");
                }

                WeatherDataDto originalWeatherData = GetWeatherDataById(id);

                if (originalWeatherData == null)
                {
                    return NotFound($"Weather data not found with id: {id}");
                }

                // Update only the non-null properties
                if (patchDto.Conditions != null)
                {
                    originalWeatherData.Conditions = patchDto.Conditions;
                }

                if (patchDto.Temperature.HasValue)
                {
                    originalWeatherData.Temperature = patchDto.Temperature.Value;
                }

                if (patchDto.WindSpeed.HasValue)
                {
                    originalWeatherData.WindSpeed = patchDto.WindSpeed.Value;
                }

                if (patchDto.Date.HasValue)
                {
                    originalWeatherData.Date = patchDto.Date.Value;
                }

                string data = JsonConvert.SerializeObject(originalWeatherData);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PatchAsync($"{_client.BaseAddress}/WeatherData/PatchWeatherData/{id}", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Failed to update weather data";
                    return View(originalWeatherData);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        private WeatherDataDto GetWeatherDataById(int id)
        {
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/WeatherData/GetWeatherData/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<WeatherDataDto>(data);
            }

            return null;
        }

    }
}
