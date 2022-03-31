using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class TermsOfUseMapping : IEntityTypeConfiguration<TermsOfUse>
    {
        public void Configure(EntityTypeBuilder<TermsOfUse> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.HasOne(x => x.user).WithMany(x => x.termsofuse).HasForeignKey(x => x.UserId);
        }
    }
}
