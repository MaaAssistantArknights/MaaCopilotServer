﻿// <auto-generated />
using System;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    [DbContext(typeof(MaaCopilotDbContext))]
    partial class MaaCopilotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotOperation", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorEntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Downloads")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("MinimumRequired")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StageName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("EntityId");

                    b.HasIndex("AuthorEntityId");

                    b.ToTable("CopilotOperations");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotUser", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.HasKey("EntityId");

                    b.ToTable("CopilotUsers");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotOperation", b =>
                {
                    b.HasOne("MaaCopilotServer.Domain.Entities.CopilotUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });
#pragma warning restore 612, 618
        }
    }
}