using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Calendar.Data;
using Core.Calendar.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Calendar.Repositories
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
    }
}