using GazeteKapiMVC5Core.DataAccessLayerAbone.Mapping;
using GazeteKapiMVC5Core.DataAccessLayerAbone.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayerAbone
{
    public class SubscribeNewsAppContext : DbContext
    {
        public SubscribeNewsAppContext(DbContextOptions<SubscribeNewsAppContext> options) : base(options)
        {

        }

        public DbSet<users> user { get; set; }
     
        public DbSet<rols> rol { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new RolesMapping());
      
            modelBuilder.ApplyConfiguration(new UsersMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
