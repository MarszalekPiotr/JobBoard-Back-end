using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using FluentValidation.Internal;
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
using System.Xml.Linq;

namespace JobBoard.Application.Logic.Offers
{
    public static class CreateOrUpdateOfferCommand
    {

        public class Request : IRequest<Result>
        {
            public  int? Id { get; set; } 
            public  string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public  int MinSalary { get; set; }
            public  int MaxSalary { get; set; }
            public  EnumWorkMode WorkingMode { get; set; }
            public  EnumContractType ContractType { get; set; }

            public FormDefinition FormDefinitionJSON { get; set; } = new FormDefinition();
            public int CategoryId { get; set; }

            public List<int> TagIds { get; set; } = new List<int>() { };
            public Request(OfferDTO offerDTO)
            {
                // mapper?
                this.Id = offerDTO.Id;
                this.Name = offerDTO.Name;
                this.Description = offerDTO.Description;
                this.City = offerDTO.City;
                this.Location = offerDTO.Location;
                this.CategoryId = offerDTO.CategoryId;
                this.MinSalary = offerDTO.MinSalary;
                this.MaxSalary = offerDTO.MaxSalary;
                this.WorkingMode = offerDTO.WorkingMode;
                this.ContractType = offerDTO.ContractType;
                this.TagIds = offerDTO.TagIds;
                this.FormDefinitionJSON = offerDTO.FormDefinition;
            }
            
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
                
                


                    /// add or update offer logic.....
                    /// check if exists alredy in db if yes then update if not then create 
                    /// if exists check if accountId is matching
                    /// if not exist then add current account id to the db of this offer.
                    /// 
                    var companyAccount = await _currentAccountProvider.GetCurrentCompanyAccount();
                    if (companyAccount == null) throw new UnauthorizedException();
                    Offer offer = null;
                    if (request.Id.HasValue)
                    {
                        offer = _applicationDbContext.Offers.FirstOrDefault(o => o.Id == (int)request.Id && o.CompanyAccountId == companyAccount.Id);
                        if (offer == null)
                        {
                            throw new UnauthorizedException();
                        }
                        else
                        {
                            offer.Name = request.Name;
                            offer.Description = request.Description;
                            offer.City = request.City;
                            offer.Location = request.Location;
                            offer.CategoryId = request.CategoryId;
                            offer.MinSalary = request.MinSalary;
                            offer.MaxSalary = request.MaxSalary;
                            offer.WorkingMode = request.WorkingMode;
                            offer.ContractType = request.ContractType;
                            offer.FormDefinitionJSON = request.FormDefinitionJSON;
                            await UpdateOfferTags(request, offer.Id); 
                           
                            
                        }
                    }
                    else
                    {
                        offer = new Offer()
                        {
                            Name = request.Name,
                            Description = request.Description,
                            City = request.City,
                            Location = request.Location,
                            CategoryId = request.CategoryId,
                            MinSalary = request.MinSalary,
                            MaxSalary = request.MaxSalary,
                            WorkingMode = request.WorkingMode,
                            ContractType = request.ContractType,
                            FormDefinitionJSON = request.FormDefinitionJSON,
                            CompanyAccountId = companyAccount.Id,
                        };
                        
                   
                        var offerEntity = await _applicationDbContext.Offers.AddAsync(offer, cancellationToken);
                       await _applicationDbContext.SaveChangesAsync(cancellationToken);

                     //request.TagIds.ForEach(ti =>  _applicationDbContext.OfferTags.Add(new OfferTag() { OfferId = offerEntity.Entity.Id, TagId = ti }));
                     await UpdateOfferTags(request, offerEntity.Entity.Id);
 
                }
                    

                    return new Result() { OfferId = offer.Id };

            }

            private async Task UpdateOfferTags(Request request,int offerId)
            {
                if (request.Id.HasValue)
                {
                    //var offerTags = _applicationDbContext.Offers.FirstOrDefault(o => o.Id == offerId)?.OfferTags;
                    var offerTags = _applicationDbContext.OfferTags.Where(ot => ot.OfferId == offerId);
                    _applicationDbContext.OfferTags.RemoveRange(offerTags);
                      
                }
                request.TagIds.ForEach(ti => _applicationDbContext.OfferTags.Add(new OfferTag() { OfferId = offerId, TagId = ti }));
                await _applicationDbContext.SaveChangesAsync();
            } 
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
               // validations for all the fields
               // validation foreach fields... so propbably some kind of validator. each must have name and type.. so not null 
               // and max min etc. are optional. because tere are some default values.

              

            }
        }

    }
}
