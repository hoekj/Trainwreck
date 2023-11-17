using Microsoft.EntityFrameworkCore;

namespace Trainwreck.Entities;

public class AppDbContext : DbContext
{
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Train> Trains { get; set; }
}
