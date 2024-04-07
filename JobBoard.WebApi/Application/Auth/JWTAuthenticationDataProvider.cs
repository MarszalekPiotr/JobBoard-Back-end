using JobBoard.Application.Interfaces;
using JobBoard.Infrastructure.Auth;

namespace JobBoard.WebApi.Application.Auth
{
    public class JWTAuthenticationDataProvider : IAuthenticationDataProvider
    {
        private readonly JWTManager _jwtManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JWTAuthenticationDataProvider(JWTManager jwtManager, IHttpContextAccessor httpContextAccessor)
        {
            _jwtManager = jwtManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserId()
        {
            var userId = GetClaimValue(JWTManager.UserIdClaim);
            if(int.TryParse(userId,out int res))
            {
                return res;
            }
            return null;
        }

        
        private string? GetTokenFromCookie()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[CookieSettings.CookieName];
        }
        private string? GetTokenFromHeader()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }

            var splited = authorizationHeader.Split(' ');
            if (splited.Length > 1 && splited[0] == "Bearer")
            {
                return splited[1];
            }

            return null;
        }
        private string? GetClaimValue(string claimType)
        {
            var token = GetTokenFromHeader();
            if (string.IsNullOrEmpty(token))
            {
                token = GetTokenFromCookie();
            }

            if (!string.IsNullOrWhiteSpace(token) && _jwtManager.ValidateToken(token))
            {
                return _jwtManager.GetClaim(token, claimType);
            }

            return null;
        }
    }
}
