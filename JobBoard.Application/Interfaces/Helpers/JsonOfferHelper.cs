using JobBoard.Application.DTO;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces.Helpers
{
    public interface IJsonOfferHelper
    {   
        OfferDTO ParseJsonToOfferDTO(JsonDocument jsonDocument);

    }
}
