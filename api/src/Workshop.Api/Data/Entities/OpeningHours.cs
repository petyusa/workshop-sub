namespace Workshop.Api.Data.Entities;

public class OpeningHours
{
    public int Id { get; set; }
    public int ReservableObjectId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }

    // Navigation properties
    public ReservableObject ReservableObject { get; set; } = null!;
}
