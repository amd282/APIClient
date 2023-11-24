using APIClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    }
}
