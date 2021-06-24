﻿// <auto-generated />
using System;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GudelIdService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200602100000_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GudelIdService.Domain.Models.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GudelId")
                        .HasColumnType("char(12)");

                    b.Property<bool>("IsExtraField")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NewValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OldValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("Uid")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("GudelId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.ExtraField", b =>
                {
                    b.Property<string>("GudelId")
                        .HasColumnType("char(12)");

                    b.Property<int>("ExtraFieldDefinitionId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("GudelId", "ExtraFieldDefinitionId");

                    b.HasIndex("ExtraFieldDefinitionId");

                    b.ToTable("ExtraField");
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.ExtraFieldDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("GudelIdStateId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Type")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("GudelIdStateId");

                    b.ToTable("ExtraFieldDefinition");
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.ExtraFieldDefinitionGudelIdState", b =>
                {
                    b.Property<int>("ExtraFieldDefinitionId")
                        .HasColumnType("int");

                    b.Property<int>("GudelIdStateId")
                        .HasColumnType("int");

                    b.HasKey("ExtraFieldDefinitionId", "GudelIdStateId");

                    b.HasIndex("GudelIdStateId");

                    b.ToTable("ExtraFieldDefinitionGudelIdState");
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.GudelId", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(12)");

                    b.Property<string>("AssignedBy")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("AssignmentDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)")
                        .HasDefaultValue("system");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OwnerKey")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("PoolId")
                        .HasColumnType("int");

                    b.Property<string>("PrivateKey")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ProducedBy")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("ProductionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ReservationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ReservedBy")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("VoidDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("VoidedBy")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PoolId");

                    b.HasIndex("StateId");

                    b.HasIndex("TypeId");

                    b.ToTable("GudelId");
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.GudelIdState", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("GudelIdState");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Description = "{\"en-US\":\"Initial Status after Güdel ID is created in the global ID pool.\",\"de-DE\":\"Innitialer Status nachdem eine Güdel ID global erstellt wurde.\"}",
                            Name = "{\"en-US\":\"Created\",\"de-DE\":\"Erstellt\"}"
                        },
                        new
                        {
                            Id = 30,
                            Description = "{\"en-US\":\"Güdel ID is assigned to a product and checked.\\nGüdel Smart Products can be linked with Güdel ID\",\"de-DE\":\"Güdel ID ist einem Produkt zugeordnet und geprüft. \\nGüdel Smart Products sind mit einer Güdel ID verknüpft.\"}",
                            Name = "{\"en-US\":\"Assigned\",\"de-DE\":\"Zugewiesen\"}"
                        },
                        new
                        {
                            Id = 20,
                            Description = "{\"en-US\":\"Güdel ID is produced (e.g. printed on a label) and checked\",\"de-DE\":\"Güdel ID ist produziert (z.B. auf ein Label gedruckt) und überprüft\"}",
                            Name = "{\"en-US\":\"Produced\",\"de-DE\":\"Produziert\"}"
                        },
                        new
                        {
                            Id = 10,
                            Description = "{\"en-US\":\"Güdel ID is reserved and transferred to a local ID pool to make sure it is available for the user even if offline.\",\"de-DE\":\"Die Güdel ID ist reserviert und einem lokalen ID Pool zugeordnet. Es wird sichergestellt dass die ID auch offline verfügbar ist.\"}",
                            Name = "{\"en-US\":\"Reserved\",\"de-DE\":\"Reserviert\"}"
                        },
                        new
                        {
                            Id = 99,
                            Description = "{\"en-US\":\"Güdel ID is faulty and must not be used\",\"de-DE\":\"Güdel ID ist Fehlerhaft und darf nicht genutzt werden\"}",
                            Name = "{\"en-US\":\"Void\",\"de-DE\":\"Ungültig\"}"
                        });
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.GudelIdType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("GudelIdTypes");

                    b.HasData(
                        new
                        {
                            Id = 5,
                            Description = "{\"en-US\":\"Güdel ID for Güdel ID System development and testing\",\"de-DE\":\"Güdel ID zur Entwicklung und Testing des Güdel ID System\"}",
                            Name = "{\"en-US\":\"DevelopmentID\",\"de-DE\":\"DevelopmentID\"}"
                        },
                        new
                        {
                            Id = 4,
                            Description = "{\"en-US\":\"Güdel ID of IT infrastructure assets\",\"de-DE\":\"Güdel ID eines IT Infrastruktur Assets\"}",
                            Name = "{\"en-US\":\"InfrastructureAsset\",\"de-DE\":\"InfrastructureAsset\"}"
                        },
                        new
                        {
                            Id = 3,
                            Description = "{\"en-US\":\"Güdel ID of an asset which could be used to assign Güdel IDs to assets\",\"de-DE\":\"Güdel ID eines assets welches dazu genutzt werden kann Güdel IDs zuzuordnen\"}",
                            Name = "{\"en-US\":\"ProductionAsset\",\"de-DE\":\"ProductionAsset\"}"
                        },
                        new
                        {
                            Id = 1,
                            Description = "{\"en-US\":\"Güdel ID of a smart product by Güdel\",\"de-DE\":\"Güdel ID eines Smart Product von Güdel\"}",
                            Name = "{\"en-US\":\"SmartProduct\",\"de-DE\":\"SmartProduct\"}"
                        },
                        new
                        {
                            Id = 2,
                            Description = "{\"en-US\":\"Güdel ID of a human being, user of Güdel ID System\",\"de-DE\":\"Güdel ID eines menschlichen Benutzers des Güdel ID System\"}",
                            Name = "{\"en-US\":\"User\",\"de-DE\":\"User\"}"
                        });
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.Pool", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ExternalId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Pool");
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.Activity", b =>
                {
                    b.HasOne("GudelIdService.Domain.Models.GudelId", "Gudel")
                        .WithMany("Activities")
                        .HasForeignKey("GudelId");
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.ExtraField", b =>
                {
                    b.HasOne("GudelIdService.Domain.Models.ExtraFieldDefinition", "ExtraFieldDefinition")
                        .WithMany()
                        .HasForeignKey("ExtraFieldDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GudelIdService.Domain.Models.GudelId", "Gudel")
                        .WithMany("ExtraFields")
                        .HasForeignKey("GudelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.ExtraFieldDefinition", b =>
                {
                    b.HasOne("GudelIdService.Domain.Models.GudelIdState", null)
                        .WithMany("ExtraFieldDefinition")
                        .HasForeignKey("GudelIdStateId");
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.ExtraFieldDefinitionGudelIdState", b =>
                {
                    b.HasOne("GudelIdService.Domain.Models.ExtraFieldDefinition", "ExtraFieldDefinition")
                        .WithMany("ExtraFieldDefinitionGudelIdState")
                        .HasForeignKey("ExtraFieldDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GudelIdService.Domain.Models.GudelIdState", "GudelIdState")
                        .WithMany()
                        .HasForeignKey("GudelIdStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GudelIdService.Domain.Models.GudelId", b =>
                {
                    b.HasOne("GudelIdService.Domain.Models.Pool", "Pool")
                        .WithMany("GudelIds")
                        .HasForeignKey("PoolId");

                    b.HasOne("GudelIdService.Domain.Models.GudelIdState", "State")
                        .WithMany("GudelIds")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GudelIdService.Domain.Models.GudelIdType", "Type")
                        .WithMany("GudelIds")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
