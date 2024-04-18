﻿// <auto-generated />
using System;
using Baker_Server.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Baker_Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Baker_Server.Database.Entities.BakerLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("TimeStamp");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Baker_Server.Database.Entities.BunSale", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("BakedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("BunTypeId")
                        .HasColumnType("uuid");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("BunTypeId");

                    b.ToTable("SalesBun");
                });

            modelBuilder.Entity("Baker_Server.Database.Entities.BunType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<TimeSpan>("ControlTerm")
                        .HasColumnType("interval");

                    b.Property<double>("DefaultPrice")
                        .HasColumnType("double precision");

                    b.Property<int>("Name")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("SellTerm")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.ToTable("BunTypes");
                });

            modelBuilder.Entity("Baker_Server.Database.Entities.QualityMonitoring", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("BunSaleId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsThrow")
                        .HasColumnType("boolean");

                    b.Property<double?>("NextPrice")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan?>("ToNextPrice")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("BunSaleId");

                    b.HasIndex("IsThrow");

                    b.ToTable("Monitorings");
                });

            modelBuilder.Entity("Baker_Server.Database.Entities.BunSale", b =>
                {
                    b.HasOne("Baker_Server.Database.Entities.BunType", "BunType")
                        .WithMany("BunForSales")
                        .HasForeignKey("BunTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BunType");
                });

            modelBuilder.Entity("Baker_Server.Database.Entities.QualityMonitoring", b =>
                {
                    b.HasOne("Baker_Server.Database.Entities.BunSale", "BunSale")
                        .WithMany("Monitorings")
                        .HasForeignKey("BunSaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BunSale");
                });

            modelBuilder.Entity("Baker_Server.Database.Entities.BunSale", b =>
                {
                    b.Navigation("Monitorings");
                });

            modelBuilder.Entity("Baker_Server.Database.Entities.BunType", b =>
                {
                    b.Navigation("BunForSales");
                });
#pragma warning restore 612, 618
        }
    }
}
