using Microsoft.AspNetCore.Mvc;
using SellingBook.Models.Identity;
using SellingBook.Services.User;
using SellingBook.Services.Email;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SellingBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailService;

        public AccountController(IUserService userService, IEmailSender emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        // Gửi lại email xác nhận
        public async Task<IActionResult> ResendConfirmation()
        {
            // Get the current user from the UserService (instead of manually getting email)
            var user = await _userService.GetCurrentUserAsync();

            if (user == null) return NotFound("Không tìm thấy tài khoản.");
            if (user.EmailConfirmed) return RedirectToAction("Login");

            string confirmationLink = Url.Action("ConfirmEmail", "Account", new { email = user.Email }, Request.Scheme);
            string emailContent = $"<p>Vui lòng xác nhận email bằng cách nhấn vào <a href='{confirmationLink}'>đây</a>.</p>";
            await _emailService.SendEmailAsync(user.Email, "Xác nhận tài khoản", emailContent);

            ViewBag.Email = user.Email;
            return View("WaitingForEmailConfirmation");
        }

        // Xử lý xác nhận email
        public async Task<IActionResult> ConfirmEmail(string email, string returnUrl = null)
        {
            // Find user by email using UserManager through UserService (optional: inject UserManager or add method in UserService)
            var user = await _userService.FindUserByEmailAsync(email);
            if (user == null) return NotFound("Không tìm thấy tài khoản.");

            // Manually confirm email (optional: add ConfirmEmailAsync in UserService)
            user.EmailConfirmed = true;
            await _userService.UpdateUserAsync(user);

            return View("ConfirmEmailSuccess");
        }

        public IActionResult ConfirmEmailSuccess()
        {
            return View();
        }
    }
}
