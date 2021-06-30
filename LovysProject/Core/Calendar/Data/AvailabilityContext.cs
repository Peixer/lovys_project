using Core.Calendar.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Calendar.Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options)
            : base(options)
        {
        }

        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<User> Users { get; set; }
    }
}