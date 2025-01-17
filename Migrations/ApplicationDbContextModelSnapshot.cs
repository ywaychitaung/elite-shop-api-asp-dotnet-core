﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using elite_shop.Data;

#nullable disable

namespace elite_shop.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("elite_shop.Models.Domains.Role", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2(7)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2(7)");

                    b.HasKey("Id");

                    b.ToTable("roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = (short)1,
                            CreatedAt = new DateTime(2024, 11, 6, 4, 37, 14, 687, DateTimeKind.Local).AddTicks(710),
                            Description = "Customers",
                            Name = "Customer",
                            UpdatedAt = new DateTime(2024, 11, 6, 4, 37, 14, 687, DateTimeKind.Local).AddTicks(740)
                        },
                        new
                        {
                            Id = (short)2,
                            CreatedAt = new DateTime(2024, 11, 6, 4, 37, 14, 687, DateTimeKind.Local).AddTicks(740),
                            Description = "Admins",
                            Name = "Admin",
                            UpdatedAt = new DateTime(2024, 11, 6, 4, 37, 14, 687, DateTimeKind.Local).AddTicks(740)
                        });
                });

            modelBuilder.Entity("elite_shop.Models.Domains.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2(7)");

                    b.Property<byte[]>("Email")
                        .IsRequired()
                        .HasColumnType("varbinary(900)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastLoginAt")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<short>("RoleId")
                        .HasColumnType("smallint");

                    b.Property<byte[]>("SaltKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2(7)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("elite_shop.Models.Domains.User", b =>
                {
                    b.HasOne("elite_shop.Models.Domains.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("elite_shop.Models.Domains.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
