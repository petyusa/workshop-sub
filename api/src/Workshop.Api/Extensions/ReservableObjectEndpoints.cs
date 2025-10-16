using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data;
using Workshop.Api.Models;

namespace Workshop.Api.Extensions;

public static class ReservableObjectEndpoints
{
    public static void MapReservableObjectEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/reservable-objects")
            .WithTags("Reservable Objects");

        group.MapGet("/", async (
            WorkshopDbContext db,
            int? locationId,
            string? type,
            bool activeOnly = true) =>
        {
            var query = db.ReservableObjects
                .Include(r => r.Location)
                .AsQueryable();

            if (locationId.HasValue)
            {
                query = query.Where(r => r.LocationId == locationId.Value);
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(r => r.Type == type);
            }

            if (activeOnly)
            {
                query = query.Where(r => r.IsActive);
            }

            var objects = await query
                .Select(r => new ReservableObjectListResponse(
                    r.Id,
                    r.Name,
                    r.Type,
                    r.Location.Name,
                    r.IsActive // For POC, IsAvailable = IsActive (no reservation check yet)
                ))
                .ToListAsync();

            return Results.Ok(objects);
        })
        .WithName("GetReservableObjects")
        .WithOpenApi()
        .WithDescription("Get a list of reservable objects with optional filters");

        group.MapGet("/{id}", async (int id, WorkshopDbContext db) =>
        {
            var reservableObject = await db.ReservableObjects
                .Include(r => r.Location)
                .Include(r => r.OpeningHours)
                .Where(r => r.Id == id)
                .Select(r => new ReservableObjectResponse(
                    r.Id,
                    r.Name,
                    r.Type,
                    r.Location.Name,
                    r.IsActive,
                    r.Description,
                    r.OpeningHours.Select(oh => new OpeningHoursResponse(
                        oh.DayOfWeek,
                        oh.OpenTime.ToString(@"hh\:mm"),
                        oh.CloseTime.ToString(@"hh\:mm")
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            return reservableObject is not null
                ? Results.Ok(reservableObject)
                : Results.NotFound();
        })
        .WithName("GetReservableObjectById")
        .WithOpenApi()
        .WithDescription("Get a specific reservable object by ID");
    }
}
