using EasyCaching.Core.Diagnostics;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.Domain.FormDefinitionSchema;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.DTO
{
    public class OfferDTO
    {   
        public int? Id { get; set; } 
        public required string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public required int MinSalary { get; set; }
        public required int MaxSalary { get; set; }
        public required EnumWorkMode WorkingMode { get; set; }
        public required EnumContractType ContractType { get; set; }

        public FormDefinition FormDefinition { get; set; } = new FormDefinition();
        public int CategoryId { get; set; }
   
        public List<int> TagIds { get; set; } = new List<int>() {  };

    }
}
