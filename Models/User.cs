using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_ArendOff.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    internal class ApplicationContext : DbContext
    {
        // Конструктор, указывающий имя строки подключения из App.config.
        public ApplicationContext() : base("ArendConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationContext>());
        }

        // DbSets, которые станут таблицами в базе ArendDB
        public DbSet<User> Users { get; set; }
       
    }
}
