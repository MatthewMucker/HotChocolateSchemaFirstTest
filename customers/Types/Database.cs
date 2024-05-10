using System;
using Bogus;

using Microsoft.EntityFrameworkCore;

namespace customers.Types
{
    public class ApplicationDbContext : DbContext
    {
        //Define our database context
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //Create abstractions of our entity tables
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        //Model the PK/FK relationships between entities
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>()
                .HasMany(c => c.Addresses)
                .WithOne(c => c.Customer)
                .HasForeignKey(c => c.CustomerID)
                .HasPrincipalKey(c => c.ID);
        }
    }

    public static class DatabaseCreator
    {
        public static void Create(string filename, int numCustomers)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite($"Data Source = {filename}")
                .Options;

            var ctx = new ApplicationDbContext(options);
            ctx.Database.EnsureCreated();

            var rnd = new Random();

            var customerFaker = new Faker<Customer>()
                .StrictMode(false)
                .RuleFor(c => c.ID, _ => Guid.NewGuid().ToString())
                .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                .RuleFor(c => c.ContactPersonName, f => f.Name.FullName())
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumberFormat())
                .RuleFor(c => c.DiscountPercent, _ => { var g = rnd.NextDouble(); return g< 0.2 ? (float)g : null; });

            var customers = customerFaker.Generate(numCustomers);
            ctx.Customers.AddRange(customers);
            ctx.SaveChanges();

            var addressFaker = new Faker<Address>()
                .StrictMode(false)
                .RuleFor(a => a.ID, _ => Guid.NewGuid().ToString())
                .RuleFor(a => a.Line1, f => f.Address.StreetAddress())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.State, f => f.Address.State())
                .RuleFor(a => a.ZIPCode, f => f.Address.ZipCode());

            var addresses = new List<Address>();
            foreach (Customer c in customers)
            {
                var numAddressesForCustomer = rnd.Next(1, 5);
                var customerAddresses = addressFaker.Generate(numAddressesForCustomer);
                foreach(Address a in customerAddresses)
                {
                    a.CustomerID = c.ID;
                    addresses.Add(a);
                }
            }
            ctx.Addresses.AddRange(addresses);
            ctx.SaveChangesAsync();
        }
    }
}