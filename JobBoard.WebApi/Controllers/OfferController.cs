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
    public class OfferController : BaseController
    {
        private JsonSerializerOptions _serializerOptions;
        public OfferController(ILogger<OfferController> logger, IMediator mediator) : base(logger, mediator)
        {
            _serializerOptions = new JsonSerializerOptions();
            _serializerOptions.Converters.Add(new BaseFieldDefinitionConverter());
            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        [HttpPost]
        public async  Task<ActionResult> AddOffer( JsonDocument jsondoc)
        {  
            // i jais helper zeby wyciagnac poszczegolne pola i poszczegolne pola deserializowac odpowiendim parserem? jsonOfferHekper ktory nam sparsuje do jakiegos dto
            // i w nim mozna obsluge zrobic parsera zeby juz tam miec gotowy form def

            // do serializatora tylko trzeba go zmienic na caly form...
            var formDefinitionJson = jsondoc.RootElement.GetProperty("FormDefinitionJSON");
            List<BaseFieldDefinition> fields = new List<BaseFieldDefinition>();

            foreach( var element in formDefinitionJson.EnumerateArray())
            {
                var fieldDefinition = JsonSerializer.Deserialize<BaseFieldDefinition>(element.GetRawText(),_serializerOptions);
                fields.Add(fieldDefinition);
            }

            //var formDefinition = JsonSerializer.Deserialize<BaseFieldDefinition>(formDefinitionJson.GetRawText(),_serializerOptions);


            Console.WriteLine(fields.ToString());
            Console.WriteLine();
            return Ok();
        }

    }
}
