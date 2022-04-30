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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.AboutUs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("aboutus");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Authorizes", b =>
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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.BrandPolicy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("brand");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Categories", b =>
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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.CookiePolicy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("cookie");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("BanknoteBuying")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BanknoteSelling")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CrossRateOther")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CrossRateUSD")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ForexBuying")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ForexSelling")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("code")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("crossorder")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("currencyCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("currencyName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("isRateOrDown")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("unit")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("currency");
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

                    b.Property<string>("Facebook")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Gmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuestImage")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("GuestName")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Instagram")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Twitter")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Youtube")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("guest");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.MenuItems", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("ItemName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<string>("slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("items");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.MenuTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("MenuName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("types");
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

                    b.Property<string>("Sound")
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Privacy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("privacy");
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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.RoleAuthorize", b =>
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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Roles", b =>
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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CopyrightText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CopyrightTextTitle")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FooterLogo")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("GetAgencyNewsService")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActiveSettings")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCurrencyService")
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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.StreamPolicy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("stream");
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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.TermsOfUse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("termsofUse");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", b =>
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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.members", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("emailAdress")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("jobs")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("nameSurname")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("phoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("submitDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("members");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.AboutUs", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("aboutus")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.BrandPolicy", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("brand")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Categories", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("categoriesList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.CookiePolicy", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("cookie")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Guest", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "users")
                        .WithMany("guestList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("users");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.MenuItems", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.MenuTypes", "type")
                        .WithMany("items")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("type");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.MenuTypes", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("typesList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.News", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Categories", "categories")
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

                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "users")
                        .WithMany("newsList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("categories");

                    b.Navigation("guest");

                    b.Navigation("publishtype");

                    b.Navigation("users");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Privacy", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("privacy")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.PublishType", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("publishTypeList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.RoleAuthorize", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Authorizes", "authroize")
                        .WithMany("roleAuthroizeForRole")
                        .HasForeignKey("AuthorizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Roles", "role")
                        .WithMany("roleAuthroizeForRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("authroize");

                    b.Navigation("role");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Settings", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("settings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.StreamPolicy", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("stream")
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

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.TermsOfUse", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", "user")
                        .WithMany("termsofuse")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", b =>
                {
                    b.HasOne("GazeteKapiMVC5Core.DataAccessLayer.Models.Roles", "roles")
                        .WithMany("usersForRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("roles");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Authorizes", b =>
                {
                    b.Navigation("roleAuthroizeForRole");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Categories", b =>
                {
                    b.Navigation("newList");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Guest", b =>
                {
                    b.Navigation("newList");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.MenuTypes", b =>
                {
                    b.Navigation("items");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.News", b =>
                {
                    b.Navigation("tagNewsListForNews");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.PublishType", b =>
                {
                    b.Navigation("newList");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Roles", b =>
                {
                    b.Navigation("roleAuthroizeForRole");

                    b.Navigation("usersForRoles");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Tags", b =>
                {
                    b.Navigation("tagNewsForTag");
                });

            modelBuilder.Entity("GazeteKapiMVC5Core.DataAccessLayer.Models.Users", b =>
                {
                    b.Navigation("aboutus");

                    b.Navigation("brand");

                    b.Navigation("categoriesList");

                    b.Navigation("cookie");

                    b.Navigation("guestList");

                    b.Navigation("newsList");

                    b.Navigation("privacy");

                    b.Navigation("publishTypeList");

                    b.Navigation("settings");

                    b.Navigation("stream");

                    b.Navigation("termsofuse");

                    b.Navigation("typesList");
                });
#pragma warning restore 612, 618
        }
    }
}
