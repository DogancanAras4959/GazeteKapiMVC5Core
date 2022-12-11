using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class MagazineBannerMapping : IEntityTypeConfiguration<Magazinebanner>
    {
        public void Configure(EntityTypeBuilder<Magazinebanner> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Link).HasMaxLength(50);
            builder.Property(x => x.BannerImage).HasMaxLength(50);
        }
    }
}
