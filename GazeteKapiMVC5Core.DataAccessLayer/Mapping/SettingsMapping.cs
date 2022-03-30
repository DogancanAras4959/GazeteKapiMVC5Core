using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class SettingsMapping : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Logo).HasMaxLength(100);
            builder.Property(x => x.SiteName).HasMaxLength(100);
            builder.Property(x => x.SiteSlogan).HasMaxLength(100);
            builder.HasOne(x => x.user).WithMany(x => x.settings).HasForeignKey(x => x.UserId);
        }
    }
}
