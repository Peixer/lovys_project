using System.Collections.Generic;
using System.Threading.Tasks;
using Lovys.Core.Calendar.Models;

namespace Lovys.Core.Calendar.Services
{
    public interface IAvailabilityService
    {
        void InsertAvailability(Availability availability, string username);
        bool IsValidSlotTime(Availability availability);
        Task<List<Availability>> GetAvailabilitiesByUserId(List<string> userIds);
    }
}