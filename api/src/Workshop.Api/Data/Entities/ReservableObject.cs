namespace Workshop.Api.Data.Entities;

public class ReservableObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }

    // Navigation properties
    public Location Location { get; set; } = null!;
}
