namespace Workshop.Api.Data.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int ReservableObjectId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ReservableObject ReservableObject { get; set; } = null!;
}
