﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using lab2;

#nullable disable

namespace lab2.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20221117073506_changeReservedAndLeasedFieldsDataType")]
    partial class changeReservedAndLeasedFieldsDataType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("lab2.Models.Book", b =>
                {
                    b.Property<int>("bookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("author")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("date")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("leased")
                        .HasColumnType("date");

                    b.Property<string>("publisher")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateOnly?>("reserved")
                        .HasColumnType("date");

                    b.Property<DateTime?>("rowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("userId")
                        .HasColumnType("int");

                    b.HasKey("bookId");

                    b.HasIndex("userId");

                    b.ToTable("books");
                });

            modelBuilder.Entity("lab2.Models.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("pwd")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("rowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("userId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("lab2.Models.Book", b =>
                {
                    b.HasOne("lab2.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId");

                    b.Navigation("user");
                });
#pragma warning restore 612, 618
        }
    }
}
