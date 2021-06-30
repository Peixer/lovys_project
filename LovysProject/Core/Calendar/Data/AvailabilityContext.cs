using Core.Calendar.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Calendar.Data
{
    public class AvailabilityContext : DbContext
    {
        public AvailabilityContext(DbContextOptions<AvailabilityContext> options)
            : base(options)
        {
        }

        public DbSet<Availability> Availabilities { get; set; }
    }
}