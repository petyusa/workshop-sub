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

            // Check if object exists and load opening hours
            var reservableObject = await db.ReservableObjects
                .Include(o => o.OpeningHours)
                .FirstOrDefaultAsync(o => o.Id == request.ReservableObjectId && o.IsActive);

            if (reservableObject == null)
            {
                return Results.NotFound("Reservable object not found or inactive");
            }

            // Validate against opening hours if they exist
            if (reservableObject.OpeningHours.Any())
            {
                var startDate = request.StartDateTime.Date;
                var endDate = request.EndDateTime.Date;
                var currentDate = startDate;

                while (currentDate <= endDate)
                {
                    var dayOfWeek = currentDate.DayOfWeek;
                    var openingHoursForDay = reservableObject.OpeningHours
                        .Where(oh => oh.DayOfWeek == dayOfWeek)
                        .ToList();

                    if (!openingHoursForDay.Any())
                    {
                        return Results.BadRequest($"Object is not available on {dayOfWeek}s");
                    }

                    // Check if the reservation times fall within opening hours
                    var startTime = currentDate == startDate ? request.StartDateTime.TimeOfDay : TimeSpan.Zero;
                    var endTime = currentDate == endDate ? request.EndDateTime.TimeOfDay : new TimeSpan(23, 59, 59);

                    var isWithinHours = openingHoursForDay.Any(oh => 
                        startTime >= oh.OpenTime && endTime <= oh.CloseTime);

                    if (!isWithinHours)
                    {
                        var hours = openingHoursForDay.First();
                        return Results.BadRequest(
                            $"Reservation time must be within opening hours ({hours.OpenTime:hh\\:mm} - {hours.CloseTime:hh\\:mm}) on {dayOfWeek}s");
                    }

                    currentDate = currentDate.AddDays(1);
                }
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
