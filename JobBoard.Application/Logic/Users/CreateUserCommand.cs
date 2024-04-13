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
    public static class CreateUserCommand
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
                var userExists = await _applicationDbContext.Users.AnyAsync(user => user.Email == request.Email);
                if (userExists)
                {
                    throw new ErrorException("User With this Email already exists");
                }
                var user = new User() {  Email = request.Email,RegisterDate = DateTime.UtcNow,HashedPassword ="" };
                var hashedPassword = _passwordManager.HashPassword(request.Password);
                user.HashedPassword = hashedPassword;
                await _applicationDbContext.Users.AddAsync(user);
                await _applicationDbContext.SaveChangesAsync();
                var result = new Result() { UserId = user.Id };
                return result;
          
                
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {   
                // Password Validation Confiuration
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Password).MaximumLength(50);
                RuleFor(x => x.Password).MinimumLength(8);

                // Email Validation Configuration
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Email).NotEmpty();
               
            }
        }
    }
}
