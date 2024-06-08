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
    public class FormFillingConfiguration : IEntityTypeConfiguration<FormFilling>
    {
        public void Configure(EntityTypeBuilder<FormFilling> builder)
        {
            builder.HasOne(ff => ff.JobApplication)
                .WithOne(ja => ja.FormFilling)
                .HasForeignKey<FormFilling>(ff => ff.JobApplicationId);

            builder.HasMany(ff => ff.FieldFillings)
                .WithOne(fldf => fldf.FormFilling)
                .HasForeignKey(fldf => fldf.FormFillingId);
        }
    }
}
