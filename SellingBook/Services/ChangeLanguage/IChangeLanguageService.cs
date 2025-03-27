namespace SellingBook.Services.ChangeLanguage
{
    public interface IChangeLanguageService
    {
        void SetLanguage(HttpContext response, string culture);
    }
}
