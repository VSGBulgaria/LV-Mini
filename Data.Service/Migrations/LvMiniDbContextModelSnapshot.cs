﻿// <auto-generated />
using Data.Service.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Data.Service.Migrations
{
    [DbContext(typeof(LvMiniDbContext))]
    partial class LvMiniDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Data.Service.Core.Entities.Account", b =>
                {
                    b.Property<int>("IDAccount")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountCategoryCode")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("AccountStatusCode")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<int>("IDProduct");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.HasKey("IDAccount");

                    b.ToTable("Account","IbClue");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.Loan", b =>
                {
                    b.Property<int>("IDLoan")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateLoanRequestReceived");

                    b.Property<DateTime>("DecisionDate");

                    b.Property<decimal>("ExpectedFundingAtClosing");

                    b.Property<int>("IDAccount");

                    b.Property<bool>("IsLoanRequest");

                    b.Property<DateTime>("LoanDate");

                    b.Property<decimal>("NewMoney");

                    b.Property<DateTime>("ProposedCloseDate");

                    b.HasKey("IDLoan");

                    b.ToTable("Loan","IbClue");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action")
                        .IsRequired();

                    b.Property<DateTime>("Time");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Logs","admin");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.Product", b =>
                {
                    b.Property<int>("IDProduct")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsHidden");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<byte>("ProductType");

                    b.HasKey("IDProduct");

                    b.ToTable("Product","IbClue");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.ProductGroup", b =>
                {
                    b.Property<int>("IDProductGroup")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("IDProductGroup");

                    b.ToTable("ProductGroup","admin");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.ProductGroupProduct", b =>
                {
                    b.Property<int>("IDProduct");

                    b.Property<int>("IDProductGroup");

                    b.HasKey("IDProduct", "IDProductGroup");

                    b.HasIndex("IDProductGroup");

                    b.ToTable("ProductGroupProduct");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.HasKey("TeamId");

                    b.HasIndex("TeamName")
                        .IsUnique();

                    b.ToTable("Teams","admin");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.User", b =>
                {
                    b.Property<string>("SubjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("SubjectId");

                    b.HasIndex("Username", "Email")
                        .IsUnique();

                    b.ToTable("Users","admin");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.UserClaim", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("SubjectId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("UserClaims","admin");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.UserLogin", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("ProviderKey")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("SubjectId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("UserLogins","admin");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.UserTeam", b =>
                {
                    b.Property<int>("TeamId");

                    b.Property<string>("UserId");

                    b.HasKey("TeamId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersTeams","admin");
                });

            modelBuilder.Entity("Data.Service.Core.Entities.ProductGroupProduct", b =>
                {
                    b.HasOne("Data.Service.Core.Entities.Product", "Product")
                        .WithMany("ProductGroups")
                        .HasForeignKey("IDProduct")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Service.Core.Entities.ProductGroup", "ProductGroup")
                        .WithMany("Products")
                        .HasForeignKey("IDProductGroup")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Service.Core.Entities.UserClaim", b =>
                {
                    b.HasOne("Data.Service.Core.Entities.User")
                        .WithMany("Claims")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Service.Core.Entities.UserLogin", b =>
                {
                    b.HasOne("Data.Service.Core.Entities.User")
                        .WithMany("Logins")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Service.Core.Entities.UserTeam", b =>
                {
                    b.HasOne("Data.Service.Core.Entities.Team", "Team")
                        .WithMany("UsersTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Data.Service.Core.Entities.User", "User")
                        .WithMany("UsersTeams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
