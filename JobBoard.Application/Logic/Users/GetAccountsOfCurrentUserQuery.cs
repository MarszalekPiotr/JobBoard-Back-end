using FluentValidation;
using JobBoard.Application.Exceptions;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Logic.Abstractions;
using JobBoard.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobBoard.Application.Logic.Users.GetAccountsOfCurrentUserQuery;

namespace JobBoard.Application.Logic.Users
{
   public static class GetAccountsOfCurrentUserQuery
    {
        public class Request : IRequest<Result>
        {

        }

        public class AccountResult
        {
            public Guid AccountId { get; set; }
            public EnumAccountType AccountType { get; set; }
            public string AccountName { get; set; }
        }
        public class Result
        {
          public  List<AccountResult> accountResults = new List<AccountResult>();
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            private readonly IAuthenticationDataProvider _authenticationDataProvider;
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IAuthenticationDataProvider authenticationDataProvider) : base(currentAccountProvider, applicationDbContext)
            {
                _authenticationDataProvider = authenticationDataProvider;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var userId = _authenticationDataProvider.GetUserId();
                if(userId != null)
                {
                    var candidateAccounts = _applicationDbContext.CandidateAccounts
                        .Where(ca => ca.UserId  == userId)?
                        .Select(ca => new AccountResult() { AccountId = ca.Id, AccountType = EnumAccountType.CandidateAccount, AccountName = ca.Name + " " + ca.SurName }
                   );

                    var companyAccounts = _applicationDbContext.companyAccountUsers.Where(cau => cau.UserId == userId)?
                        .Select(cau => new AccountResult() { AccountId = cau.CompanyAccountId, AccountType = EnumAccountType.CompanyAccount, AccountName = _applicationDbContext.companyAccounts.FirstOrDefault(x => x.Id == cau.CompanyAccountId).Name?? "" });

                    var allAccounts = candidateAccounts?.Union(companyAccounts)?.ToList();
                    if(allAccounts == null)
                    {
                        allAccounts = new List<AccountResult>();
                    }
                    return new Result() { accountResults = allAccounts };
                }
                throw new UnauthorizedException();

              
             
            }

        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {

            }
        }
    }
}
