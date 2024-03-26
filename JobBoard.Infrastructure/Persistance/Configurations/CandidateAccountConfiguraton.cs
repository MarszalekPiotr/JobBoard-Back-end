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
    public class CandidateAccountConfiguraton : IEntityTypeConfiguration<CandidateAcccount>
    {
        public void Configure(EntityTypeBuilder<CandidateAcccount> builder)
        {
            builder.HasOne(candidateAccount => candidateAccount.User)
                .WithOne(user => user.CandidateAcccount)
                .HasForeignKey<CandidateAcccount>(candidateAcccount => candidateAcccount.UserId);
                
        }
    }
}
