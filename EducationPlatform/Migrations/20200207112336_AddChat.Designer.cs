﻿// <auto-generated />
using System;
using EducationPlatform.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EducationPlatform.Migrations
{
    [DbContext(typeof(EducationPlatformContext))]
    [Migration("20200207112336_AddChat")]
    partial class AddChat
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetRoles", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("([NormalizedName] IS NOT NULL)");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetUserLogins", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetUserRoles", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetUserTokens", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetUsers", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool?>("IsBanned");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("MiddleName");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("(N'f1f7f444-fb7b-4469-9979-0720852f2300')");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("([NormalizedUserName] IS NOT NULL)");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Chat", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.ToTable("Chat");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<int>("SubjectId");

                    b.Property<string>("TeacherId")
                        .IsRequired();

                    b.HasKey("CourseId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.CourseModule", b =>
                {
                    b.Property<int>("CourseModuleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseId");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("date");

                    b.Property<int>("ModuleId");

                    b.HasKey("CourseModuleId");

                    b.HasIndex("CourseId");

                    b.HasIndex("ModuleId");

                    b.ToTable("CourseModule");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.CourseStudent", b =>
                {
                    b.Property<int>("CourseStudentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CourseId");

                    b.Property<int?>("StudentId");

                    b.HasKey("CourseStudentId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("CourseStudent");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Mark", b =>
                {
                    b.Property<int>("MarkId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseModuleId");

                    b.Property<int?>("LabMark");

                    b.Property<int>("StudentId");

                    b.Property<int?>("TestMark");

                    b.HasKey("MarkId");

                    b.HasIndex("CourseModuleId");

                    b.HasIndex("StudentId");

                    b.ToTable("Mark");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Module", b =>
                {
                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(256);

                    b.Property<bool>("HasLab");

                    b.Property<bool>("HasTest");

                    b.Property<int?>("MaxLabMark");

                    b.Property<int?>("MaxTestMark");

                    b.Property<int?>("MinLabMark");

                    b.Property<int?>("MinTestMark");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<int?>("SubjectId");

                    b.HasKey("ModuleId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.OnlineList", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.ToTable("OnlineList");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Faculty")
                        .HasMaxLength(50);

                    b.Property<bool?>("HasAccess")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("Skills")
                        .HasMaxLength(500);

                    b.Property<int?>("StudyYear");

                    b.Property<string>("University")
                        .HasMaxLength(50);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("StudentId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasName("UQ__Student__1788CC4D8BA74B7F");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("SubjectId");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetRoleClaims", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.AspNetRoles", "Role")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetUserClaims", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.AspNetUsers", "User")
                        .WithMany("AspNetUserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetUserLogins", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.AspNetUsers", "User")
                        .WithMany("AspNetUserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetUserRoles", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.AspNetRoles", "Role")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EducationPlatform.Models.EntityModels.AspNetUsers", "User")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.AspNetUserTokens", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.AspNetUsers", "User")
                        .WithMany("AspNetUserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Course", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.Subject", "Subject")
                        .WithMany("Course")
                        .HasForeignKey("SubjectId")
                        .HasConstraintName("FK_Course_SubjectId");

                    b.HasOne("EducationPlatform.Models.EntityModels.AspNetUsers", "Teacher")
                        .WithMany("Course")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("fk_Course_Teacher_AspNetUsers");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.CourseModule", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.Course", "Course")
                        .WithMany("CourseModule")
                        .HasForeignKey("CourseId")
                        .HasConstraintName("fk_CourseModule_Course");

                    b.HasOne("EducationPlatform.Models.EntityModels.Module", "Module")
                        .WithMany("CourseModule")
                        .HasForeignKey("ModuleId")
                        .HasConstraintName("fk_CourseModule_Module");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.CourseStudent", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.Course", "Course")
                        .WithMany("CourseStudent")
                        .HasForeignKey("CourseId")
                        .HasConstraintName("fk_CourseStudent_Course");

                    b.HasOne("EducationPlatform.Models.EntityModels.Student", "Student")
                        .WithMany("CourseStudent")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("fk_CourseStudent_Student");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Mark", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.CourseModule", "CourseModule")
                        .WithMany("Mark")
                        .HasForeignKey("CourseModuleId")
                        .HasConstraintName("fk_Mark_CourseModule");

                    b.HasOne("EducationPlatform.Models.EntityModels.Student", "Student")
                        .WithMany("Mark")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("fk_Mark_Student");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Module", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.Subject", "Subject")
                        .WithMany("Module")
                        .HasForeignKey("SubjectId")
                        .HasConstraintName("fk_Module_Subject");
                });

            modelBuilder.Entity("EducationPlatform.Models.EntityModels.Student", b =>
                {
                    b.HasOne("EducationPlatform.Models.EntityModels.AspNetUsers", "User")
                        .WithOne("Student")
                        .HasForeignKey("EducationPlatform.Models.EntityModels.Student", "UserId")
                        .HasConstraintName("fk_Student_AspNetUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
