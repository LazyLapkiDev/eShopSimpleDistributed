﻿using SimpleRabbitEventBus.Abstractions;

namespace IdentityUserService.IntegrationEvents;

public class UserCreatedEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Culture { get; set; }
    public required bool IsNotificationEnabled { get; set; }
}
