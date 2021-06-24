using GudelIdService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GudelIdService.Implementation.Persistence.Context.Configuration
{
    public class PoolConfiguration : IEntityTypeConfiguration<Pool>
    {
        public void Configure(EntityTypeBuilder<Pool> builder)
        {
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.CreationDate).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<Dictionary<string, string>>(p));
            builder.Property(p => p.Description).HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<Dictionary<string, string>>(p));
        }
    }
}