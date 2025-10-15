using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data;
using Workshop.Api.Models;

namespace Workshop.Api.Extensions;

public static class LocationEndpoints
{
    public static void MapLocationEndpoints(this WebApplication app)
    {
        app.MapGet("/api/locations", async (WorkshopDbContext db) =>
        {
            var locations = await db.Locations
                .Where(l => l.IsActive)
                .OrderBy(l => l.Name)
                .Select(l => new LocationResponse(l.Id, l.Name, l.Address))
                .ToListAsync();

            return Results.Ok(locations);
        })
        .WithName("GetLocations")
        .WithOpenApi()
        .WithDescription("Returns all active locations");
    }
}
