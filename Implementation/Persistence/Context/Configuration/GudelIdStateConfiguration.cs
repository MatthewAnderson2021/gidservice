using GudelIdService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GudelIdService.Implementation.Persistence.Context.Configuration
{
    public class GudelIdStateConfiguration : IEntityTypeConfiguration<GudelIdState>
    {
        public void Configure(EntityTypeBuilder<GudelIdState> builder)
        {
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<Dictionary<string, string>>(p));
            builder.Property(p => p.Description).HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<Dictionary<string, string>>(p));

        }
    }
}