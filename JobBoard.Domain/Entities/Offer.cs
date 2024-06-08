﻿using JobBoard.Domain.Common;
using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class Offer : DomainEntity
    {
        public required string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public  string City { get; set; } = string.Empty; 
        public  string Location { get; set; } = string.Empty;
        public required int MinSalary { get; set; } 
        public required  int MaxSalary { get; set;}
        public required EnumWorkMode WorkingMode { get; set; }
        public required EnumContractType ContractType { get; set; }

        // FormDefinition JSON 
        public required Guid CompanyAccountId { get; set; }
        public required CompanyAccount CompanyAccount {  get; set; } 

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
        public ICollection<OfferTag> OfferTags { get; set; } = new List<OfferTag>();
        
    }
}