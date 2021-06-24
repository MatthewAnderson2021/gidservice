using GudelIdService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GudelIdService.Implementation.Persistence.Context.Configuration
{
    public class ExtraFieldDefinitionGudelIdStateConfiguration : IEntityTypeConfiguration<ExtraFieldDefinitionGudelIdState>
    {
        public void Configure(EntityTypeBuilder<ExtraFieldDefinitionGudelIdState> builder)
        {
            builder.HasKey(k => new { k.ExtraFieldDefinitionId, k.GudelIdStateId });
            builder.HasOne(t => t.ExtraFieldDefinition).WithMany(t => t.ExtraFieldDefinitionGudelIdState).HasForeignKey(t => t.ExtraFieldDefinitionId);
        }
    }
}