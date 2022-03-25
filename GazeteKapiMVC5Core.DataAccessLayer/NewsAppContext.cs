using DOMAIN.DataAccessLayer.Mapping;
using DOMAIN.DataAccessLayer.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new RolesMapping());
            modelBuilder.ApplyConfiguration(new AuthorizesMapping());
            modelBuilder.ApplyConfiguration(new RoleAuthorizeMapping());
            modelBuilder.ApplyConfiguration(new CategoriesMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
