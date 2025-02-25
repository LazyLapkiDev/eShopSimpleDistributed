
namespace IdentityUserService.Services
{
    public interface IUserService
    {
        Task ConfirmMailingAsync(Guid id);
    }
}