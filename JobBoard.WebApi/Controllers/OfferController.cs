using JobBoard.Application.DTO;
using JobBoard.Application.Interfaces.Helpers;
using JobBoard.Application.Logic.Offers;
using JobBoard.Domain.FormDefinitionSchema;
using JobBoard.Infrastructure.Helpers;
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
    public class OfferController : BaseController
    {
        private JsonSerializerOptions _serializerOptions;
        private IJsonOfferHelper _jsonOfferHelper;
        public OfferController(ILogger<OfferController> logger, IMediator mediator, IJsonOfferHelper jsonOfferHelper) : base(logger, mediator)
        {
            _serializerOptions = new JsonSerializerOptions();
            _serializerOptions.Converters.Add(new BaseFieldDefinitionConverter());
            _serializerOptions.Converters.Add(new FormDefinitionConverter());
            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
            _jsonOfferHelper = jsonOfferHelper;
        }

        [HttpPost]
        //public async  Task<ActionResult> CreateOrUpdate( JsonDocument jsondoc)
        //{    

        //    OfferDTO offerDto = _jsonOfferHelper.ParseJsonToOfferDTO(jsondoc);


        //    // send to mediatr --- 
        //    var model = await _mediator.Send(new CreateOrUpdateOfferCommand.Request(offerDto));
        //    return Ok(model.OfferId);

        //}

        [HttpPost]
        public async Task<ActionResult> CreateOrUpdateNEW(CreateOrUpdateOfferCommand.Request request)
        {
            // send to mediatr --- 
            var model = await _mediator.Send(request);
            return Ok(model.OfferId);

        }

        [HttpGet]
        public async  Task<ActionResult> GetDetails([FromQuery] GetDetailsQuery.Request model)
        {
            var result = await _mediator.Send(model);
            
            return Ok(result);
        }

    }
}
