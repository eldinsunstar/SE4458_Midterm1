# SE4458_Midterm1

Project link : https://github.com/eldinsunstar/SE4458_Midterm1

Database Model is below

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<House> Houses { get; set; }
    public DbSet<BookingDate> BookingDates { get; set; }
    public DbSet<BookingRequest> BookingRequests { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<House>()
            .HasMany(h => h.BookedDates)
            .WithOne(d => d.House)
            .HasForeignKey(d => d.HouseId);

        modelBuilder.Entity<BookingRequest>()
            .HasOne(r => r.House)
            .WithMany(h => h.BookingRequest)
            .HasForeignKey(r => r.HouseId);

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "user1", Password = "password1" },
            new User { Id = 2, Username = "user2", Password = "password2" },
            new User { Id = 3, Username = "user3", Password = "password3" });
    }

    public class DbContextOptions<T>
    {
    }

    public class DbSet<T>
    {
        internal IEnumerable<BookingRequest> ToList()
        {
            throw new NotImplementedException();
        }

        internal Task<List<House>> ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
