using AirController.Api.Models;
using AirController.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AirController.Api.Controllers
{
    public class MelodyKeeperController : Controller
    {
		private readonly IRepository _repository;

		public MelodyKeeperController(IRepository repository)
		{
			_repository = repository;
		}

		public IActionResult MyChosenSong()
            => View(_repository.GetSong());

        [HttpPost]
        public void SetSong([FromQuery] SongModel songModel)
            => _repository.SetSong(songModel);
	}
}
