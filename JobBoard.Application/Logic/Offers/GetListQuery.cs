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
using static JobBoard.Application.Logic.Offers.GetListQuery.Result;

namespace JobBoard.Application.Logic.Offers
{
    public static class GetListQuery
    {
        public class Request : IRequest<Result>
        {
            public Guid? CompanyId { get; set; }
            public string? SearchName { get; set; }
            public int? CityId { get; set; }
            public int? MinSalary { get; set; }
            public int? MaxSalary { get; set; }
            public EnumWorkMode? WorkingMode { get; set; }
            public EnumContractType? ContractType { get; set; }
            public int? CategoryId { get; set; }
            public List<int>? TagIds { get; set; }
        }

        public class Result
        {
            public List<Offer> OfferListDTO { get; set; } = new List<Offer>();

            public class Offer
            {
                public required string Name { get; set; }
                public City? CityDTO { get; set; }
                public string Location { get; set; } = string.Empty;
                public required int MinSalary { get; set; }
                public required int MaxSalary { get; set; }
                public required EnumWorkMode WorkingMode { get; set; }
                public required EnumContractType ContractType { get; set; }
                public required string CompanyName { get; set; }
                public required string CategoryName { get; set; }
                public List<TagDTO> Tags { get; set; } = new List<TagDTO>();

                public class City
                {
                    public required string Name { get; set; } = string.Empty;
                    public int Id { get; set; }
                }
            }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IAuthenticationDataProvider authenticationDataProvider)
                : base(currentAccountProvider, applicationDbContext)
            {
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var offers = _applicationDbContext.Offers.AsQueryable();

                if (request.CategoryId != null)
                {
                    offers = offers.Where(o => o.CategoryId == request.CategoryId.Value);
                }

                if (request.CityId != null)
                {
                    offers = offers.Where(o => o.CityId == request.CityId.Value);
                }

                if (request.MinSalary.HasValue)
                {
                    offers = offers.Where(o => o.MinSalary <= request.MinSalary);
                }

                if (request.MaxSalary.HasValue)
                {
                    offers = offers.Where(o => o.MaxSalary >= request.MaxSalary);
                }

                if (request.WorkingMode.HasValue)
                {
                    offers = offers.Where(o => o.WorkingMode == request.WorkingMode);
                }

                if (request.ContractType.HasValue)
                {
                    offers = offers.Where(o => o.ContractType == request.ContractType);
                }

                if (request.CategoryId.HasValue)
                {
                    offers = offers.Where(o => o.CategoryId == request.CategoryId);
                }

                if (request.CompanyId.HasValue)
                {
                    offers = offers.Where(o => o.CompanyAccountId == request.CompanyId);
                }

                if (request.TagIds?.Count > 0)
                {
                    offers = offers.Where(o => o.OfferTags.All(oft => request.TagIds.Contains(oft.TagId)));
                }

                var offerListDto = offers.Select(offer => new Result.Offer
                {
                    Name = offer.Name,
                    CityDTO = GetCity(offer.CityId,_applicationDbContext),
                    Location = offer.Address,
                    MinSalary = offer.MinSalary,
                    MaxSalary = offer.MaxSalary,
                    WorkingMode = offer.WorkingMode,
                    ContractType = offer.ContractType,
                    // tu ewentualnie osobne metody i zwracanie pustego strninga
                    CompanyName = _applicationDbContext.companyAccounts.FirstOrDefault(ca => ca.Id == offer.CompanyAccountId).Name ?? string.Empty,
                    CategoryName = _applicationDbContext.Categories.FirstOrDefault(c => c.Id == offer.CategoryId).Name,
                    Tags = GetTagsForOffer(offer.Id,_applicationDbContext)
                }).ToList();

                return new Result { OfferListDTO = offerListDto };
            }

            private static Result.Offer.City GetCity(int? cityId, IApplicationDbContext applicationDbContext)
            {
                if (!cityId.HasValue) return null;

                var city = applicationDbContext.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null) return null;

                return new Result.Offer.City { Name = city.Name, Id = city.Id };
            }

            private static List<TagDTO> GetTagsForOffer(int offerId, IApplicationDbContext applicationDbContext)
            
              {   // to delete 
                var category = applicationDbContext.Categories.ToList();
                var category2 = applicationDbContext.Categories.FirstOrDefault(c => c.Id == applicationDbContext.Offers.FirstOrDefault(o => o.Id == offerId).CategoryId);

                var offerTags = applicationDbContext.OfferTags.ToList();

                var tags = applicationDbContext.OfferTags
                    .Join(applicationDbContext.Tags, offerTag => offerTag.TagId, tag => tag.Id, (offerTag, tag) => new { offerTag, tag })
                    .Where(joined => joined.offerTag.OfferId == offerId)
                    .Select(joined => new TagDTO { Name = joined.tag.Name, IconPath = "default" })
                    .ToList();

                return tags;
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
