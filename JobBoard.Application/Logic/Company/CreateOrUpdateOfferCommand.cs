using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using FluentValidation.Internal;
using JobBoard.Application.DTO;
using JobBoard.Application.Exceptions;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Logic.Abstractions;
using JobBoard.Application.Validators;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.Domain.FormDefinitionSchema;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JobBoard.Application.Logic.Company
{
    public static class CreateOrUpdateOfferCommand
    {

        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int? CityId { get; set; }
            public string Location { get; set; } = string.Empty;
            public int MinSalary { get; set; }
            public int MaxSalary { get; set; }
            public EnumWorkMode WorkingMode { get; set; }
            public EnumContractType ContractType { get; set; }
            public EnumOfferStatus OfferStatus { get; set; }

            public FormDefinition? FormDefinitionJSON { get; set; }
            public int CategoryId { get; set; }

            public List<int> TagIds { get; set; } = new List<int>() { };

        }

        public class Result
        {
            public int OfferId { get; set; }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {

            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {

            }


            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var companyAccount = await _currentAccountProvider.GetCurrentCompanyAccount();
                if (companyAccount == null) throw new UnauthorizedException();
                Offer offer = null;
                if (request.Id.HasValue)
                {
                    offer = companyAccount.Offers.FirstOrDefault(off => off.Id == (int)request.Id);
                    if (offer == null)
                    {
                        throw new UnauthorizedException();
                    }
                    else
                    {
                        offer.Name = request.Name;
                        offer.Description = request.Description;
                        offer.CityId = request.CityId;
                        offer.Address = request.Location;
                        offer.CategoryId = request.CategoryId;
                        offer.MinSalary = request.MinSalary;
                        offer.MaxSalary = request.MaxSalary;
                        offer.WorkingMode = request.WorkingMode;
                        offer.ContractType = request.ContractType;
                        offer.FormDefinitionJSON = request.FormDefinitionJSON;
                        offer.OfferStatus = request.OfferStatus;
                        await UpdateOfferTags(request, offer.Id);

                    }
                }
                else
                {
                    offer = new Offer()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        CityId = request.CityId,
                        Address = request.Location,
                        CategoryId = request.CategoryId,
                        MinSalary = request.MinSalary,
                        MaxSalary = request.MaxSalary,
                        WorkingMode = request.WorkingMode,
                        ContractType = request.ContractType,
                        FormDefinitionJSON = request.FormDefinitionJSON,
                        CompanyAccountId = companyAccount.Id,
                        OfferStatus = request.OfferStatus
                    };
                    var offerEntity = await _applicationDbContext.Offers.AddAsync(offer, cancellationToken);
                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                    // Q:1 czy jak usunę ten save chabges async to nie sprawi to ze nie bede mial dostepu do offerId? bo potrzebuje tego dalej
                    await UpdateOfferTags(request, offerEntity.Entity.Id);

                }
                return new Result() { OfferId = offer.Id };

            }

            private async Task UpdateOfferTags(Request request, int offerId)
            {
                if (request.Id.HasValue)
                {

                    var presentTagIds = _applicationDbContext.OfferTags.Where(ot => ot.OfferId == offerId).Select(ot => ot.TagId);
                    request.TagIds.ForEach(ti =>
                    {
                        if (!presentTagIds.Contains(ti))
                        {
                            _applicationDbContext.OfferTags.Add(new OfferTag() { OfferId = offerId, TagId = ti });
                        }

                    }
                    );
                    presentTagIds.ForEachAsync(pti =>
                    {
                        if (!request.TagIds.Contains(pti))
                        {
                            var offerTagsToDelete = _applicationDbContext.OfferTags.Where(ot => ot.OfferId == offerId && ot.TagId == pti);
                            _applicationDbContext.OfferTags.RemoveRange(offerTagsToDelete);
                        }
                    });
                }
                else
                {
                    request.TagIds.ForEach(ti => _applicationDbContext.OfferTags.Add(new OfferTag() { OfferId = offerId, TagId = ti }));
                    await _applicationDbContext.SaveChangesAsync();
                }
            }
        }

        public class Validator : AbstractValidator<Request>
        {

          

            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MinimumLength(5).MaximumLength(30);
                RuleFor(x => x.Description).NotEmpty().MinimumLength(30).MaximumLength(2500);

                RuleFor(x => x.Location).NotEmpty().MaximumLength(50);
                RuleFor(x => x.MinSalary).NotEmpty();
                RuleFor(x => x.MaxSalary).NotEmpty();
                RuleFor(x => x.WorkingMode).NotEmpty();
                RuleFor(x => x.ContractType).NotEmpty();
                RuleFor(x => x.CategoryId).NotEmpty();
                RuleFor(x => x.FormDefinitionJSON).FormDefinitionCorrect();

                
            }
        }

    }
}
