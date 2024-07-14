using FluentValidation;
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
    public static class GetCurrentAccontQuery
    {

        public class Request : IRequest<Result>
        {

        }

        public class Result
        { 
            public int Id { get; set; }
            public EnumAccountType AccountType { get; set; } 
       

        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {

            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAccount();


                return new Result()
                {
                    AccountType = account.AccountType,
                    Id = account.Id,

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
