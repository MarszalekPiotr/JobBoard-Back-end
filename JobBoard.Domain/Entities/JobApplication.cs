using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class JobApplication : DomainEntity
    {
        public required DateTimeOffset ApplicationDate { get; set; }
        // public EnumApplicationStatus ApplicationStatus {get;set;}
        public string CVPath { get; set; } = string.Empty;

        public required int OfferId { get; set; }
        public Offer Offer { get; set; } = default!;

        public required Guid CandidateAccountId { get; set; }
        public CandidateAcccount CandidateAcccount { get; set; } = default!;
        public FormFilling FormFilling { get; set; } = default!;
    }
}
