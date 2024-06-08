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
    internal class JobApplicationConfiguraton : IEntityTypeConfiguration<JobApplication>
    {
        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            builder.HasOne(ja => ja.Offer)
                .WithMany(o => o.JobApplications)
                .HasForeignKey(ja => ja.OfferId);

            builder.HasOne(ja => ja.CandidateAcccount)
                .WithMany(ca => ca.JobApplications)
                .HasForeignKey(ja => ja.CandidateAccountId);
        }
    }
}
