using JobBoard.Application.DTO;
using JobBoard.Application.Interfaces.Helpers;
using JobBoard.Domain.Enums;
using JobBoard.Domain.FormDefinitionSchema;
using JobBoard.Infrastructure.Persistance;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Helpers
{
    public class JsonOfferHelper : IJsonOfferHelper
    {

            private JsonSerializerOptions _serializerOptions;
              
             public JsonOfferHelper()
             {
                _serializerOptions = new JsonSerializerOptions();
                //_serializerOptions.Converters.Add(new BaseFieldDefinitionConverter());
                _serializerOptions.Converters.Add(new FormDefinitionConverter());
                _serializerOptions.Converters.Add(new JsonStringEnumConverter());
             }
            
            public OfferDTO ParseJsonToOfferDTO(JsonDocument jsonDocument)
            {

               var formDefinitionJson = jsonDocument.RootElement.GetProperty("FormDefinitionJSON");

               var formDefinition = JsonSerializer.Deserialize<FormDefinition>(formDefinitionJson.GetRawText(), _serializerOptions);
             var offerDTO = new OfferDTO()
              {
                Name = JsonSerializer.Deserialize<string>(jsonDocument.RootElement.GetProperty("Name").GetRawText(), _serializerOptions),
                Description = JsonSerializer.Deserialize<string>(jsonDocument.RootElement.GetProperty("Description").GetRawText(), _serializerOptions),

                City = JsonSerializer.Deserialize<string>(jsonDocument.RootElement.GetProperty("City").GetRawText(), _serializerOptions),

                Location = JsonSerializer.Deserialize<string>(jsonDocument.RootElement.GetProperty("Location").GetRawText(), _serializerOptions),

                MinSalary = JsonSerializer.Deserialize<int>(jsonDocument.RootElement.GetProperty("MinSalary").GetRawText(), _serializerOptions),

                MaxSalary = JsonSerializer.Deserialize<int>(jsonDocument.RootElement.GetProperty("MaxSalary").GetRawText(), _serializerOptions),


                ContractType = JsonSerializer.Deserialize<EnumContractType>(jsonDocument.RootElement.GetProperty("ContractType").GetRawText(), _serializerOptions),

                WorkingMode = JsonSerializer.Deserialize<EnumWorkMode>(jsonDocument.RootElement.GetProperty("WorkingMode").GetRawText(), _serializerOptions),


                CategoryId = JsonSerializer.Deserialize<int>(jsonDocument.RootElement.GetProperty("CategoryId").GetRawText(), _serializerOptions),

                TagIds = JsonSerializer.Deserialize<List<int>>(jsonDocument.RootElement.GetProperty("TagIds").GetRawText(), _serializerOptions),

              

                FormDefinition = formDefinition
            };

            if (jsonDocument.RootElement.TryGetProperty("Id" ,out JsonElement jsonElement ))
            {
                offerDTO.Id = JsonSerializer.Deserialize<int>(jsonDocument.RootElement.GetProperty("Id").GetRawText(), _serializerOptions);
            }
            else
            {
                offerDTO.Id = null;
            }
                 
        
            return offerDTO;
    }
    }
}
