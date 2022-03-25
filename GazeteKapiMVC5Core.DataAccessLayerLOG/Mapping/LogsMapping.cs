using DOMAIN.DataAccessLayerLOG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayerLOG.Mapping
{
    public class LogsMapping : IEntityTypeConfiguration<Logs>
    {
        public void Configure(EntityTypeBuilder<Logs> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Action).HasMaxLength(100);
            builder.Property(x => x.Controller).HasMaxLength(100);
            builder.HasOne(x => x.processes).WithMany(x => x.logsByProccess).HasForeignKey(x => x.ProcessID);
            builder.HasOne(x => x.transactions).WithMany(x => x.logsByTransactions).HasForeignKey(x => x.TransactionID);
            builder.HasOne(x => x.userslog).WithMany(x => x.logsByUsers).HasForeignKey(x => x.UserID);
        }
    }
}
