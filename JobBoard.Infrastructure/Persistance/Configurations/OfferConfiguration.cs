using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.Domain.FormDefinitionSchema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

            var serializerOptions = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.Preserve};
            serializerOptions.Converters.Add(new FormDefinitionConverter());
            serializerOptions.Converters.Add(new JsonStringEnumConverter());

           

            builder.Property(o => o.FormDefinitionJSON)
                .HasColumnName("FormDefinitionJSON")
                .HasConversion(
                v => JsonSerializer.Serialize(v, serializerOptions),
                v => JsonSerializer.Deserialize<FormDefinition>(v, serializerOptions) ?? new FormDefinition()
            );


            builder
       .Property(o => o.WorkingMode)
       .HasConversion(
           v => v.ToString(),
           v => (EnumWorkMode)Enum.Parse(typeof(EnumWorkMode), v));

            builder
                .Property(o => o.ContractType)
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumContractType)Enum.Parse(typeof(EnumContractType), v));
        }
    }
}
