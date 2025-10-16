namespace Workshop.Api.Models;

public record CreateReservationRequest(
    int ReservableObjectId,
    DateTime StartDateTime,
    DateTime EndDateTime);

public record ReservationResponse(
    int Id,
    int ReservableObjectId,
    string ObjectName,
    string LocationName,
    DateTime StartDateTime,
    DateTime EndDateTime,
    DateTime CreatedAt);
