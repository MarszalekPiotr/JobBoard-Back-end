namespace JobBoard.WebApi.Application.Auth
{
    public class CookieSettings
    {
        public const string CookieName = "auth.token";

        public const string AccountIdCookieName = "AccountId";
        public const string AccountTypeCookieName = "AccountType";

        public bool Secure { get; set; } = true;

        public SameSiteMode SameSite { get; set; } = SameSiteMode.Lax;
    }
}
