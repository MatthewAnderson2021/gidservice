using GudelIdService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GudelIdService.Implementation.Persistence.Context.Configuration
{
    public class ExtraFieldConfiguration : IEntityTypeConfiguration<ExtraField>
    {
        public void Configure(EntityTypeBuilder<ExtraField> builder)
        {
            builder.HasKey(p => new { p.GudelId, p.ExtraFieldDefinitionId });
            builder.Property(b => b.GudelId).HasColumnType("char(12)");
            builder.Property(p => p.Value).HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<Dictionary<string, string>>(p));
            builder.HasOne(c => c.Gudel).WithMany(b => b.ExtraFields).HasForeignKey(p => p.GudelId);
        }
    }
}