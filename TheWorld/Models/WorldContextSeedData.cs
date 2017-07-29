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
        //ctor
        public WorldContextSeedData(WorldContext context)
        {
            _context = context;
        }

        
        //one of our future calls will be "async"
        public async Task EnsureSeedData()
        {
            //checking for data in databse
            if (!_context.Trips.Any())
            {
                //creating a trip instance
                var workExperience = new Trip()
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "My International Work Experience",
                    UserName = "", // TODO Add UserName
                    Stops = new List<Stop>()
                    {
                        new Stop() {  Name = "Chisinau, Moldova", Arrival = new DateTime(2009, 01, 01), Latitude = 28.8638, Longitude = 47.0105, Order = 0 },
                        new Stop() {  Name = "AbuDhabi, UAE", Arrival = new DateTime(2013, 01, 01), Latitude = 24.466667, Longitude = 54.366669, Order = 1 },
                        new Stop() {  Name = "Dubai, UAE", Arrival = new DateTime(2014, 01, 01), Latitude = 25.276987, Longitude = 55.296249, Order = 2 },
                        new Stop() {  Name = "Doha, Qatar", Arrival = new DateTime(2015, 01, 01), Latitude = 25.286106, Longitude = 51.534817, Order = 3 },
                        new Stop() {  Name = "Horsens, Denmark", Arrival = new DateTime(2016, 01, 01), Latitude = 55.8581, Longitude = 9.8476, Order = 4 },
                    }
                };

                //adding trips to the context
                //adding stops to the stops table in the database
                _context.Trips.Add(workExperience);
                _context.Stops.AddRange(workExperience.Stops);

                var wedingTrip = new Trip()
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "Going to my Weding",
                    UserName = "", //TODO Add UserName
                    Stops = new List<Stop>()
                    {
                        new Stop() {  Name = "Chisinau, Moldova", Arrival = new DateTime(2014, 6, 9), Latitude = 28.8638, Longitude = 47.0105, Order = 0 },
                    }
                };

                _context.Trips.Add(wedingTrip);
                _context.Stops.AddRange(wedingTrip.Stops);

                //pushes data to the database
                await _context.SaveChangesAsync();
            }
        }
    }
}
