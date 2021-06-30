using Core.Calendar.Models;

namespace Core.Calendar.Services
{
    public interface IAvailabilityService
    {
        void InsertAvailability(Availability availability, string username);
    }
}