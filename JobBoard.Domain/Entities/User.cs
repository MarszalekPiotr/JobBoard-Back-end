using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class User: DomainEntity
    {
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
        public DateTimeOffset RegisterDate {  get; set; }
        public ICollection<CompanyAccount> CompanyAccounts { get; set; } = new List<CompanyAccount>();
        public CandidateAcccount CandidateAcccount { get; set; } = default!;
    }
}
