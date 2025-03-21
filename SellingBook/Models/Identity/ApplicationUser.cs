using Microsoft.AspNetCore.Identity;

namespace SellingBook.Models.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }
    }
}
