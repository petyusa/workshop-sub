namespace Workshop.Api.Models;

public record ReservableObjectListResponse(
    int Id,
    string Name,
    string Type,
    string LocationName,
    bool IsAvailable
);

public record ReservableObjectResponse(
    int Id,
    string Name,
    string Type,
    string LocationName,
    bool IsActive,
    string? Description,
    List<OpeningHoursResponse>? OpeningHours
);

public record OpeningHoursResponse(
    DayOfWeek DayOfWeek,
    string OpenTime,
    string CloseTime
);
