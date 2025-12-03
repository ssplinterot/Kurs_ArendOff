using Kurs_ArendOff.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_ArendOff.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=ArendConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Place> Places { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Создаём админа при первом запуске
            var adminHash = BCrypt.Net.BCrypt.HashPassword("admin123");
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Login = "admin", PasswordHash = adminHash, FullName = "Администратор", Role = "Admin" }
            );
        }
    }
}
