﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Stazo.API.Data;
using Stazo.API.Models;
using System;

namespace Stazo.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180319065055_ChangedReviewScore")]
    partial class ChangedReviewScore
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Stazo.API.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Stazo.API.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Stazo.API.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<bool>("AdminApproved");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<byte>("Market");

                    b.Property<string>("Name");

                    b.Property<int?>("PhotoId");

                    b.Property<string>("State");

                    b.Property<int>("Zip");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Stazo.API.Models.LocationProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LocationId");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("ProductId");

                    b.ToTable("LocationProducts");
                });

            modelBuilder.Entity("Stazo.API.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ReviewId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("ReviewId");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("Stazo.API.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AdminApproved");

                    b.Property<int?>("BrandId");

                    b.Property<int>("CategoryId");

                    b.Property<DateTime>("Created");

                    b.Property<byte>("MarketType");

                    b.Property<string>("Name");

                    b.Property<int?>("PhotoId");

                    b.Property<decimal>("StarRating");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PhotoId");

                    b.HasIndex("UserId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Stazo.API.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("LocationId");

                    b.Property<int>("ProductId");

                    b.Property<int>("StarScore");

                    b.Property<string>("Text");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Stazo.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarUrl");

                    b.Property<string>("Email");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("SocialProvider");

                    b.Property<int>("UserRole");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Stazo.API.Models.Location", b =>
                {
                    b.HasOne("Stazo.API.Models.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("Stazo.API.Models.LocationProduct", b =>
                {
                    b.HasOne("Stazo.API.Models.Location", "Location")
                        .WithMany("LocationProducts")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Stazo.API.Models.Product", "Product")
                        .WithMany("LocationProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Stazo.API.Models.Photo", b =>
                {
                    b.HasOne("Stazo.API.Models.Review")
                        .WithMany("Photos")
                        .HasForeignKey("ReviewId");
                });

            modelBuilder.Entity("Stazo.API.Models.Product", b =>
                {
                    b.HasOne("Stazo.API.Models.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId");

                    b.HasOne("Stazo.API.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Stazo.API.Models.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");

                    b.HasOne("Stazo.API.Models.User")
                        .WithMany("ProductsToTry")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Stazo.API.Models.Review", b =>
                {
                    b.HasOne("Stazo.API.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Stazo.API.Models.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Stazo.API.Models.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
