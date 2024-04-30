using FluentValidation;
using JobBoard.Application.Exceptions;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Logic.Abstractions;
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
            public Guid AccountId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; } = string.Empty;
            public string ContactEmail { get; set; } = string.Empty;
            public required string NIP { get; set; }

        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {

            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetCurrentCompanyAccount();
                if (account != null)
                {
                    return new Result()
                    {
                        AccountId = account.Id,
                        Name = account.Name,
                        Description = account.Description,
                        NIP = account.NIP,
                        ContactEmail = account.ContactEmail
                       
                    };
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
