using TripBuddy.Models;

using Microsoft.EntityFrameworkCore;
 
namespace TripBuddy.Models
{
    public class contextContext : DbContext
    {
        
        public contextContext(DbContextOptions<contextContext> options) : base(options) { }
        public DbSet<TripMate> tripmates { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Trip> trips { get; set; }
        
    }
}
