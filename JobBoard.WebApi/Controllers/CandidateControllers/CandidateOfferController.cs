using JobBoard.Application.Logic.Candidate;
using JobBoard.Application.Logic.Offers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.WebApi.Controllers.CandidateControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateOfferController : BaseController
    {
        public CandidateOfferController(ILogger<CandidateOfferController> logger, IMediator mediator) : base(logger, mediator)
        {
        }


        [HttpGet]
        public async Task<ActionResult> GetDetails([FromQuery] GetDetailsQuery.Request model)
        {
            var result = await _mediator.Send(model);

            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetList([FromQuery] GetListQuery.Request request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
