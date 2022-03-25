﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Struktura_drzewiasta.Models;

#nullable disable

namespace Struktura_drzewiasta.Migrations
{
    [DbContext(typeof(TreeDbContext))]
    partial class TreeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Struktura_drzewiasta.Models.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("NodeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("Struktura_drzewiasta.Models.Node", b =>
                {
                    b.HasOne("Struktura_drzewiasta.Models.Node", null)
                        .WithMany("Children")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Struktura_drzewiasta.Models.Node", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
