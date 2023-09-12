using Microsoft.EntityFrameworkCore;
using NyeriEvents.Entities;

namespace NyeriEvents.Data
{
    public class EventDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

      




    }
}
