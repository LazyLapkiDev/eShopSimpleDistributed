
namespace NotificationService.API.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(Guid id, string email, bool isNotificationEnabled);
        Task UpdateUserAsync(Guid id, string email, bool isNotificationEnabled);
    }
}