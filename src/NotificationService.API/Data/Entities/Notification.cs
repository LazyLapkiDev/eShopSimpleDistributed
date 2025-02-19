namespace NotificationService.API.Data.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public required string Subject { get; set; }
    public string? Body { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

}