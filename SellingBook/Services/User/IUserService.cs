using SellingBook.Models.Identity;

namespace SellingBook.Services.User
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        string GetCurrentUserId();
        string GetCurrentUserEmail();
    }
}
