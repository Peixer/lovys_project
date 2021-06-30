using System.Collections.Generic;
using System.Threading.Tasks;
using Lovys.Core.Calendar.Entities;

namespace Lovys.Core.Calendar.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<bool> Insert(Availability availability);
        Task<List<Availability>> Find();        
        Task<List<Availability>> Find(List<string> userIds);      
    }
}