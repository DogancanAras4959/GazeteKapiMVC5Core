using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class CurrencyMapping : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.code).HasMaxLength(50);
            builder.Property(x => x.crossorder).HasMaxLength(50);
            builder.Property(x => x.currencyCode).HasMaxLength(50);
            builder.Property(x => x.unit).HasMaxLength(50);
            builder.Property(x => x.name).HasMaxLength(50);
            builder.Property(x => x.currencyName).HasMaxLength(50);
            builder.Property(x => x.ForexBuying).HasMaxLength(50);
            builder.Property(x => x.ForexSelling).HasMaxLength(50);
            builder.Property(x => x.BanknoteBuying).HasMaxLength(50);
            builder.Property(x => x.BanknoteSelling).HasMaxLength(50);
            builder.Property(x => x.CrossRateOther).HasMaxLength(50);
            builder.Property(x => x.CrossRateUSD).HasMaxLength(50);
        }
    }
}
