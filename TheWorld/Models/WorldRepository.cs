﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{   /// <summary>
    /// An interface to be implemented in test scenarios easelly
    /// </summary>
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(
            WorldContext context, 
            ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IEnumerable<Trip> GetAllTrips()
        {
            //logging messages
            _logger.LogInformation("Getting all trips from the Database");
            return _context.Trips.ToList();
        }
    }
}
