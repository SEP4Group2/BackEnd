﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(AppContext))]
    partial class AppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Model.Device", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DeviceId"));

                    b.Property<int?>("PlantId")
                        .HasColumnType("integer");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.HasKey("DeviceId");

                    b.HasIndex("PlantId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Domain.Model.Plant", b =>
                {
                    b.Property<int>("PlantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlantId"));

                    b.Property<int>("IconId")
                        .HasColumnType("integer");

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

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("PlantId");

                    b.HasIndex("PlantPresetPresetId");

                    b.HasIndex("UserId");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("Domain.Model.PlantData", b =>
                {
                    b.Property<string>("TimeStamp")
                        .HasColumnType("text");

                    b.Property<float>("Humidity")
                        .HasColumnType("real");

                    b.Property<float>("Moisture")
                        .HasColumnType("real");

                    b.Property<int>("PlantDeviceDeviceId")
                        .HasColumnType("integer");

                    b.Property<float>("TankLevel")
                        .HasColumnType("real");

                    b.Property<float>("Temperature")
                        .HasColumnType("real");

                    b.Property<float>("UVLight")
                        .HasColumnType("real");

                    b.HasKey("TimeStamp");

                    b.HasIndex("PlantDeviceDeviceId");

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

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("PresetId");

                    b.ToTable("Presets");
                });

            modelBuilder.Entity("Domain.Model.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Model.Device", b =>
                {
                    b.HasOne("Domain.Model.Plant", "Plant")
                        .WithMany()
                        .HasForeignKey("PlantId");

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("Domain.Model.Plant", b =>
                {
                    b.HasOne("Domain.Model.PlantPreset", "PlantPreset")
                        .WithMany()
                        .HasForeignKey("PlantPresetPresetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlantPreset");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Model.PlantData", b =>
                {
                    b.HasOne("Domain.Model.Device", "PlantDevice")
                        .WithMany()
                        .HasForeignKey("PlantDeviceDeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlantDevice");
                });
#pragma warning restore 612, 618
        }
    }
}
