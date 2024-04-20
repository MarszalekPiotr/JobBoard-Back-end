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
    public static  class SetCurrentAccountCommand
    {

        public class Request : IRequest<Result>
        {

            public Guid AccountId { get; set; }
            
        }

        public class Result
        {
            public Guid AccountId { get; set; }
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
                if(userId != null)
                {
                    if (_currentAccountProvider.AccountBelongsToCurrentUser(request.AccountId))
                    {
                        return new Result() { AccountId = request.AccountId };
                    }
                }
                throw new UnauthorizedException();
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                //RuleFor(x => x.AccountId).NotEmpty();
                //RuleFor(x => x.AccountId).Must(x => x.GetType() == typeof(Guid));
               

            }
        }
    }
}
