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
    public static  class GetListQuery
    {
        public class Request : IRequest<Result>
        {
            public Guid? CompanyId { get; set; }
            public string? SearchName { get; set; }
            public string? City { get; set; } // probably do zmiany tzn te miasta jakos inaczej zrobic
            public int? MinSalary { get; set; }
            public int? MaxSalary { get; set; }
            public EnumWorkMode? WorkingMode { get; set; }
            public EnumContractType? ContractType { get; set; }
            public int? CategoryId { get; set; }
            public List<int>? TagIds { get; set; }  
         }

        public class Result
        {
            public List<OfferForListDTO> offerListDTO { get; set; } = new List<OfferForListDTO>();

        }

        public class OfferForListDTO
        {
            public required string Name { get; set; }
            public string City { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public required int MinSalary { get; set; }
            public required int MaxSalary { get; set; }
            public required EnumWorkMode WorkingMode { get; set; }
            public required EnumContractType ContractType { get; set; }


            public required string CompanyName { get; set; }

            public required string CategoryName { get; set; }

            public List<TagDTO> Tags { get; set; } = new List<TagDTO>();
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {

            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IAuthenticationDataProvider authenticationDataProvider) : base(currentAccountProvider, applicationDbContext)
            {

            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
               
                var offers = _applicationDbContext.Offers.ToList();

                offers = offers.Where(o => request.CompanyId == null ? true : o.CompanyAccountId == request.CompanyId)
                  .Where(o => request.CategoryId == null ? true : o.CategoryId == request.CategoryId)
                  .Where(o => request.City == null ? true : o.City == request.City)
                  .Where(o => request.WorkingMode == null ? true : o.WorkingMode == request.WorkingMode)
                  .Where(o => request.ContractType == null ? true : o.ContractType == request.ContractType)
                  .Where(o => request.MinSalary == null ? true : o.MinSalary <= request.MinSalary)
                  .Where(o => request.MaxSalary == null ? true : o.MaxSalary >= request.MaxSalary)
                  .ToList();

                offers = offers.Where(o => request.TagIds == null ? true : o.OfferTags.All(oft => request.TagIds.Contains(oft.TagId))).ToList();


                List<OfferForListDTO> offerListDto = offers.Select(offer =>
                {
                    var tagIds = _applicationDbContext.OfferTags.Where(oft => oft.OfferId == offer.Id).Select(oft => oft.TagId);

                    return new OfferForListDTO()
                    {
                        Name = offer.Name,
                        City = offer.City,
                        Location = offer.Location,
                        MinSalary = offer.MinSalary,
                        MaxSalary = offer.MaxSalary,
                        WorkingMode = offer.WorkingMode,
                        ContractType = offer.ContractType,

                        CompanyName = _applicationDbContext.companyAccounts.FirstOrDefault(ca => ca.Id == offer.CompanyAccountId)?.Name ?? string.Empty,
                        CategoryName = _applicationDbContext.Categories.FirstOrDefault(c => c.Id == offer.CategoryId)?.Name ?? string.Empty,
                        //Tags = _applicationDbContext.Tags.Where(t => offer.OfferTags.Any(oft => t.Id == oft.TagId)).Select(t => new TagDTO() { Name = t.Name, IconPath = "dafault" }).ToList()
                        Tags = _applicationDbContext.Tags.Where(t => tagIds.Contains(t.Id)).Select(t => new TagDTO() { Name = t.Name, IconPath = "default" }).ToList()
                    };
                }).ToList();



                return new Result() { offerListDTO = offerListDto };
                



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
