using Kurs_ArendOff.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics.Contracts;
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
        public ApplicationContext() : base("ArendConnection")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationContext>());
        }
        public DbSet<User> Users { get; set; }// DbSets, которые станут таблицами в базе ArendDB
        public DbSet<OrganizationData> OrganizationDatas { get; set; }
        public DbSet<Place> Places { get; set; }
    }
}
