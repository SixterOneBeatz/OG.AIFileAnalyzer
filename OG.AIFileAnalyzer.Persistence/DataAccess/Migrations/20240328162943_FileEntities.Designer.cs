﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OG.AIFileAnalyzer.Persistence.DataAccess.Contexts;

#nullable disable

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Migrations
{
    [DbContext(typeof(AIFileAnalyzerDbContext))]
    [Migration("20240328162943_FileEntities")]
    partial class FileEntities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OG.AIFileAnalyzer.Common.Entities.FileAnaysisEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FileEntityId")
                        .HasColumnType("int");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FileEntityId");

                    b.ToTable("FileAnalyses");
                });

            modelBuilder.Entity("OG.AIFileAnalyzer.Common.Entities.FileEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("SHA256")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SHA256")
                        .IsUnique()
                        .HasFilter("[SHA256] IS NOT NULL");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("OG.AIFileAnalyzer.Common.Entities.LogEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ActionType")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("OG.AIFileAnalyzer.Common.Entities.FileAnaysisEntity", b =>
                {
                    b.HasOne("OG.AIFileAnalyzer.Common.Entities.FileEntity", null)
                        .WithMany("Anaysis")
                        .HasForeignKey("FileEntityId");
                });

            modelBuilder.Entity("OG.AIFileAnalyzer.Common.Entities.FileEntity", b =>
                {
                    b.Navigation("Anaysis");
                });
#pragma warning restore 612, 618
        }
    }
}
