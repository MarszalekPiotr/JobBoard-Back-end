using JobBoard.Domain.Entities;
using JobBoard.Domain.FormDefinitionSchema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Persistance.Configurations
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.Category)
                .WithMany(c => c.Offers)
                .HasForeignKey(o => o.CategoryId);

            builder.HasOne(o => o.CompanyAccount)
                .WithMany(ca => ca.Offers)
                 .HasForeignKey(o => o.CompanyAccountId);

            builder.HasMany(o => o.OfferTags)
                .WithOne(ot => ot.Offer)
                .HasForeignKey(ot => ot.OfferId);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new BaseFieldDefinitionConverter());

            builder.Property(o => o.FormDefinitionJSON)
                .HasColumnName("FormDefinitionJSON")
                .HasConversion(
                v => JsonSerializer.Serialize(v, serializerOptions),
                v => JsonSerializer.Deserialize<FormDefinition>(v, serializerOptions) ?? new FormDefinition()
                );
        }
    }
}
