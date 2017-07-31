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
    //creating the default route for the entire class
    //will be pushed to [HttpGet] / [HttpPost] requests
    //any additional rout added to [HttpGet(or Post)] 
    //will not replace but ammend the default route
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<TripsController> _logger;

        //ctor
        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            //storing
            _repository = repository;
            _logger = logger;
        }

        //specifying method for a URL "get" request
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetAllTrips();
                //returning a collection of trips converted into a collection of "TripViewModel"
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch (Exception ex)
            {
                //logging error
                _logger.LogError($"Failed to get all trips: {ex}");
                return BadRequest("Error occured");
            }
        }

        //specifying method for a URL "post" request
        //[FromBody] - model bind data coming as "post" to "this object"
        //by matching name properties of Json with name properties of this object
        //Note: data doesn't need to match "1on1" i will simply try match as many as possible
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                //TODO save to Databse
                var newTrip = Mapper.Map<Trip>(theTrip);
                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    //sending the object as it looks after any databse calls
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
            }

            return BadRequest("Failed to save the trip");
        }
    }
}
