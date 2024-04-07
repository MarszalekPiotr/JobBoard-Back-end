using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.WebApi.Application.Auth;
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
        public CurrentAccountProvider(JWTManager jwtManager, IHttpContextAccessor httpContextAccessor, IApplicationDbContext applicationDbContext)
        {
            _jwtManager = jwtManager;
            _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Guid> GetAccountId()
        {
            var accountCookie = _httpContextAccessor.HttpContext?.Request.Cookies[CookieSettings.AccountIdCookieName];
            if (Guid.TryParse(accountCookie, out Guid accountId))
            {
                return  accountId;
            }
            return Guid.Empty;
        }

        public async Task<EnumAccountType> GetAccountType()
        {
            Guid accountId = await this.GetAccountId();
            if(_applicationDbContext.candidateAcccounts.FirstOrDefault(x => x.Id == accountId ) != null) 
            {
                return EnumAccountType.CandidateAccount;
            }
            else if(_applicationDbContext.companyAccounts.FirstOrDefault(x => x.Id == accountId) != null)
            {
                return EnumAccountType.CompanyAccount;
            }
            return EnumAccountType.AccountNotSelected;
        }

        public async  Task<CandidateAcccount> GetCurrentCandidateAccount()
        {    
            var accountId = await this.GetAccountId();
            if(await this.GetAccountType() == EnumAccountType.CandidateAccount)
            {
                return _applicationDbContext.candidateAcccounts.FirstOrDefault(x => x.Id == accountId); 
            }
            return null;
        }

        public async  Task<CompanyAccount> GetCurrentCompanyAccount()
        {
            var accountId = await this.GetAccountId();
            if (await this.GetAccountType() == EnumAccountType.CandidateAccount)
            {
                return _applicationDbContext.companyAccounts.FirstOrDefault(x => x.Id == accountId);
            }
            return null;
        }
    }
}
