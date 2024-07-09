using JobBoard.Application.DTO;
using JobBoard.Application.Exceptions;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.WebApi.Application.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Auth
{
    public class CurrentAccountProvider : ICurrentAccountProvider
    {
        private readonly JWTManager _jwtManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IAuthenticationDataProvider _authenticationDataProvider;
        public CurrentAccountProvider(JWTManager jwtManager, IHttpContextAccessor httpContextAccessor, IApplicationDbContext applicationDbContext, IAuthenticationDataProvider authenticationDataProvider)
        {
            _jwtManager = jwtManager;
            _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = applicationDbContext;
            _authenticationDataProvider = authenticationDataProvider;
        }

       public bool AccountBelongsToCurrentUser(int accountId, EnumAccountType accountType)
        {
            var currentUserId = _authenticationDataProvider.GetUserId();

            if(accountType == EnumAccountType.CompanyAccount)
            {

                if (_applicationDbContext.companyAccountUsers.FirstOrDefault(cas => cas.CompanyAccountId == accountId)?.UserId == currentUserId)
                {
                    return true;
                }

            }
            else if(accountType == EnumAccountType.CandidateAccount)
            {
                if (_applicationDbContext.CandidateAccounts.FirstOrDefault(cas => cas.Id == accountId)?.UserId == currentUserId)
                {
                    return true;
                }
            }

            return false;


            
        }
        public async Task<Account> GetAccount()
        {
            var accountIdCookieValue = _httpContextAccessor.HttpContext?.Request.Cookies[CookieSettings.AccountIdCookieName];
            var accountTypeCookieValue = _httpContextAccessor.HttpContext?.Request.Cookies[CookieSettings.AccountTypeCookieName];
            // check if account belongs to the current user
            if (int.TryParse(accountIdCookieValue,out int accountId) && Enum.TryParse(accountTypeCookieValue,out EnumAccountType accountType))
            {
                if (AccountBelongsToCurrentUser(accountId,accountType))
                {
                    return new Account() { Id = accountId, AccountType = accountType };
                }

            }
            throw new UnauthorizedException();
        }

        public async  Task<CandidateAcccount> GetCurrentCandidateAccount()
        {
            var account = await this.GetAccount();
            if(account.AccountType != EnumAccountType.CandidateAccount) { throw new UnauthorizedException(); }
            return await _applicationDbContext.CandidateAccounts.FirstOrDefaultAsync(cas => cas.Id == account.Id);
           
            
        }

        public async  Task<CompanyAccount> GetCurrentCompanyAccount()
        {
            var account = await this.GetAccount();
            if (account.AccountType != EnumAccountType.CompanyAccount) { throw new UnauthorizedException(); }
            return await _applicationDbContext.companyAccounts.FirstOrDefaultAsync(cas => cas.Id == account.Id);
        }
    }
}
