using JobBoard.Application.DTO;
using JobBoard.Application.Logic.Candidate;
using JobBoard.Application.Logic.Company;
using JobBoard.Application.Logic.Offers;
using JobBoard.Domain.FormDefinitionSchema;

using JobBoard.Infrastructure.Persistance;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobBoard.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Obsolete]
    public class OfferController : BaseController
    {
      
        public OfferController(ILogger<OfferController> logger, IMediator mediator) : base(logger, mediator)
        {
           
        }

       

      

    }
}
