using JobBoard.Application.Logic.Company;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.WebApi.Controllers.CompanyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyOfferController : BaseController
    {
        public CompanyOfferController(ILogger<CompanyOfferController> logger, IMediator mediator) : base(logger, mediator)
        {
        }


        [HttpPost]
        public async Task<ActionResult> CreateOrUpdate(CreateOrUpdateOfferCommand.Request request)
        {

            var model = await _mediator.Send(request);
            return Ok(model);

        }

    }
}
