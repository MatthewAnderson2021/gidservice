using GudelIdService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Persistence.Context.Configuration
{
    public class PermissionKeyConfiguration : IEntityTypeConfiguration<PermissionKey>
    {
        public void Configure(EntityTypeBuilder<PermissionKey> builder)
        {
            builder.HasKey(permissionKey => new { permissionKey.GudelId_Id, permissionKey.Type });
            builder.Property(persmissionKey => persmissionKey.Key);
            builder.Property(persmissionKey => persmissionKey.Hint);
            builder.Property(persmissionKey => persmissionKey.Type);
            builder.HasOne(permissionKey => permissionKey.GudelId)
                   .WithMany(gudelId => gudelId.PermissionKeys)
                   .HasForeignKey(permissionKey => permissionKey.GudelId_Id);
        }
    }
}
