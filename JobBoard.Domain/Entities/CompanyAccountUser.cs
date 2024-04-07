using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class CompanyAccountUser: DomainEntity
    {   
        public Guid  CompanyAccountId { get; set; }
        public CompanyAccount CompanyAccount { get; set; } = default!;
        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
