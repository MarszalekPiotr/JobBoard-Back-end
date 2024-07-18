using FluentValidation;
using JobBoard.Application.Exceptions;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Logic.Abstractions;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Logic.Users
{
    public static  class CreateCompanyAccountComand
    {

        public class Request : IRequest<Result>
        {

            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string ContactEmail { get; set; } = string.Empty;
            
        }

        public class Result
        {
            public int AccountId { get; set; }
            
            public EnumAccountType AccountType { get => EnumAccountType.CompanyAccount; } 
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            IAuthenticationDataProvider _authenticationDataProvider { get; set; }
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IAuthenticationDataProvider authenticationDataProvider) : base(currentAccountProvider, applicationDbContext)
            {
                _authenticationDataProvider = authenticationDataProvider;
            }


            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var userId = _authenticationDataProvider.GetUserId();
                if(userId != null && _applicationDbContext.Users.Any(x => x.Id == userId))
                {
                    var newCompanyAccount = new CompanyAccount()
                    {
                        CreationDate = DateTime.UtcNow,
                        ContactEmail = request.ContactEmail,
                       
                        Name = request.Name,
                        Description = request.Description

                    };

                    var addedCompanyAccount = await _applicationDbContext.companyAccounts.AddAsync(newCompanyAccount);
                    await _applicationDbContext.SaveChangesAsync();
                    await _applicationDbContext.companyAccountUsers.AddAsync(new CompanyAccountUser() { CompanyAccountId = addedCompanyAccount.Entity.Id, UserId = (int)userId });
                    await _applicationDbContext.SaveChangesAsync();
                    return new Result() { AccountId = addedCompanyAccount.Entity.Id };
                }
                throw new UnauthorizedException();

            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.ContactEmail).NotEmpty();
                RuleFor(x => x.ContactEmail).EmailAddress();
                RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
                

            }
        }

    }
}
