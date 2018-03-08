﻿// <auto-generated />
using back_end_api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace backendapi.Migrations
{
    [DbContext(typeof(back_end_apiContext))]
    partial class back_end_apiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("back_end_api.DailyCheck", b =>
                {
                    b.Property<int>("DailyCheckId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

                    b.Property<string>("NeedSupport")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.Property<string>("actions")
                        .IsRequired();

                    b.Property<string>("feeling")
                        .IsRequired();

                    b.HasKey("DailyCheckId");

                    b.HasIndex("UserId");

                    b.ToTable("DailyCheck");
                });

            modelBuilder.Entity("back_end_api.Goals", b =>
                {
                    b.Property<int>("GoalsId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

                    b.Property<DateTime>("DateDue");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.Property<bool>("isCompleted");

                    b.HasKey("GoalsId");

                    b.HasIndex("UserId");

                    b.ToTable("Goals");
                });

            modelBuilder.Entity("back_end_api.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("back_end_api.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<bool>("isStaff");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("back_end_api.DailyCheck", b =>
                {
                    b.HasOne("back_end_api.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("back_end_api.Goals", b =>
                {
                    b.HasOne("back_end_api.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("back_end_api.Post", b =>
                {
                    b.HasOne("back_end_api.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
