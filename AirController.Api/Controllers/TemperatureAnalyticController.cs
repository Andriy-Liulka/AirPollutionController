using AirController.Api.Models;
using AirController.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AirController.Api.Controllers
{
    public class TemperatureAnalyticController : Controller
    {
        private readonly IRepository _repository;

        public TemperatureAnalyticController(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> TemperatureAnalytic()
            => Ok(await _repository.GetTemperatureAnalyticItemAsync());
        
        [HttpPost]
        public async Task<IActionResult> InsertData(TemperatureAnalyticModel temperatureAnalyticModel)
        {
            await _repository.InsertTemperatureAsync(temperatureAnalyticModel);
            return Ok("Data was inserted successfully!");
        }
    }
}
