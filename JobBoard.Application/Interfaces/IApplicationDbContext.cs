using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<CandidateAcccount> candidateAcccounts { get; set; }
        DbSet<CompanyAccount> companyAccounts { get; set; }
        DbSet<CompanyAccountUser> companyAccountUsers { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
