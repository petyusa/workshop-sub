using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data;
using Workshop.Api.Data.Entities;
using Workshop.Api.Extensions;
using Workshop.Api.Models;

namespace Workshop.Api.Extensions;

public static class ReservationEndpoints
{
    public static void MapReservationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reservations");

        group.MapPost("/", async (
            CreateReservationRequest request,
            HttpContext context,
            WorkshopDbContext db) =>
        {
            var userId = context.GetCurrentUsername() ?? "anonymous";

            // Simple validation
            if (request.StartDateTime >= request.EndDateTime)
            {
                return Results.BadRequest("End time must be after start time");
            }

            if (request.StartDateTime < DateTime.UtcNow)
            {
                return Results.BadRequest("Cannot create reservation in the past");
            }

            // Check if object exists
            var objectExists = await db.ReservableObjects
                .AnyAsync(o => o.Id == request.ReservableObjectId && o.IsActive);

            if (!objectExists)
            {
                return Results.NotFound("Reservable object not found or inactive");
            }

            var reservation = new Reservation
            {
                ReservableObjectId = request.ReservableObjectId,
                UserId = userId,
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                CreatedAt = DateTime.UtcNow
            };

            db.Reservations.Add(reservation);
            await db.SaveChangesAsync();

            var response = await db.Reservations
                .Where(r => r.Id == reservation.Id)
                .Select(r => new ReservationResponse(
                    r.Id,
                    r.ReservableObjectId,
                    r.ReservableObject.Name,
                    r.ReservableObject.Location.Name,
                    r.StartDateTime,
                    r.EndDateTime,
                    r.CreatedAt))
                .FirstAsync();

            return Results.Ok(response);
        })
        .WithName("CreateReservation")
        .WithOpenApi();

        group.MapGet("/my", async (
            HttpContext context,
            WorkshopDbContext db) =>
        {
            var userId = context.GetCurrentUsername() ?? "anonymous";

            var reservations = await db.Reservations
                .Where(r => r.UserId == userId && r.EndDateTime >= DateTime.UtcNow)
                .OrderBy(r => r.StartDateTime)
                .Select(r => new ReservationResponse(
                    r.Id,
                    r.ReservableObjectId,
                    r.ReservableObject.Name,
                    r.ReservableObject.Location.Name,
                    r.StartDateTime,
                    r.EndDateTime,
                    r.CreatedAt))
                .ToListAsync();

            return Results.Ok(reservations);
        })
        .WithName("GetMyReservations")
        .WithOpenApi();
    }
}
