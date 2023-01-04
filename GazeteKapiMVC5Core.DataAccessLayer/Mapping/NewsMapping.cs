using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class NewsMapping : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Image).HasMaxLength(100).IsRequired();
            builder.Property(x => x.NewsContent).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.HasOne(x => x.users).WithMany(x => x.newsList).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.categories).WithMany(x => x.newList).HasForeignKey(x => x.CategoryId);
            builder.HasOne(x => x.guest).WithMany(x => x.newList).HasForeignKey(x => x.GuestId);
            builder.HasOne(x => x.publishtype).WithMany(x => x.newList).HasForeignKey(x => x.PublishTypeId);
        }
    }
}
