using AirController.Api.Models;
using AirController.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AirController.Api.Controllers
{
    public class AirPollutionController : Controller
    {
        private readonly IRepository _repository;
       
        public AirPollutionController(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> AirPollutionStatistics()
            => View(await _repository.GetAllDataAsync());

        [HttpPost]
        public async Task<IActionResult> InsertData(AirPollutionAnalytic airPollutionUnit)
        {
            await _repository.InsertAirPollutionAsync(airPollutionUnit);
            return Ok("Data was inserted successfully!");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteForId([FromQuery] string guid)
        {
            await _repository.DeleteForIdAsync(guid);
            return RedirectToAction("AirPollutionStatistics");
        }
    }
}