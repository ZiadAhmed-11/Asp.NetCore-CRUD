using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PersonsDbContext :DbContext
    {
        public DbSet<Person> Persons{get; set;}
        public DbSet<Country> Countries { get; set;}

        public PersonsDbContext( DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //seed countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries= System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            
            foreach(Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            //seed persons
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }
        }
    }
}
