using Microsoft.EntityFrameworkCore;
using Portfolio.API.Models;


namespace Portfolio.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
