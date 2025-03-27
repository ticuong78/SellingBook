using System.Globalization;

namespace SellingBook.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userCulture = context.Session.GetString("UserCulture");
            if (!string.IsNullOrEmpty(userCulture))
            {
                var cultureInfo = new CultureInfo(userCulture);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }

            await _next(context);
        }
    }
}
