using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class NewsIpMapping : IEntityTypeConfiguration<NewsIp>
    {
        public void Configure(EntityTypeBuilder<NewsIp> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ip).WithMany(x => x.newsIpListIp).HasForeignKey(x => x.IpAdressId);
            builder.HasOne(x => x.news).WithMany(x => x.newsIpList).HasForeignKey(x => x.NewsId);
        }
    }
}
