using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Persistance.Configurations
{
    public class CompanyAccountUserConfiguration : IEntityTypeConfiguration<CompanyAccountUser>
    {
        public void Configure(EntityTypeBuilder<CompanyAccountUser> builder)
        {
            builder.HasOne(companyAccountUser => companyAccountUser.CompanyAccount)
                .WithMany(companyAccount => companyAccount.CompanyAccountUsers)
                .HasForeignKey(x => x.CompanyAccountId);

            builder.HasOne(companyAccountUser => companyAccountUser.User)
                .WithMany(user => user.CompanyAccountsUsers)
                .HasForeignKey(companyAccountUser => companyAccountUser.UserId);
        }
    }
}
