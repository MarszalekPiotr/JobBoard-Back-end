using FluentValidation;
using JobBoard.Application.Exceptions;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Logic.Abstractions;
using JobBoard.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Logic.Users
{
    public static class LoginCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Email { get; set; }
            public string Password { get; set; }
        }

        public class Result
        {
            public required int UserId { get; set; }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            private IPasswordManager _passwordManager;
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IPasswordManager passwordManager) : base(currentAccountProvider, applicationDbContext)
            {
                _passwordManager = passwordManager;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {

                var user = _applicationDbContext.Users.FirstOrDefault(u => u.Email == request.Email);
                if (user != null)
                {
                    var correctPassword = user.HashedPassword;
                    if (_passwordManager.VerifyPassword(correctPassword, request.Password))
                    {
                        return new Result() { UserId = user.Id };
                    }
                }

                throw new Exception("Invalid login or password");
                

            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                // Password Validation Confiuration
                RuleFor(x => x.Password).NotEmpty();
                
                // Email Validation Configuration
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Email).NotEmpty();

            }
        }
    }
}
