using JobBoard.Domain.Common;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface ICurrentAccountProvider
    {
        // how to manage multiple account types?

      Task<CandidateAcccount> GetCurrentCandidateAccount();
      Task<CompanyAccount> GetCurrentCompanyAccount();
      Task<EnumAccountType> GetAccountType();
      Task<Guid> GetAccountId();
       
      
    }
}
