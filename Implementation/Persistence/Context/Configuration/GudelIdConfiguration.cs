using GudelIdService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GudelIdService.Implementation.Persistence.Context.Configuration
{
    public class GudelIdConfiguration : IEntityTypeConfiguration<GudelId>
    {
        public void Configure(EntityTypeBuilder<GudelId> builder)
        {
            builder.Property(b => b.Id).HasColumnType("char(12)");
            builder.Property(b => b.CreatedBy).HasColumnType("varchar(255)");
            builder.Property(b => b.ReservedBy).HasColumnType("varchar(255)");
            builder.Property(b => b.ProducedBy).HasColumnType("varchar(255)");
            builder.Property(b => b.AssignedBy).HasColumnType("varchar(255)");
            builder.Property(b => b.VoidedBy).HasColumnType("varchar(255)");
            builder.Property(b => b.StateId).HasDefaultValue(GudelIdStates.Created.Id);
            builder.Property(b => b.CreatedBy).HasDefaultValue("system");
            builder.Property(b => b.CreationDate).ValueGeneratedOnAdd();
            builder.HasOne(c => c.State).WithMany(b => b.GudelIds).HasForeignKey(p => p.StateId);
            builder.Property(b => b.TypeId).HasDefaultValue(1);
            builder.HasOne(c => c.Type).WithMany(b => b.GudelIds).HasForeignKey(p => p.TypeId);
            builder.HasOne(p => p.Pool).WithMany(p => p.GudelIds);
            builder.Property(p => p.Name).HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<Dictionary<string, string>>(p));
            builder.Property(p => p.Description).HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<Dictionary<string, string>>(p));
        }
    }
}

