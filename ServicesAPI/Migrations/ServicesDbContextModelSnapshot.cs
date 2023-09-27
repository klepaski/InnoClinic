﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServicesAPI.Context;

#nullable disable

namespace ServicesAPI.Migrations
{
    [DbContext(typeof(ServicesDbContext))]
    partial class ServicesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ServicesAPI.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SpecializationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SpecializationId");

                    b.ToTable("Services");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            IsActive = true,
                            Price = 10f,
                            ServiceName = "Pressure measurement",
                            SpecializationId = 1
                        });
                });

            modelBuilder.Entity("ServicesAPI.Models.ServiceCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimeSlotSize")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ServiceCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "Consultations",
                            TimeSlotSize = 10
                        },
                        new
                        {
                            Id = 2,
                            CategoryName = "Diagnostics",
                            TimeSlotSize = 5
                        },
                        new
                        {
                            Id = 3,
                            CategoryName = "Analyzes",
                            TimeSlotSize = 20
                        });
                });

            modelBuilder.Entity("ServicesAPI.Models.Specialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("SpecializationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Specializations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsActive = true,
                            SpecializationName = "Cardio"
                        },
                        new
                        {
                            Id = 2,
                            IsActive = false,
                            SpecializationName = "Oncology"
                        },
                        new
                        {
                            Id = 3,
                            IsActive = true,
                            SpecializationName = "Ophtalmology"
                        });
                });

            modelBuilder.Entity("ServicesAPI.Models.Service", b =>
                {
                    b.HasOne("ServicesAPI.Models.ServiceCategory", "ServiceCategory")
                        .WithMany("Services")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicesAPI.Models.Specialization", "Specialization")
                        .WithMany("Services")
                        .HasForeignKey("SpecializationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceCategory");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("ServicesAPI.Models.ServiceCategory", b =>
                {
                    b.Navigation("Services");
                });

            modelBuilder.Entity("ServicesAPI.Models.Specialization", b =>
                {
                    b.Navigation("Services");
                });
#pragma warning restore 612, 618
        }
    }
}
