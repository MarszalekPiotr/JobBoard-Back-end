using FluentValidation;
using JobBoard.Application.Exceptions;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Logic.Abstractions;
using JobBoard.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Logic.Users
{
    public static  class CurrentCandidateAccountQuery
    {
        public class Request : IRequest<Result>
        {

        }

        public class Result
        {
           public Guid AccountId { get; set; }
            public string Name { get; set; }
           public required string SurName { get; set; }
           public required DateTimeOffset BirthDate { get; set; }
           public string PhoneNumber { get; set; } = string.Empty;
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext ) : base(currentAccountProvider, applicationDbContext)
            {
              
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetCurrentCandidateAccount();
                if (account != null)
                {
                    return new Result()
                    {
                        AccountId = account.Id,
                        Name = account.Name,
                        SurName = account.SurName,
                        BirthDate = account.BirthDate,
                        PhoneNumber = account.PhoneNumber
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
