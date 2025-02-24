
namespace NotificationService.API.Services
{
    public interface INotificationService
    {
        Task SendGreetingAsync(Guid userId, string email);
    }
}