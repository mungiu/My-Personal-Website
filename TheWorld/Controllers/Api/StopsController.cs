using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<StopsController> _logger;

        public StopsController(IWorldRepository repository, 
            ILogger<StopsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //returning stops list & catching errors
        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName);

                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(
                    trip.Stops.OrderBy(s => s.Order).ToList()));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get stops: {ex}");
            }

            return BadRequest("Failed to get stops");

        }

        //returning stops list & catching errors
        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, 
            [FromBody]StopViewModel vm)
        {
            try
            {
                // TODO: See if VM is valid
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);
                    // Lookup the geocodes (long/lat)

                    //save to database
                    _repository.AddStop(tripName, newStop);

                    if (await _repository.SaveChangesAsync())
                    {
                    return Created($"api/trips/{tripName}/stops/{newStop.Name}",
                        Mapper.Map<StopViewModel>(newStop));
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save new Stop: {ex}");
            }

            return BadRequest("Failed to save new stop");
        }
    }
}
