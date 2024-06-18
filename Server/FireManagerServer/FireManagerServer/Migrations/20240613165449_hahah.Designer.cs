﻿// <auto-generated />
using System;
using FireManagerServer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FireManagerServer.Migrations
{
    [DbContext(typeof(FireDbContext))]
    [Migration("20240613165449_hahah")]
    partial class hahah
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FireManagerServer.Database.Entity.Apartment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("BuldingId")
                        .HasColumnType("varchar(95)");

                    b.Property<DateTime?>("DateCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Desc")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsFire")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("BuldingId");

                    b.ToTable("Apartments");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.ApartmentNeighbour", b =>
                {
                    b.Property<string>("ApartmentId")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("NeighbourId")
                        .HasColumnType("varchar(95)");

                    b.HasKey("ApartmentId", "NeighbourId");

                    b.HasIndex("NeighbourId");

                    b.ToTable("ApartmentNeighbours");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Building", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95)");

                    b.Property<DateTime?>("DateCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Desc")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(95)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.DeviceEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95)");

                    b.Property<DateTime?>("DateCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("InitValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ModuleId")
                        .IsRequired()
                        .HasColumnType("varchar(95)");

                    b.Property<string>("Port")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.HistoryData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95)");

                    b.Property<DateTime>("DateRetrieve")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("varchar(95)");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("DeviceType")
                        .HasColumnType("int");

                    b.Property<bool?>("IsSuccess")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("UserId");

                    b.ToTable("HistoryDatas");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Module", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("ApartmentId")
                        .HasColumnType("varchar(95)");

                    b.Property<DateTime?>("DateCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Desc")
                        .HasColumnType("longtext");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool?>("Status")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(95)");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId");

                    b.HasIndex("UserId");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95)");

                    b.Property<DateTime?>("DateCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RoleName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.RuleEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ModuleId")
                        .IsRequired()
                        .HasColumnType("varchar(95)");

                    b.Property<int>("TypeRule")
                        .HasColumnType("int");

                    b.Property<bool>("isActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("isFireRule")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.ToTable("RuleEntity");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.TopicThreshhold", b =>
                {
                    b.Property<string>("DeviceId")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("RuleId")
                        .HasColumnType("varchar(95)");

                    b.Property<int?>("ThreshHold")
                        .HasColumnType("int");

                    b.Property<int>("TypeCompare")
                        .HasColumnType("int");

                    b.Property<int?>("Value")
                        .HasColumnType("int");

                    b.HasKey("DeviceId", "RuleId");

                    b.HasIndex("RuleId");

                    b.ToTable("TopicThreshholds");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.UserEntity", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("Adress")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DateCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(95)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Apartment", b =>
                {
                    b.HasOne("FireManagerServer.Database.Entity.Building", "Building")
                        .WithMany("Apartments")
                        .HasForeignKey("BuldingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Building");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.ApartmentNeighbour", b =>
                {
                    b.HasOne("FireManagerServer.Database.Entity.Apartment", "Apartment1")
                        .WithMany("ApartmentNeighbours")
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FireManagerServer.Database.Entity.Apartment", "Apartment2")
                        .WithMany("ApartmentNeighbours2")
                        .HasForeignKey("NeighbourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Apartment1");

                    b.Navigation("Apartment2");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Building", b =>
                {
                    b.HasOne("FireManagerServer.Database.Entity.UserEntity", "User")
                        .WithMany("Buildings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.DeviceEntity", b =>
                {
                    b.HasOne("FireManagerServer.Database.Entity.Module", "Module")
                        .WithMany("Devices")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.HistoryData", b =>
                {
                    b.HasOne("FireManagerServer.Database.Entity.DeviceEntity", "DeviceEntity")
                        .WithMany("HistoryDatas")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FireManagerServer.Database.Entity.UserEntity", "User")
                        .WithMany("HistoryDatas")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("DeviceEntity");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Module", b =>
                {
                    b.HasOne("FireManagerServer.Database.Entity.Apartment", "Apartment")
                        .WithMany("Modules")
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("FireManagerServer.Database.Entity.UserEntity", "User")
                        .WithMany("Modules")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Apartment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.RuleEntity", b =>
                {
                    b.HasOne("FireManagerServer.Database.Entity.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.TopicThreshhold", b =>
                {
                    b.HasOne("FireManagerServer.Database.Entity.DeviceEntity", "Device")
                        .WithMany("TopicThreshholds")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FireManagerServer.Database.Entity.RuleEntity", "Rule")
                        .WithMany("TopicThreshholds")
                        .HasForeignKey("RuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Rule");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.UserEntity", b =>
                {
                    b.HasOne("FireManagerServer.Database.Entity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Apartment", b =>
                {
                    b.Navigation("ApartmentNeighbours");

                    b.Navigation("ApartmentNeighbours2");

                    b.Navigation("Modules");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Building", b =>
                {
                    b.Navigation("Apartments");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.DeviceEntity", b =>
                {
                    b.Navigation("HistoryDatas");

                    b.Navigation("TopicThreshholds");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Module", b =>
                {
                    b.Navigation("Devices");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.RuleEntity", b =>
                {
                    b.Navigation("TopicThreshholds");
                });

            modelBuilder.Entity("FireManagerServer.Database.Entity.UserEntity", b =>
                {
                    b.Navigation("Buildings");

                    b.Navigation("HistoryDatas");

                    b.Navigation("Modules");
                });
#pragma warning restore 612, 618
        }
    }
}
