using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This represents the access to the database itself
/// </summary>
namespace TheWorld.Models
{
    public class WorldContext : DbContext
    {
        //ctor
        private IConfigurationRoot _config;
        public WorldContext(IConfigurationRoot config, 
            DbContextOptions options) 
            : base(options)
        {
            _config = config;
        }

        //exposing "entity types" as properties of "DbSet type"
        //now we can execute LINQ queries against "Trips/Stops"
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer
                (_config["ConnectionStrings:WorldContextConnection"]);
        }
    }
}
