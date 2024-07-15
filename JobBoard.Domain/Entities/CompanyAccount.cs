using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class CompanyAccount: BaseAccount
    {
      
       public ICollection<CompanyAccountUser> CompanyAccountUsers { get; set; }  = new List<CompanyAccountUser>();
       public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
