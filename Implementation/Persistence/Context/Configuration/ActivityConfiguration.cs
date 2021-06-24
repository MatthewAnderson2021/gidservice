using GudelIdService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GudelIdService.Implementation.Persistence.Context.Configuration
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.Property(b => b.GudelId).HasColumnType("char(12)");
            builder.Property(b => b.CreationDate).ValueGeneratedOnAdd();
        }
    }
}