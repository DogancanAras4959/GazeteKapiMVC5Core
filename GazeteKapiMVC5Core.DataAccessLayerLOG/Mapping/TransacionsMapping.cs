using DOMAIN.DataAccessLayerLOG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayerLOG.Mapping
{
    public class TransacionsMapping : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TransactionNames).HasMaxLength(70);
        }
    }
}
