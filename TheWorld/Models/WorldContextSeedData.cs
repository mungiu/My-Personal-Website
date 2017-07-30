using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldContextSeedData
    {
        private WorldContext _context;

        public WorldContextSeedData(WorldContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
            //since the database was previously populated 
            //the below inputs will not go through due to the "!"
            if (!_context.Trips.Any())
            {
                var usTrip = new Trip()
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "US Trip",
                    UserName = "", //TODO Add UserName
                    Stops = new List<Stop>()
                    {
                        //input here seed data for empty databse case
                    }
                };
                //adding the trip to the database
                //adding the stops to the database
                _context.Trips.Add(usTrip);
                _context.Stops.AddRange(usTrip.Stops);


                var worldTrip = new Trip()
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "World Trip",
                    UserName = "", //TODO Add UserName
                    Stops = new List<Stop>()
                    {
                        //input here seed data for empty databse case
                    }
                };

                _context.Trips.Add(worldTrip);
                _context.Stops.AddRange(worldTrip.Stops);
                //pushing data to the database
                await _context.SaveChangesAsync();
            }
        }
    }
}
