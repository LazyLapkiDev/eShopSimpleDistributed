using SimpleRabbitEventBus.Abstractions;

namespace IdentityUserService.IntegrationEvents.Events;

public record NotificationUserCreatedConfirmEvent(Guid UserId, bool Success) : IntegrationEvent;
