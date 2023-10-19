﻿// <auto-generated />
using System;
using AppointmentsAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AppointmentsAPI.Migrations
{
    [DbContext(typeof(AppointmentDbContext))]
    [Migration("20231010094917_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AppointmentsAPI.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<string>("DoctorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PatientBirthday")
                        .HasColumnType("datetime2");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("PatientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientPhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("ServicePrice")
                        .HasColumnType("real");

                    b.Property<int>("SpecializationId")
                        .HasColumnType("int");

                    b.Property<string>("SpecializationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("AppointmentsAPI.Models.Result", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<string>("Complaints")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Conclusion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Recommendations")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId")
                        .IsUnique();

                    b.ToTable("Results");
                });

            modelBuilder.Entity("AppointmentsAPI.Models.Result", b =>
                {
                    b.HasOne("AppointmentsAPI.Models.Appointment", "Appointment")
                        .WithOne("Result")
                        .HasForeignKey("AppointmentsAPI.Models.Result", "AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");
                });

            modelBuilder.Entity("AppointmentsAPI.Models.Appointment", b =>
                {
                    b.Navigation("Result")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
