using JobBoard.Application.Logic.Users;
using JobBoard.Infrastructure.Auth;
using JobBoard.WebApi.Application.Auth;
using JobBoard.WebApi.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JobBoard.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly JWTManager _jWTManager;
        private readonly IOptions<CookieSettings> _cookieSettings;
        public UserController(ILogger<UserController> logger, IMediator mediator, JWTManager jWTManager, IOptions<CookieSettings> cookieSettings) : base(logger, mediator)
        {
            _jWTManager = jWTManager;
            _cookieSettings = cookieSettings;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand.Request model)
        {
            var createUserResult = await _mediator.Send(model);
            var token = _jWTManager.GenerateUserToken(createUserResult.UserId);
            SetTokenCookie(token);
            return Ok(new JWTResponse() { AuthToken = token}); 
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginCommand.Request model)
        {
            var result = await _mediator.Send(model);
            var token = _jWTManager.GenerateUserToken(result.UserId);
            // save to the cookie
            this.SetTokenCookie(token);
            return Ok(new JWTResponse() { AuthToken = token });


        }
        private void SetTokenCookie(string token)
        {
            var cookieOption = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = SameSiteMode.Lax,
            };

            if (_cookieSettings != null)
            {
                cookieOption = new CookieOptions()
                {
                    HttpOnly = cookieOption.HttpOnly,
                    Expires = cookieOption.Expires,
                    Secure = _cookieSettings.Value.Secure,
                    SameSite = _cookieSettings.Value.SameSite,
                };
            }

            Response.Cookies.Append(CookieSettings.CookieName, token, cookieOption);
        }
    }
}
