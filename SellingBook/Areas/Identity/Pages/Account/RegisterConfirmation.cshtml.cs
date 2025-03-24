using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SellingBook.Areas.Identity.Pages.Account
{
    public class RegisterConfirmationModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string email, string returnUrl = null)
        {
            Email = email;
            ReturnUrl = returnUrl;
        }
    }
}
