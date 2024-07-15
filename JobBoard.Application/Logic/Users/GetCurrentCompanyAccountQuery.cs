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

namespace JobBoard.Application.Logic.Users
{
    public static  class GetCurrentCompanyAccountQuery
    {

        public class Request : IRequest<Result>
        {

        }

        public class Result
        {
            public int AccountId { get; set; }
            public EnumAccountType AccountType { get => EnumAccountType.CompanyAccount; }
            public string Name { get; set; }
            public string Description { get; set; } = string.Empty;
            public string ContactEmail { get; set; } = string.Empty;
           

        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {

            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetCurrentCompanyAccount();
                
               
                    return new Result()
                    {
                        AccountId = account.Id,
                        Name = account.Name,
                        Description = account.Description,
                       
                        ContactEmail = account.ContactEmail
                       
                    };
     
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
