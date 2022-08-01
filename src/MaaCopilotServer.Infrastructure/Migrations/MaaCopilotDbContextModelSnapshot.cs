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
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.ArkCharacterInfo", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("NameEntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("Profession")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Star")
                        .HasColumnType("integer");

                    b.HasKey("EntityId");

                    b.HasIndex("NameEntityId");

                    b.ToTable("ArkCharacterInfos");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.ArkI18N", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("ChineseSimplified")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ChineseTraditional")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

                    b.Property<string>("English")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Japanese")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Korean")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("EntityId");

                    b.ToTable("ArkI18Ns");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.ArkLevelData", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CatOneEntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CatThreeEntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CatTwoEntityId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LevelId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("NameEntityId")
                        .HasColumnType("uuid");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.HasKey("EntityId");

                    b.HasIndex("CatOneEntityId");

                    b.HasIndex("CatThreeEntityId");

                    b.HasIndex("CatTwoEntityId");

                    b.HasIndex("NameEntityId");

                    b.ToTable("ArkLevelData");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotOperation", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArkLevelEntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorEntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DislikeCount")
                        .HasColumnType("integer");

                    b.Property<string>("Groups")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("HotScore")
                        .HasColumnType("bigint");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("LikeCount")
                        .HasColumnType("integer");

                    b.Property<string>("MinimumRequired")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Operators")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RatingLevel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UpdateBy")
                        .HasColumnType("uuid");

                    b.Property<int>("ViewCounts")
                        .HasColumnType("integer");

                    b.HasKey("EntityId");

                    b.HasIndex("ArkLevelEntityId");

                    b.HasIndex("AuthorEntityId");

                    b.ToTable("CopilotOperations");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotOperationRating", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("OperationId")
                        .HasColumnType("uuid");

                    b.Property<string>("RatingType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("EntityId");

                    b.ToTable("CopilotOperationRatings");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotToken", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ResourceId")
                        .HasColumnType("uuid");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("ValidBefore")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("EntityId");

                    b.ToTable("CopilotTokens");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotUser", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

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

                    b.Property<Guid>("UpdateBy")
                        .HasColumnType("uuid");

                    b.Property<bool>("UserActivated")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("EntityId");

                    b.ToTable("CopilotUsers");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.PersistStorage", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("EntityId");

                    b.ToTable("PersistStorage");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.ArkCharacterInfo", b =>
                {
                    b.HasOne("MaaCopilotServer.Domain.Entities.ArkI18N", "Name")
                        .WithMany()
                        .HasForeignKey("NameEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Name");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.ArkLevelData", b =>
                {
                    b.HasOne("MaaCopilotServer.Domain.Entities.ArkI18N", "CatOne")
                        .WithMany()
                        .HasForeignKey("CatOneEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaaCopilotServer.Domain.Entities.ArkI18N", "CatThree")
                        .WithMany()
                        .HasForeignKey("CatThreeEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaaCopilotServer.Domain.Entities.ArkI18N", "CatTwo")
                        .WithMany()
                        .HasForeignKey("CatTwoEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaaCopilotServer.Domain.Entities.ArkI18N", "Name")
                        .WithMany()
                        .HasForeignKey("NameEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CatOne");

                    b.Navigation("CatThree");

                    b.Navigation("CatTwo");

                    b.Navigation("Name");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotOperation", b =>
                {
                    b.HasOne("MaaCopilotServer.Domain.Entities.ArkLevelData", "ArkLevel")
                        .WithMany()
                        .HasForeignKey("ArkLevelEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaaCopilotServer.Domain.Entities.CopilotUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ArkLevel");

                    b.Navigation("Author");
                });
#pragma warning restore 612, 618
        }
    }
}
