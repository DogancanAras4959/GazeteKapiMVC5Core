using DOMAIN.DataAccessLayerLOG.Mapping;
using DOMAIN.DataAccessLayerLOG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayerLOG
{
    public class NewsAppContextLog : DbContext
    {
        public NewsAppContextLog(DbContextOptions<NewsAppContextLog> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransacionsMapping());
            modelBuilder.ApplyConfiguration(new ProcessesMapping());
            modelBuilder.ApplyConfiguration(new UsersLogMapping());
            modelBuilder.ApplyConfiguration(new UsersLogMapping());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Logs> logs { get; set; }
        public DbSet<Processes> processes { get; set; }
        public DbSet<Transactions> transactions { get; set; }
        public DbSet<UsersLog> usersLogs { get; set; }
    }
}
