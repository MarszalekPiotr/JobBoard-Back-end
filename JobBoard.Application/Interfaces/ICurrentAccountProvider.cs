using JobBoard.Domain.Common;
using JobBoard.Domain.Entities;
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
      Task<int> GetAuthenticatedAcountType();
      Task<CompanyAccount>  GetAuthenticatedCompanyAccount();
      Task<int> GetAccountId();

      
    }
}
