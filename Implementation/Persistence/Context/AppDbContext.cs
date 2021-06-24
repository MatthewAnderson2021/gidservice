using EntityFrameworkCore.CustomMigration;
using GudelIdService.Domain.Models;
using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Persistence.Context.Configuration;
using GudelIdService.Implementation.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GudelIdService.Implementation.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<GudelId> GudelId { get; set; }

        public DbSet<GudelIdType> GudelIdTypes { get; set; }

        public DbSet<ExtraField> ExtraField { get; set; }

        public DbSet<ExtraFieldDefinition> ExtraFieldDefinition { get; set; }

        public DbSet<GudelIdState> GudelIdState { get; set; }

        public DbSet<Activity> Activity { get; set; }

        public DbSet<PermissionKey> PermissionKeys { get; set; }

        public DbSet<ExtraFieldDefinitionGudelIdState> ExtraFieldDefinitionGudelIdState { get; set; }

        public DbSet<Pool> Pool { get; set; }
        private readonly IConfigService _config;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfigService config) : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(
                _config.Get(ConfigService.MYSQLCONNSTR_DEFAULT),
                x => x.MigrationsHistoryTable("__Migration_IdService")
            );
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GudelIdStateConfiguration());
            builder.Seed(new List<GudelIdState> {
                Domain.Models.GudelIdStates.Created,
                Domain.Models.GudelIdStates.Assigned,
                Domain.Models.GudelIdStates.Produced,
                Domain.Models.GudelIdStates.Reserved,
                Domain.Models.GudelIdStates.Voided
            });
            builder.ApplyConfiguration(new GudelIdConfiguration());
            builder.ApplyConfiguration(new GudelIdTypeConfiguration());
            builder.ApplyConfiguration(new ExtraFieldConfiguration());
            builder.ApplyConfiguration(new ExtraFieldDefinitionConfiguration());
            builder.ApplyConfiguration(new ActivityConfiguration());
            builder.ApplyConfiguration(new PoolConfiguration());
            builder.ApplyConfiguration(new ExtraFieldDefinitionGudelIdStateConfiguration());
            builder.ApplyConfiguration(new PermissionKeyConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
namespace EntityFrameworkCore.CustomMigration
{
    public static class CustomModelBuilder
    {
        public static bool IsSignedInteger(this Type type)
           => type == typeof(int)
              || type == typeof(long)
              || type == typeof(short)
              || type == typeof(sbyte);

        public static void Seed<T>(this ModelBuilder modelBuilder, IEnumerable<T> data) where T : class
        {
            var entnty = modelBuilder.Entity<T>();

            var pk = entnty.Metadata
                .GetProperties()
                .FirstOrDefault(property =>
                    property.RequiresValueGenerator()
                    && property.IsPrimaryKey()
                    && property.ClrType.IsSignedInteger()
                    && property.ClrType.IsDefaultValue(0)
                );
            if (pk != null)
            {
                entnty.Property(pk.Name).ValueGeneratedNever();
                entnty.HasData(data);
                entnty.Property(pk.Name).UseMySqlIdentityColumn();
            }
            else
            {
                entnty.HasData(data);
            }
        }
    }
}