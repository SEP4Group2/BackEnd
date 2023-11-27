﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(AppContext))]
    [Migration("20231123104104_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Model.Plant", b =>
                {
                    b.Property<int>("PlantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlantId"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("PlantPresetPresetId")
                        .HasColumnType("integer");

                    b.HasKey("PlantId");

                    b.HasIndex("PlantPresetPresetId");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("Domain.Model.PlantData", b =>
                {
                    b.Property<string>("TimeStamp")
                        .HasColumnType("text");

                    b.Property<float?>("Humidity")
                        .HasColumnType("real");

                    b.Property<float>("Moisture")
                        .HasColumnType("real");

                    b.Property<float>("TankLevel")
                        .HasColumnType("real");

                    b.Property<float?>("Temperature")
                        .HasColumnType("real");

                    b.Property<float>("UVLight")
                        .HasColumnType("real");

                    b.HasKey("TimeStamp");

                    b.ToTable("PlantData");
                });

            modelBuilder.Entity("Domain.Model.PlantPreset", b =>
                {
                    b.Property<int>("PresetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PresetId"));

                    b.Property<float>("Humidity")
                        .HasColumnType("real");

                    b.Property<float>("Moisture")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Temperature")
                        .HasColumnType("real");

                    b.Property<float>("UVLight")
                        .HasColumnType("real");

                    b.HasKey("PresetId");

                    b.ToTable("Presets");
                });

            modelBuilder.Entity("Domain.Model.Plant", b =>
                {
                    b.HasOne("Domain.Model.PlantPreset", "PlantPreset")
                        .WithMany()
                        .HasForeignKey("PlantPresetPresetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlantPreset");
                });
#pragma warning restore 612, 618
        }
    }
}
