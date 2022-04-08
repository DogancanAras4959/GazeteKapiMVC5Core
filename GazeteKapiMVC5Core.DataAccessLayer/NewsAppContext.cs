using DOMAIN.DataAccessLayer.Mapping;
using DOMAIN.DataAccessLayer.Models;
using GazeteKapiMVC5Core.DataAccessLayer.Mapping;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer
{
    public class NewsAppContext : DbContext
    {
        public NewsAppContext(DbContextOptions<NewsAppContext> options) : base(options)
        {

        }
        public virtual DbSet<Users> users { get; set; }
        public virtual DbSet<Roles> roles { get; set; }
        public virtual DbSet<Authorizes> authorize { get; set; }
        public virtual DbSet<RoleAuthorize> roleAuthorize { get; set; }
        public virtual DbSet<Categories> categories { get; set; }
        public virtual DbSet<News> news { get; set; }
        public virtual DbSet<Guest> guest { get; set; }
        public virtual DbSet<PublishType> publishTypes { get; set; }
        public virtual DbSet<Tags> tagNames { get; set; }
        public virtual DbSet<TagNews> tagNews { get; set; }
        public virtual DbSet<Settings> setting { get; set; }
        public virtual DbSet<Privacy> privacy { get; set; }
        public virtual DbSet<AboutUs> aboutus { get; set; }
        public virtual DbSet<TermsOfUse> termsofUse { get; set; }
        public virtual DbSet<Currency> currency { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new RolesMapping());
            modelBuilder.ApplyConfiguration(new AuthorizesMapping());
            modelBuilder.ApplyConfiguration(new RoleAuthorizeMapping());
            modelBuilder.ApplyConfiguration(new CategoriesMapping());
            modelBuilder.ApplyConfiguration(new NewsMapping());
            modelBuilder.ApplyConfiguration(new GuestMapping());
            modelBuilder.ApplyConfiguration(new PublishTypeMapping());
            modelBuilder.ApplyConfiguration(new TagMapping());
            modelBuilder.ApplyConfiguration(new TagNewsMapping());
            modelBuilder.ApplyConfiguration(new SettingsMapping());
            modelBuilder.ApplyConfiguration(new PrivacyMapping());
            modelBuilder.ApplyConfiguration(new AboutUsMapping());
            modelBuilder.ApplyConfiguration(new TermsOfUseMapping());
            modelBuilder.ApplyConfiguration(new CurrencyMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
