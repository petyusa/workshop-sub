using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data.Entities;

namespace Workshop.Api.Data;

public class WorkshopDbContext : DbContext
{
    public WorkshopDbContext(DbContextOptions<WorkshopDbContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Location entity
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.IsActive).IsRequired();
        });

        // Seed initial locations
        modelBuilder.Entity<Location>().HasData(
            new Location 
            { 
                Id = 1, 
                Name = "Downtown Location", 
                Address = "123 Main Street, Downtown",
                IsActive = true 
            },
            new Location 
            { 
                Id = 2, 
                Name = "North Branch", 
                Address = "456 North Avenue",
                IsActive = true 
            },
            new Location 
            { 
                Id = 3, 
                Name = "East Side Location", 
                Address = "789 East Boulevard",
                IsActive = true 
            }
        );
    }
}
