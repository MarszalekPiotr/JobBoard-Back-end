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
        DbSet<CandidateAcccount> CandidateAccounts { get; set; }
        DbSet<CompanyAccount> companyAccounts { get; set; }
        DbSet<CompanyAccountUser> companyAccountUsers { get; set; }
        DbSet<Offer> Offers { get; set; }
        DbSet<JobApplication> JobApplications { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Category> Categories {  get; set; } 
        DbSet<OfferTag> OfferTags { get; set; }
        DbSet<FormFilling> FormFillings { get; set; }
        DbSet<FieldFilling> FieldFillings { get; set; } 
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
