using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lovys.Core.Calendar.Data;
using Lovys.Core.Calendar.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lovys.Core.Calendar.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly APIContext _apiContext;

        public AvailabilityRepository(APIContext apiContext)
        {
            _apiContext = apiContext;
        }

        public async Task<bool> Insert(Availability availability)
        {
            this._apiContext.Availabilities.Add(availability);
            await this._apiContext.SaveChangesAsync();

            return true;
        }

        public Task<List<Availability>> Find()
        {
            return this._apiContext.Availabilities
                .Include(u => u.User)
                .ToListAsync();
        }

        public Task<List<Availability>> Find(List<string> userIds)
        {
            return this._apiContext.Availabilities
                .Include(u => u.User)
                .Where(x => userIds.Contains(x.User.Id))
                .ToListAsync();
        }
    }
}