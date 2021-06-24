using GudelIdService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GudelIdService.Implementation.Persistence.Context.Configuration
{
    public class GudelIdTypeConfiguration : IEntityTypeConfiguration<GudelIdType>
    {
        public void Configure(EntityTypeBuilder<GudelIdType> builder)
        {
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Name).HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<Dictionary<string, string>>(p));
            builder.Property(t => t.Description).HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<Dictionary<string, string>>(p));
            builder.HasData(
                Domain.Models.GudelIdTypes.developmentid,
                Domain.Models.GudelIdTypes.infrastructureasset,
                Domain.Models.GudelIdTypes.productionasset,
                Domain.Models.GudelIdTypes.smartproduct,
                Domain.Models.GudelIdTypes.user
            );
        }
    }
}