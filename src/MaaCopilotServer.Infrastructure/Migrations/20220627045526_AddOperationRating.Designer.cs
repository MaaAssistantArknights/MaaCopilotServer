﻿// <auto-generated />
using System;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    [DbContext(typeof(MaaCopilotDbContext))]
    [Migration("20220627045526_AddOperationRating")]
    partial class AddOperationRating
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
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

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FavoriteCount")
                        .HasColumnType("integer");

                    b.Property<string>("Groups")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("MinimumRequired")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Operators")
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

                    b.Property<Guid>("UpdateBy")
                        .HasColumnType("uuid");

                    b.Property<int>("ViewCounts")
                        .HasColumnType("integer");

                    b.HasKey("EntityId");

                    b.HasIndex("AuthorEntityId");

                    b.ToTable("CopilotOperations");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotOperationComment", b =>
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

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OperationEntityId")
                        .HasColumnType("uuid");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<Guid>("ReplyTo")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserEntityId")
                        .HasColumnType("uuid");

                    b.HasKey("EntityId");

                    b.HasIndex("OperationEntityId");

                    b.HasIndex("UserEntityId");

                    b.ToTable("CopilotOperationComments");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotOperationRating", b =>
                {
                    b.Property<Guid>("EntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

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

                    b.ToTable("CopilotOperationRating");
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

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotUserFavorite", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uuid");

                    b.Property<string>("FavoriteName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("OperationIds")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UpdateBy")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserEntityId")
                        .HasColumnType("uuid");

                    b.HasKey("EntityId");

                    b.HasIndex("UserEntityId");

                    b.ToTable("CopilotUserFavorites");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.MapFavoriteOperation", b =>
                {
                    b.Property<Guid>("FavoritesEntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OperationsEntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreateBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<Guid?>("DeleteBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.HasKey("FavoritesEntityId", "OperationsEntityId");

                    b.HasIndex("OperationsEntityId");

                    b.ToTable("Map_Favorite_Operation", (string)null);
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

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotOperationComment", b =>
                {
                    b.HasOne("MaaCopilotServer.Domain.Entities.CopilotOperation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaaCopilotServer.Domain.Entities.CopilotUser", "User")
                        .WithMany()
                        .HasForeignKey("UserEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Operation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotUserFavorite", b =>
                {
                    b.HasOne("MaaCopilotServer.Domain.Entities.CopilotUser", "User")
                        .WithMany("UserFavorites")
                        .HasForeignKey("UserEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.MapFavoriteOperation", b =>
                {
                    b.HasOne("MaaCopilotServer.Domain.Entities.CopilotUserFavorite", "Favorites")
                        .WithMany()
                        .HasForeignKey("FavoritesEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FavOper_Fav_FavEntityId");

                    b.HasOne("MaaCopilotServer.Domain.Entities.CopilotOperation", "Operations")
                        .WithMany()
                        .HasForeignKey("OperationsEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FavOper_Oper_OperEntityId");

                    b.Navigation("Favorites");

                    b.Navigation("Operations");
                });

            modelBuilder.Entity("MaaCopilotServer.Domain.Entities.CopilotUser", b =>
                {
                    b.Navigation("UserFavorites");
                });
#pragma warning restore 612, 618
        }
    }
}
