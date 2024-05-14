﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PianoMentor.DAL;

#nullable disable

namespace PianoMentor.DAL.Migrations
{
    [DbContext(typeof(PianoMentorDbContext))]
    partial class PianoMentorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("FileManager")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<long>", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", "FileManager");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", "FileManager");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", "FileManager");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", "FileManager");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", "FileManager");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.DataSet.BinaryData", b =>
                {
                    b.Property<long>("DataSetId")
                        .HasColumnType("bigint");

                    b.Property<int>("DataId")
                        .HasColumnType("int");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<long>("Length")
                        .HasColumnType("bigint");

                    b.HasKey("DataSetId", "DataId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("DataSetId", "DataId"));

                    b.ToTable("Binaries", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.DataSet.DataSet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("bit");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<int>("StorageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("StorageId");

                    b.ToTable("DataSets", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.DataSet.OneTimeLink", b =>
                {
                    b.Property<string>("UrlEncryptedToken")
                        .IsUnicode(false)
                        .HasColumnType("varchar(900)");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LinkExpirationTime")
                        .HasColumnType("datetime2");

                    b.HasKey("UrlEncryptedToken");

                    b.ToTable("OneTimeLinks", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.DataSet.Storage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AllowWrite")
                        .HasColumnType("bit");

                    b.Property<string>("BasePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("Storages", "FileManager");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AllowWrite = true,
                            BasePath = "C:\\PianoMentor\\DevStorage",
                            Name = "DevStorage"
                        });
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.Identity.PianoMentorUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpireTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<string>("Subtitle")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CourseId");

                    b.ToTable("Courses", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItem", b =>
                {
                    b.Property<int>("CourseItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseItemId"));

                    b.Property<long?>("AttachedDataSetId")
                        .HasColumnType("bigint");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("CourseItemTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CourseItemId");

                    b.HasIndex("AttachedDataSetId")
                        .IsUnique()
                        .HasFilter("[AttachedDataSetId] IS NOT NULL");

                    b.HasIndex("CourseId");

                    b.HasIndex("CourseItemTypeId");

                    b.ToTable("CourseItems", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItemProgressType", b =>
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

                    b.ToTable("CourseItemsProgressTypes", "FileManager");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "NotStarted"
                        },
                        new
                        {
                            Id = 2,
                            Name = "InProgress"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Completed"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Failed"
                        });
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItemType", b =>
                {
                    b.Property<int>("CourseItemTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseItemTypeId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("CourseItemTypeId");

                    b.ToTable("CourseItemTypes", "FileManager");

                    b.HasData(
                        new
                        {
                            CourseItemTypeId = 1,
                            Name = "Lecture"
                        },
                        new
                        {
                            CourseItemTypeId = 2,
                            Name = "Exercise"
                        },
                        new
                        {
                            CourseItemTypeId = 3,
                            Name = "Quiz"
                        });
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItemUserProgress", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("CourseItemId")
                        .HasColumnType("int");

                    b.Property<int>("CourseItemProgressTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CourseItemId");

                    b.HasIndex("CourseItemProgressTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("CourseItemsUsersProgresses", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseUserProgress", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("ProgressInPercent")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("CourseUsersProgresses", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestion", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionId"));

                    b.Property<long?>("AttachedDataSetId")
                        .HasColumnType("bigint");

                    b.Property<int>("CourseItemId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("QuizQuestionTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("QuestionId");

                    b.HasIndex("AttachedDataSetId")
                        .IsUnique()
                        .HasFilter("[AttachedDataSetId] IS NOT NULL");

                    b.HasIndex("CourseItemId");

                    b.HasIndex("QuizQuestionTypeId");

                    b.ToTable("QuizQuestions", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestionAnswer", b =>
                {
                    b.Property<int>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnswerId"));

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("QuizQuestionId")
                        .HasColumnType("int");

                    b.HasKey("AnswerId");

                    b.HasIndex("QuizQuestionId");

                    b.ToTable("QuizQuestionsAnswers", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestionType", b =>
                {
                    b.Property<int>("QuizQuestionTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuizQuestionTypeId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.HasKey("QuizQuestionTypeId");

                    b.ToTable("QuizQuestionsTypes", "FileManager");

                    b.HasData(
                        new
                        {
                            QuizQuestionTypeId = 1,
                            Name = "SingleChoice"
                        },
                        new
                        {
                            QuizQuestionTypeId = 2,
                            Name = "MultipleChoice"
                        },
                        new
                        {
                            QuizQuestionTypeId = 3,
                            Name = "FreeText"
                        });
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestionUserAnswerLog", b =>
                {
                    b.Property<Guid>("AnswerLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AnswerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AnsweredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("UserAnswerText")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("AnswerLogId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("QuizQuestionsUsersAnswersLogs", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Texts.ViewPagerText", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ViewPagerTexts", "FileManager");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Texts.ViewPagerTextNumberRanges", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("ViewPagerTextId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ViewPagerTextId");

                    b.ToTable("ViewPagerTextNumberRanges", "FileManager");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<long>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("PianoMentor.DAL.Domain.Identity.PianoMentorUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("PianoMentor.DAL.Domain.Identity.PianoMentorUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<long>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PianoMentor.DAL.Domain.Identity.PianoMentorUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.HasOne("PianoMentor.DAL.Domain.Identity.PianoMentorUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.DataSet.BinaryData", b =>
                {
                    b.HasOne("PianoMentor.DAL.Domain.DataSet.DataSet", "DataSet")
                        .WithMany("Binaries")
                        .HasForeignKey("DataSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataSet");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.DataSet.DataSet", b =>
                {
                    b.HasOne("PianoMentor.DAL.Domain.Identity.PianoMentorUser", "Owner")
                        .WithMany("DataSets")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PianoMentor.DAL.Domain.DataSet.Storage", "Storage")
                        .WithMany()
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItem", b =>
                {
                    b.HasOne("PianoMentor.DAL.Domain.DataSet.DataSet", "AttachedDataSet")
                        .WithOne()
                        .HasForeignKey("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItem", "AttachedDataSetId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("PianoMentor.DAL.Domain.PianoMentor.Courses.Course", "Course")
                        .WithMany("CourseItems")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItemType", "CourseItemType")
                        .WithMany()
                        .HasForeignKey("CourseItemTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AttachedDataSet");

                    b.Navigation("Course");

                    b.Navigation("CourseItemType");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItemUserProgress", b =>
                {
                    b.HasOne("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItem", "CourseItem")
                        .WithMany()
                        .HasForeignKey("CourseItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItemProgressType", "CourseItemProgressType")
                        .WithMany()
                        .HasForeignKey("CourseItemProgressTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PianoMentor.DAL.Domain.Identity.PianoMentorUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CourseItem");

                    b.Navigation("CourseItemProgressType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseUserProgress", b =>
                {
                    b.HasOne("PianoMentor.DAL.Domain.PianoMentor.Courses.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PianoMentor.DAL.Domain.Identity.PianoMentorUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestion", b =>
                {
                    b.HasOne("PianoMentor.DAL.Domain.DataSet.DataSet", "AttachedDataSet")
                        .WithOne()
                        .HasForeignKey("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestion", "AttachedDataSetId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("PianoMentor.DAL.Domain.PianoMentor.Courses.CourseItem", "CourseItem")
                        .WithMany()
                        .HasForeignKey("CourseItemId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestionType", "QuizQuestionType")
                        .WithMany()
                        .HasForeignKey("QuizQuestionTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AttachedDataSet");

                    b.Navigation("CourseItem");

                    b.Navigation("QuizQuestionType");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestionAnswer", b =>
                {
                    b.HasOne("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestion", "QuizQuestions")
                        .WithMany("QuizQuestionsAnswers")
                        .HasForeignKey("QuizQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuizQuestions");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestionUserAnswerLog", b =>
                {
                    b.HasOne("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestionAnswer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestion", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PianoMentor.DAL.Domain.Identity.PianoMentorUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Texts.ViewPagerTextNumberRanges", b =>
                {
                    b.HasOne("PianoMentor.DAL.Models.PianoMentor.Texts.ViewPagerText", null)
                        .WithMany("ViewPagerTextNumberRanges")
                        .HasForeignKey("ViewPagerTextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.DataSet.DataSet", b =>
                {
                    b.Navigation("Binaries");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.Identity.PianoMentorUser", b =>
                {
                    b.Navigation("DataSets");
                });

            modelBuilder.Entity("PianoMentor.DAL.Domain.PianoMentor.Courses.Course", b =>
                {
                    b.Navigation("CourseItems");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Quizzes.QuizQuestion", b =>
                {
                    b.Navigation("QuizQuestionsAnswers");
                });

            modelBuilder.Entity("PianoMentor.DAL.Models.PianoMentor.Texts.ViewPagerText", b =>
                {
                    b.Navigation("ViewPagerTextNumberRanges");
                });
#pragma warning restore 612, 618
        }
    }
}
