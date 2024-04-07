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
       public  DbSet<CandidateAcccount> candidateAcccounts { get; set; }
       public DbSet<CompanyAccount> companyAccounts { get; set; }
       public DbSet<CompanyAccountUser> companyAccountUsers { get; set; }
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
