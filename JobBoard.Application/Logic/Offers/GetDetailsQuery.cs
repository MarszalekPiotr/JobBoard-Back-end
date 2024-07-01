using FluentValidation;
using JobBoard.Application.DTO;
using JobBoard.Application.Exceptions;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Logic.Abstractions;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.Domain.FormDefinitionSchema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Logic.Offers
{
    public static  class GetDetailsQuery
    {
        public class Request : IRequest<Result>
        {
            public int Id { get; set; }
        }

        public class Result
        {
            public required string Name { get; set; }
            public string Description { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public required int MinSalary { get; set; }
            public required int MaxSalary { get; set; }
            public required EnumWorkMode WorkingMode { get; set; }
            public required EnumContractType ContractType { get; set; }

            public FormDefinition? FormDefinitionJSON { get; set; }
            
            public required string CompanyName { get; set; } 

            public  required string CategoryName { get; set; } 

            public List<TagDTO> Tags { get; set; } = new List<TagDTO>();

        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IAuthenticationDataProvider authenticationDataProvider) : base(currentAccountProvider, applicationDbContext)
            {
               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var offer = _applicationDbContext.Offers.FirstOrDefault(o => o.Id == request.Id) ?? throw new ErrorException("Offer not found");


                //      public required string Name { get; set; }
                //public string Description { get; set; } = string.Empty;
                //public string City { get; set; } = string.Empty;
                //public string Location { get; set; } = string.Empty;
                //public required int MinSalary { get; set; }
                //public required int MaxSalary { get; set; }
                //public required EnumWorkMode WorkingMode { get; set; }
                //public required EnumContractType ContractType { get; set; }

                //public FormDefinition? FormDefinitionJSON { get; set; }

                var tagIds = _applicationDbContext.OfferTags.Where(oft => oft.OfferId == offer.Id).Select(oft => oft.TagId);

                return new Result()
                {
                    Name = offer.Name,
                    Description = offer.Description,
                    City = offer.City,
                    Location = offer.Location,
                    MinSalary = offer.MinSalary,
                    MaxSalary = offer.MaxSalary,
                    WorkingMode = offer.WorkingMode,
                    ContractType = offer.ContractType,
                    FormDefinitionJSON = offer.FormDefinitionJSON,
                    CompanyName = _applicationDbContext.companyAccounts.FirstOrDefault(ca => ca.Id == offer.CompanyAccountId)?.Name ?? string.Empty,
                    CategoryName = _applicationDbContext.Categories.FirstOrDefault(c => c.Id == offer.CategoryId)?.Name ?? string.Empty,
                    // Tags = _applicationDbContext.Tags.Where(t => tagIds.Contains(t.Id)).Select(t => new TagDTO() {Name = t.Name, IconPath = "default"}).ToList()
                    Tags = _applicationDbContext.Tags.Join(_applicationDbContext.OfferTags
                   , tag => tag.Id,
                   offerTag => offerTag.TagId,
                   (tag, offerTag) => new { tag, offerTag })
                    .Where(joined => joined.offerTag.OfferId == offer.Id)
                    .Select(joined => new TagDTO() { Name = joined.tag.Name, IconPath = "default" }).ToList()
                 
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
