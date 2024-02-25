using Flights_Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Flights_Application.Data
{
    public class Entities : DbContext 
    {
      

        public DbSet<Passenger> Passengers => Set<Passenger>();

        public DbSet<Flight> Flights=> Set<Flight>();
        public Entities(DbContextOptions options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Passenger>().HasKey(p => p.Email);
            // We are accessing the remaining number of seats property from the flight entity and setting it as concurrency token
            // because it is the deciding factor as to whether the flight gets booked for a particular user or not.
            modelBuilder.Entity<Flight>().Property(p => p.RemainingNumberOfSeats).IsConcurrencyToken();
            modelBuilder.Entity<Flight>().OwnsOne(f => f.Departure);
            modelBuilder.Entity<Flight>().OwnsOne(f => f.Arrival);
            modelBuilder.Entity<Flight>().OwnsMany(f => f.Bookings);
        }
    }
}
