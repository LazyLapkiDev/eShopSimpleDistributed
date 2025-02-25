using SimpleRabbitEventBus.Abstractions;

namespace NotificationService.API.IntegrationEvents.Events;

public record NotificationUserCreatedConfirmEvent(Guid UserId, bool Success) : IntegrationEvent;