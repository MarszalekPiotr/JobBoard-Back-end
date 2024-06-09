using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Persistance
{
    public class MainDbContext : DbContext, IApplicationDbContext
    {  
        public MainDbContext(DbContextOptions<MainDbContext> options):base(options)
        {

        }
       public DbSet<User> Users { get; set; }
       public  DbSet<CandidateAcccount> CandidateAccounts { get; set; }
       public DbSet<CompanyAccount> companyAccounts { get; set; }
       public DbSet<CompanyAccountUser> companyAccountUsers { get; set; }
        public DbSet<Offer> Offers { get ; set; }
        public DbSet<JobApplication> JobApplications { get ; set ; }
        public DbSet<Tag> Tags { get; set ; }
        public DbSet<Category> Categories { get ; set; }
        public DbSet<OfferTag> OfferTags { get; set; }
        public DbSet<FormFilling> FormFillings { get; set; }
        public DbSet<FieldFilling> FieldFillings { get ;set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 4);
            base.ConfigureConventions(configurationBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }

    }
}
