﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Context;

#nullable disable

namespace api.Migrations
{
    [DbContext(typeof(apiContext))]
    [Migration("20241205135016_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("api.Model.AdminModel.CondolenceMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeceasedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<string>("MessageText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("condolenceMessages");
                });

            modelBuilder.Entity("api.Model.AdminModel.SmsTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("smsTemplates");
                });

            modelBuilder.Entity("api.Model.AdminModel.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("api.Model.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AudioFileLimit")
                        .HasColumnType("int");

                    b.Property<bool>("BarcodeEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("CondolenceMessageEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("DisplayEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("File360DegreeEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("ImageCount")
                        .HasColumnType("int");

                    b.Property<bool>("LocationAndNavigationEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("NotificationCount")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RenewalFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("SharingEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("TemplateSelectionEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("UpdateCapabilityEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("ValidityPeriod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VideoCount")
                        .HasColumnType("int");

                    b.Property<bool>("VisitorContentSubmissionEnabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("packages");
                });

            modelBuilder.Entity("api.Model.Shahid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("BirthBorn")
                        .HasColumnType("date");

                    b.Property<DateOnly>("BirthDead")
                        .HasColumnType("date");

                    b.Property<string>("CauseOfMartyrdom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ghesmat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gorooh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastResponsibility")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Memories")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Niru")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Operations")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrls")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlaceDead")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Poem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Responsibilities")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VideoUrls")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VoiceUrls")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Will")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Yegan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("shahids");
                });

            modelBuilder.Entity("api.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("PackageId")
                        .HasColumnType("int");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("phoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PackageId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("api.Model.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsExpired")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("userTokens");
                });

            modelBuilder.Entity("api.Model.User", b =>
                {
                    b.HasOne("api.Model.Package", "Package")
                        .WithMany("Users")
                        .HasForeignKey("PackageId");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("api.Model.Package", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
