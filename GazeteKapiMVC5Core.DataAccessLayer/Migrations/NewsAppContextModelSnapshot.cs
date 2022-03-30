﻿// <auto-generated />
using System;
using GazeteKapiMVC5Core.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GazeteKapiMVC5Core.DataAccessLayer.Migrations
{
    [DbContext(typeof(NewsAppContext))]
    partial class NewsAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Authorizes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AuthorizeCode")
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("AuthorizeName")
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.HasKey("Id");

                    b.ToTable("authorize");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Categories", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CategoryName")
                        .HasMaxLength(90)
                        .HasColumnType("nvarchar(90)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.RoleAuthorize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AuthorizeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("bit");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorizeId");

                    b.HasIndex("RoleId");

                    b.ToTable("roleAuthorize");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EmailAdress")
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("ProfileImage")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Biography")
                        .HasMaxLength(700)
                        .HasColumnType("nvarchar(700)");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("GuestImage")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("GuestName")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("guest");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("GuestId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCommentActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLock")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOpenNotifications")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSlide")
                        .HasColumnType("bit");

                    b.Property<string>("NewsContent")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("PublishTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PublishedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Sorted")
                        .HasColumnType("int");

                    b.Property<string>("Spot")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("GuestId");

                    b.HasIndex("PublishTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("news");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.PublishType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("publishTypes");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("GetAgencyNewsService")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActiveSettings")
                        .HasColumnType("bit");

                    b.Property<bool>("LogIsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("LogSystemErrorActive")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SiteName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SiteSlogan")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("setting");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.TagNews", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("NewsId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NewsId");

                    b.HasIndex("TagId");

                    b.ToTable("tagNews");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Tags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("TagName")
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.HasKey("Id");

                    b.ToTable("tagNames");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Categories", b =>
                {
                    b.HasOne("DOMAIN.DataAccessLayer.Models.Users", "user")
                        .WithMany("categoriesList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.RoleAuthorize", b =>
                {
                    b.HasOne("DOMAIN.DataAccessLayer.Models.Authorizes", "authroize")
                        .WithMany("roleAuthroizeForRole")
                        .HasForeignKey("AuthorizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DOMAIN.DataAccessLayer.Models.Roles", "role")
                        .WithMany("roleAuthroizeForRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("authroize");

                    b.Navigation("role");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Users", b =>
                {
                    b.HasOne("DOMAIN.DataAccessLayer.Models.Roles", "roles")
                        .WithMany("usersForRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("roles");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Guest", b =>
                {
                    b.HasOne("DOMAIN.DataAccessLayer.Models.Users", "users")
                        .WithMany("guestList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("users");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.News", b =>
                {
                    b.HasOne("DOMAIN.DataAccessLayer.Models.Categories", "categories")
                        .WithMany("newList")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Guest", "guest")
                        .WithMany("newList")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.PublishType", "publishtype")
                        .WithMany("newList")
                        .HasForeignKey("PublishTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DOMAIN.DataAccessLayer.Models.Users", "users")
                        .WithMany("newsList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("categories");

                    b.Navigation("guest");

                    b.Navigation("publishtype");

                    b.Navigation("users");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.PublishType", b =>
                {
                    b.HasOne("DOMAIN.DataAccessLayer.Models.Users", "user")
                        .WithMany("publishTypeList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Settings", b =>
                {
                    b.HasOne("DOMAIN.DataAccessLayer.Models.Users", "user")
                        .WithMany("settings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.TagNews", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.News", "news")
                        .WithMany("tagNewsListForNews")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Tags", "tag")
                        .WithMany("tagNewsForTag")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("news");

                    b.Navigation("tag");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Authorizes", b =>
                {
                    b.Navigation("roleAuthroizeForRole");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Categories", b =>
                {
                    b.Navigation("newList");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Roles", b =>
                {
                    b.Navigation("roleAuthroizeForRole");

                    b.Navigation("usersForRoles");
                });

            modelBuilder.Entity("DOMAIN.DataAccessLayer.Models.Users", b =>
                {
                    b.Navigation("categoriesList");

                    b.Navigation("guestList");

                    b.Navigation("newsList");

                    b.Navigation("publishTypeList");

                    b.Navigation("settings");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Guest", b =>
                {
                    b.Navigation("newList");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.News", b =>
                {
                    b.Navigation("tagNewsListForNews");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.PublishType", b =>
                {
                    b.Navigation("newList");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Tags", b =>
                {
                    b.Navigation("tagNewsForTag");
                });
#pragma warning restore 612, 618
        }
    }
}
