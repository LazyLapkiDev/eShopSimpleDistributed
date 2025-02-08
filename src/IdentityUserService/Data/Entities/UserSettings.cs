using System.Numerics;

namespace UserService.Data.Entities;

public class UserSettings
{
    public BigInteger Id { get; set; }
    public string Culture { get; set; } = "ru-RU";
    public bool IsNotificationEnabled { get; set; } = true;
}
