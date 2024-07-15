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
    public static class CreateCandidateAccountCommand
    {
        public class Request : IRequest<Result>
        {
          
            // czy to required tu powinno byc w requescie czy tylko dac rule for validation?
            public required string Name { get; set; }
            public string Description { get; set; } 
            public string ContactEmail { get; set; } 
            public string SurName { get; set; }
            public DateTimeOffset BirthDate { get; set; }
            public string PhoneNumber { get; set; } 
        }

        public class Result
        {
          public int AccountId { get; set; } 
          public EnumAccountType AccountType { get => EnumAccountType.CandidateAccount; }
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
                // tu raczej przniesc do authentication data provider ze jak null to exception?
                var currentUserId = _authenticationDataProvider.GetUserId();
                if(currentUserId != null)
                {

                    if (_applicationDbContext.CandidateAccounts.FirstOrDefault(ca => ca.UserId == currentUserId) != null) { throw new ErrorException("This user has already created candidate account"); }

                    var candidateAccount = new CandidateAcccount()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        ContactEmail = request.ContactEmail,
                        SurName = request.SurName,
                        BirthDate = request.BirthDate,
                        PhoneNumber = request.PhoneNumber,
                        CreationDate = DateTimeOffset.UtcNow,
                        UserId = (int)currentUserId
                    };

                    
                    var addedAccount  = await _applicationDbContext.CandidateAccounts.AddAsync(candidateAccount);
                
                    await _applicationDbContext.SaveChangesAsync();
                    return new Result() { AccountId = addedAccount.Entity.Id };

                }
                throw new UnauthorizedException();
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.ContactEmail).EmailAddress();
                RuleFor(x => x.SurName).NotEmpty();
                RuleFor(x => x.PhoneNumber).NotNull();
                RuleFor(x => x.PhoneNumber).MinimumLength(9);
                RuleFor(x => x.BirthDate).NotNull();
                RuleFor(x => x.BirthDate).Must( x => x.GetType() == typeof(DateTimeOffset));
                
            }
        }
    }
}
