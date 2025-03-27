using Microsoft.AspNetCore.Localization;

namespace SellingBook.Services.ChangeLanguage
{
    public class ChangeLanguageService : IChangeLanguageService
    {
        public void SetLanguage(HttpContext httpContext, string culture)
        {
            // Store in cookie
            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Store in session
            httpContext.Session.SetString("UserCulture", culture);
        }
    }
}
