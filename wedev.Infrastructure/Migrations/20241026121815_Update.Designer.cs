﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using wedev.Infrastructure;

#nullable disable

namespace wedev.Infrastructure.Migrations
{
    [DbContext(typeof(GlobalDbContext))]
    [Migration("20241026121815_Update")]
    partial class Update
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("wedev.Domain.App", b =>
                {
                    b.Property<Guid>("AppId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AppId");

                    b.HasIndex("TenantId");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("wedev.Domain.AuditLog", b =>
                {
                    b.Property<Guid>("AuditLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ChangedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AuditLogId");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("wedev.Domain.Database", b =>
                {
                    b.Property<Guid>("DatabaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EnvironmentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("DatabaseId");

                    b.HasIndex("EnvironmentId");

                    b.HasIndex("ServerId");

                    b.ToTable("Databases");
                });

            modelBuilder.Entity("wedev.Domain.DeploymentEnvironment", b =>
                {
                    b.Property<Guid>("EnvironmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("EnvironmentId");

                    b.ToTable("DeploymentEnvironments");
                });

            modelBuilder.Entity("wedev.Domain.Server", b =>
                {
                    b.Property<Guid>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalParams")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EnvironmentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("ServerId");

                    b.HasIndex("EnvironmentId");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("wedev.Domain.Tenant", b =>
                {
                    b.Property<Guid>("TenantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("DatabaseId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TenantId");

                    b.HasIndex("DatabaseId");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("wedev.Domain.App", b =>
                {
                    b.HasOne("wedev.Domain.Tenant", null)
                        .WithMany("Apps")
                        .HasForeignKey("TenantId");
                });

            modelBuilder.Entity("wedev.Domain.Database", b =>
                {
                    b.HasOne("wedev.Domain.DeploymentEnvironment", "Environment")
                        .WithMany("Databases")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("wedev.Domain.Server", "Server")
                        .WithMany("Databases")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Environment");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("wedev.Domain.Server", b =>
                {
                    b.HasOne("wedev.Domain.DeploymentEnvironment", "Environment")
                        .WithMany("Servers")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Environment");
                });

            modelBuilder.Entity("wedev.Domain.Tenant", b =>
                {
                    b.HasOne("wedev.Domain.Database", "Database")
                        .WithMany("Tenants")
                        .HasForeignKey("DatabaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Database");
                });

            modelBuilder.Entity("wedev.Domain.Database", b =>
                {
                    b.Navigation("Tenants");
                });

            modelBuilder.Entity("wedev.Domain.DeploymentEnvironment", b =>
                {
                    b.Navigation("Databases");

                    b.Navigation("Servers");
                });

            modelBuilder.Entity("wedev.Domain.Server", b =>
                {
                    b.Navigation("Databases");
                });

            modelBuilder.Entity("wedev.Domain.Tenant", b =>
                {
                    b.Navigation("Apps");
                });
#pragma warning restore 612, 618
        }
    }
}
