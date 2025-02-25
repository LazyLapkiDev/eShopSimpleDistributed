namespace IdentityUserService.Data.Entities;

public class UserSettings
{
    public long Id { get; set; }
    public string Culture { get; set; } = "ru-RU";
    public bool IsNotificationEnabled { get; set; } = true;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
