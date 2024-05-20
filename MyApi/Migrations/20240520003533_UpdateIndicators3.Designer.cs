﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyApi.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240520003533_UpdateIndicators3")]
    partial class UpdateIndicators3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyApi.Models.AnalysType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Format")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AnalysTypes");
                });

            modelBuilder.Entity("MyApi.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("MyApi.Models.Direction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AcceptedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("AcceptedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("AnalysTypeFormat")
                        .HasColumnType("integer");

                    b.Property<int>("AnalysTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("AnalysTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("BioMaterialCount")
                        .HasColumnType("integer");

                    b.Property<string>("BioMaterialType")
                        .HasColumnType("text");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<bool>("Cito")
                        .HasColumnType("boolean");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("text");

                    b.Property<string>("DirectionStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DirectionStatusId")
                        .HasColumnType("integer");

                    b.Property<string>("DoctorBiologFio")
                        .HasColumnType("text");

                    b.Property<string>("DoctorFeldsherLaborantFio")
                        .HasColumnType("text");

                    b.Property<string>("DoctorLabDiagnosticFio")
                        .HasColumnType("text");

                    b.Property<bool>("HasAnyResults")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("LaborantComment")
                        .HasColumnType("text");

                    b.Property<string>("Laboratory")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("LaboratoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("NumberByJournal")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("OnDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PatientFullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ReadyDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RequestedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SampleNumber")
                        .HasColumnType("text");

                    b.Property<DateTime?>("SamplingDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SamplingDateStr")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SamplingDoctorFio")
                        .HasColumnType("text");

                    b.Property<string>("Sid")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnalysTypeId")
                        .HasDatabaseName("IX_Directions_AnalysTypeId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("LaboratoryId")
                        .HasDatabaseName("IX_Directions_LaboratoryId");

                    b.HasIndex("PatientId")
                        .HasDatabaseName("IX_Directions_PatientId");

                    b.ToTable("Directions");
                });

            modelBuilder.Entity("MyApi.Models.DirectionStatusHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DirectionId")
                        .HasColumnType("integer");

                    b.Property<int>("DirectionStatusId")
                        .HasColumnType("integer");

                    b.Property<string>("UserFio")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DirectionId");

                    b.ToTable("DirectionStatusHistories");
                });

            modelBuilder.Entity("MyApi.Models.Indicator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DirectionId")
                        .HasColumnType("integer");

                    b.Property<List<string>>("DynamicValues")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GroupOrderNumber")
                        .HasColumnType("integer");

                    b.Property<bool>("IsAdditional")
                        .HasColumnType("boolean");

                    b.Property<bool?>("IsInReference")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsNormExist")
                        .HasColumnType("boolean");

                    b.Property<double?>("MaxStandardValue")
                        .HasColumnType("double precision");

                    b.Property<double?>("MinStandardValue")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("PossibleStringValues")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ResultStr")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double?>("ResultVal")
                        .HasColumnType("double precision");

                    b.Property<int>("SortNumber")
                        .HasColumnType("integer");

                    b.Property<List<string>>("TextStandards")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Units")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DirectionId");

                    b.ToTable("Indicators");
                });

            modelBuilder.Entity("MyApi.Models.IndicatorGroup", b =>
                {
                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("IndicatorGroup");
                });

            modelBuilder.Entity("MyApi.Models.LaboratoryData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LaboratoryDatas");
                });

            modelBuilder.Entity("MyApi.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Sex")
                        .HasColumnType("integer");

                    b.Property<string>("SexDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("MyApi.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyApi.Models.Direction", b =>
                {
                    b.HasOne("MyApi.Models.AnalysType", "AnalysType")
                        .WithMany("Directions")
                        .HasForeignKey("AnalysTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyApi.Models.Department", "Department")
                        .WithMany("Directions")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MyApi.Models.LaboratoryData", "LaboratoryData")
                        .WithMany("Directions")
                        .HasForeignKey("LaboratoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("MyApi.Models.Patient", "Patient")
                        .WithMany("Directions")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnalysType");

                    b.Navigation("Department");

                    b.Navigation("LaboratoryData");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("MyApi.Models.DirectionStatusHistory", b =>
                {
                    b.HasOne("MyApi.Models.Direction", "Direction")
                        .WithMany("DirectionStatusHistory")
                        .HasForeignKey("DirectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Direction");
                });

            modelBuilder.Entity("MyApi.Models.Indicator", b =>
                {
                    b.HasOne("MyApi.Models.Direction", "Direction")
                        .WithMany("Indicators")
                        .HasForeignKey("DirectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Direction");
                });

            modelBuilder.Entity("MyApi.Models.AnalysType", b =>
                {
                    b.Navigation("Directions");
                });

            modelBuilder.Entity("MyApi.Models.Department", b =>
                {
                    b.Navigation("Directions");
                });

            modelBuilder.Entity("MyApi.Models.Direction", b =>
                {
                    b.Navigation("DirectionStatusHistory");

                    b.Navigation("Indicators");
                });

            modelBuilder.Entity("MyApi.Models.LaboratoryData", b =>
                {
                    b.Navigation("Directions");
                });

            modelBuilder.Entity("MyApi.Models.Patient", b =>
                {
                    b.Navigation("Directions");
                });
#pragma warning restore 612, 618
        }
    }
}
