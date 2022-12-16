using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class BannerMapping : IEntityTypeConfiguration<Banners>
    {
        public void Configure(EntityTypeBuilder<Banners> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.BannerFrame).HasMaxLength(800);
            builder.Property(x => x.BannerName).HasMaxLength(100);
            builder.Property(x => x.Link).HasMaxLength(300);
            builder.HasOne(x => x.bannerRotate).WithMany(x => x.bannerList).HasForeignKey(x => x.RotateId);
        }
    }
}
