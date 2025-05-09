﻿namespace NotificationService.API.Data.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public bool IsNotificationEnabled { get; set; }

    public DateTime CreateAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<Notification> Notifications { get; set; } = [];
}
