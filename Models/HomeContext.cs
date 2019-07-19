using Microsoft.EntityFrameworkCore;

namespace Exam.Models
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Event> Events {get;set;}
        public DbSet<Reservation> Rsvps {get;set;}
    }
}