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
    public static  class CurrentUserQuery
    {
        public class Request : IRequest<Result>
        {

        }

        public class Result
        {
            public int UserId { get; set; }
            public string Email { get; set; } = string.Empty;
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
                    var user = _applicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
                    if(user != null)
                    {
                       return new Result()
                        {
                            UserId = user.Id,
                            Email = user.Email
                        };
                    }
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

