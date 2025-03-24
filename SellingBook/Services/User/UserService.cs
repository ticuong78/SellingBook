using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using SellingBook.Models.Identity;
using SellingBook.Services.User;
using System.Net.WebSockets;
using System.Security.Claims;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string GetCurrentUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
            return null;

        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<ApplicationUser> FindUserByEmailAsync(string email)
    {
        if(email == null)
        {
            Console.WriteLine("Fuck off");
            return null;
        }

        return await _userManager.FindByEmailAsync(email);
    }

    public async Task UpdateUserAsync(ApplicationUser user)
    {
        await _userManager.UpdateAsync(user);
    }
}
