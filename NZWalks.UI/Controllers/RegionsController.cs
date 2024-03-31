using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.Dto;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            //Get all regions from web api
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessgae = await client.GetAsync("https://localhost:7034/api/v1/regions");

                httpResponseMessgae.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessgae.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {
                //log the exception

            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7034/api/v1/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };
            var responseMessage = await client.SendAsync(httpRequestMessage);
            responseMessage.EnsureSuccessStatusCode();

            var response = await httpRequestMessage.Content.ReadFromJsonAsync<RegionDto>();
            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7034/api/v1/regions/{id.ToString()}");

            if (response is not null)
            {
                return View(response);
            }

            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto region)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7034/api/v1/regions/{region.Id.ToString()}"),
                Content = new StringContent(JsonSerializer.Serialize(region), Encoding.UTF8, "application/json")
            };

            var responseMessage = await client.SendAsync(httpRequestMessage);

            responseMessage.EnsureSuccessStatusCode();

            var response = await httpRequestMessage.Content.ReadFromJsonAsync<RegionDto>();
            if (response is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto region)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var responseMessage = await client.DeleteAsync($"https://localhost:7034/api/v1/regions/{region.Id.ToString()}");

                responseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {

            }
            return View("Edit");
        }
    }

}
