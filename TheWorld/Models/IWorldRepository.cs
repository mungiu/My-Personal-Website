using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripByName(string tripName);

        //sending trip to underlying context
        void AddTrip(Trip trip);
        void AddStop(string tripName, Stop newStop);

        //testing if save was succesfull (true/false) + saving changes
        Task<bool> SaveChangesAsync();
    }
}