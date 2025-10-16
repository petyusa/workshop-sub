using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data.Entities;

namespace Workshop.Api.Data;

public class WorkshopDbContext : DbContext
{
    public WorkshopDbContext(DbContextOptions<WorkshopDbContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<ReservableObject> ReservableObjects => Set<ReservableObject>();

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

        // Configure ReservableObject entity
        modelBuilder.Entity<ReservableObject>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).IsRequired();
            
            entity.HasOne(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed reservable objects
        modelBuilder.Entity<ReservableObject>().HasData(
            // Downtown Location - Desks
            new ReservableObject { Id = 1, Name = "Desk A-101", Type = "Desk", LocationId = 1, IsActive = true, Description = "Window seat with natural light" },
            new ReservableObject { Id = 2, Name = "Desk A-102", Type = "Desk", LocationId = 1, IsActive = true, Description = "Standing desk available" },
            new ReservableObject { Id = 3, Name = "Desk A-103", Type = "Desk", LocationId = 1, IsActive = true, Description = "Quiet corner desk" },
            new ReservableObject { Id = 4, Name = "Desk A-104", Type = "Desk", LocationId = 1, IsActive = false, Description = "Under maintenance" },
            new ReservableObject { Id = 5, Name = "Desk A-105", Type = "Desk", LocationId = 1, IsActive = true, Description = "Near collaboration area" },
            
            // Downtown Location - Parking
            new ReservableObject { Id = 6, Name = "Parking DT-01", Type = "ParkingSpace", LocationId = 1, IsActive = true, Description = "Covered parking" },
            new ReservableObject { Id = 7, Name = "Parking DT-02", Type = "ParkingSpace", LocationId = 1, IsActive = true, Description = "Ground level" },
            new ReservableObject { Id = 8, Name = "Parking DT-03", Type = "ParkingSpace", LocationId = 1, IsActive = true, Description = "EV charging available" },
            
            // North Branch - Desks
            new ReservableObject { Id = 9, Name = "Desk B-201", Type = "Desk", LocationId = 2, IsActive = true, Description = "Ergonomic chair included" },
            new ReservableObject { Id = 10, Name = "Desk B-202", Type = "Desk", LocationId = 2, IsActive = true, Description = "Dual monitor setup" },
            new ReservableObject { Id = 11, Name = "Desk B-203", Type = "Desk", LocationId = 2, IsActive = true, Description = "Private booth desk" },
            new ReservableObject { Id = 12, Name = "Desk B-204", Type = "Desk", LocationId = 2, IsActive = true, Description = "Open space desk" },
            
            // North Branch - Parking
            new ReservableObject { Id = 13, Name = "Parking NB-01", Type = "ParkingSpace", LocationId = 2, IsActive = true, Description = "Outdoor parking" },
            new ReservableObject { Id = 14, Name = "Parking NB-02", Type = "ParkingSpace", LocationId = 2, IsActive = true, Description = "Covered parking" },
            new ReservableObject { Id = 15, Name = "Parking NB-03", Type = "ParkingSpace", LocationId = 2, IsActive = false, Description = "Reserved for maintenance vehicle" },
            
            // East Side Location - Desks
            new ReservableObject { Id = 16, Name = "Desk C-301", Type = "Desk", LocationId = 3, IsActive = true, Description = "Corner office desk" },
            new ReservableObject { Id = 17, Name = "Desk C-302", Type = "Desk", LocationId = 3, IsActive = true, Description = "Collaborative workspace" },
            new ReservableObject { Id = 18, Name = "Desk C-303", Type = "Desk", LocationId = 3, IsActive = true, Description = "Focus room desk" },
            
            // East Side Location - Parking & Meeting Room
            new ReservableObject { Id = 19, Name = "Parking ES-01", Type = "ParkingSpace", LocationId = 3, IsActive = true, Description = "Underground parking" },
            new ReservableObject { Id = 20, Name = "Meeting Room Alpha", Type = "MeetingRoom", LocationId = 3, IsActive = true, Description = "8-person capacity, video conferencing" },
            new ReservableObject { Id = 21, Name = "Meeting Room Beta", Type = "MeetingRoom", LocationId = 3, IsActive = true, Description = "4-person capacity, whiteboard" }
        );
    }
}
