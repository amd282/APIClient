using APIClient.DTO;
using APIClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace APIClient.Controllers
{
    public class CitiesController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7156/api");
        private readonly HttpClient _client;

        public CitiesController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Cities> citieslist = new List<Cities>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Cities/GetCities").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                citieslist = JsonConvert.DeserializeObject<List<Cities>>(data);
            }
            return View(citieslist);
        }

        [HttpGet] 
        public IActionResult Create() 
        {
            return View();        
        }

        [HttpPost]
        public IActionResult Create(Cities model) 
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Cities/PostCities", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "City Created"; 
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
            Cities city = new Cities();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Cities/GetCities/" + id).Result;
            if (response.IsSuccessStatusCode) 
            {
                string data = response.Content.ReadAsStringAsync().Result;
                city = JsonConvert.DeserializeObject<Cities>(data);
            }
            return View(city);
        }

        [HttpPost]
        public IActionResult Edit(Cities model) 
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Cities/PutCity", content).Result;
            if (response.IsSuccessStatusCode) 
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id) 
        {
            Cities city = new Cities();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Cities/GetCities/" + id).Result;
            if (response.IsSuccessStatusCode) 
            {
                string data = response.Content?.ReadAsStringAsync().Result;
                city = JsonConvert.DeserializeObject<Cities>(data);
            }
            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id) 
        {
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Cities/DeleteCities/" + id).Result;
            if (response.IsSuccessStatusCode) 
            {
                return RedirectToAction("Index");
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            List<WeatherDataDto> weatherdatalist = new List<WeatherDataDto>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/WeatherData/GetWeatherDataByCityId/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                weatherdatalist = JsonConvert.DeserializeObject<List<WeatherDataDto>>(data);
            }
            return View(weatherdatalist);
        }

        [HttpGet]
        public IActionResult Patch(int id)
        {
            Cities originalCityData = GetCityById(id);

            if (originalCityData == null)
            {
                return NotFound($"City not found with name: {id}");
            }

            return View(originalCityData);
        }

        [HttpPost]
        public IActionResult Patch(int id, Cities patchDto)
        {
            try
            {
                if (patchDto == null)
                {
                    return BadRequest("Patch data is null");
                }

                Cities originalCityData = GetCityById(id);

                if (originalCityData == null)
                {
                    return NotFound($"City not found with id: {id}");
                }

                // Update only the non-null properties
                if (patchDto.Name != null)
                {
                    originalCityData.Name = patchDto.Name;
                }

                if (patchDto.Province != null)
                {
                    originalCityData.Province = patchDto.Province;
                }

                string data = JsonConvert.SerializeObject(originalCityData);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PatchAsync(_client.BaseAddress + "/Cities/PatchCity/" + id, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Failed to update weather data";
                    return View(originalCityData);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        private Cities GetCityById(int id)
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Cities/GetCityById/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Cities>(data);
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> Search(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/Cities/GetCityById/" + id);

            string data = await response.Content.ReadAsStringAsync();
            var city = JsonConvert.DeserializeObject<Cities>(data);

            var cityList = new List<Cities> { city };

            return View(cityList);
        }
    }

   

}
