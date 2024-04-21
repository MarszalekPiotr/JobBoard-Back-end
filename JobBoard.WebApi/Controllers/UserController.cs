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

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            var result = await  _mediator.Send(new LogoutCommand.Request());
            DeleteTokenFromCookie();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrentUser()
        {
            var result =  await _mediator.Send(new CurrentUserQuery.Request());
            return Ok(result);
        }

        private void DeleteTokenFromCookie()
        {
            Response.Cookies.Delete(CookieSettings.CookieName, new CookieOptions()
            {
                HttpOnly = true
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrentCandidateAccount()
        {
            var result = await _mediator.Send(new CurrentCandidateAccountQuery.Request());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCandidateAccount([FromBody] CreateCandidateAccountCommand.Request model)
        {
            var result = await _mediator.Send(model);
            SetAccountCookie(result.AccountId); 
            return Ok(result);
        }
        private void SetAccountCookie(Guid AccountId)
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

            Response.Cookies.Append(CookieSettings.AccountIdCookieName, AccountId.ToString(), cookieOption);
        }

        [HttpGet]
        public async Task<ActionResult> GetAccountsForCurrentUser()
        {
            var result = await _mediator.Send(new GetAccountsOfCurrentUserQuery.Request());
            return Ok(result.accountResults);
        }

        [HttpPost]
        public async Task<ActionResult> SetCurrentAccount([FromBody] SetCurrentAccountCommand.Request model)
        {
            var result = await _mediator.Send(model);
            // useless if?
            if(result != null)
            {
                SetAccountCookie(model.AccountId);
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> CreateCompanyAccount([FromBody] CreateCompanyAccountComand.Request model)
        {
            var result = await  _mediator.Send(model);
            if(result != null)
            {
                SetAccountCookie(result.AccountId);

            }
            return Ok(result);
        }
    }
}
