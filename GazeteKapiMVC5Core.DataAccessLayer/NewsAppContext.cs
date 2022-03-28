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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new RolesMapping());
            modelBuilder.ApplyConfiguration(new AuthorizesMapping());
            modelBuilder.ApplyConfiguration(new RoleAuthorizeMapping());
            modelBuilder.ApplyConfiguration(new CategoriesMapping()); modelBuilder.ApplyConfiguration(new NewsMapping());
            modelBuilder.ApplyConfiguration(new GuestMapping());
            modelBuilder.ApplyConfiguration(new PublishTypeMapping());
            modelBuilder.ApplyConfiguration(new TagMapping());
            modelBuilder.ApplyConfiguration(new TagNewsMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
