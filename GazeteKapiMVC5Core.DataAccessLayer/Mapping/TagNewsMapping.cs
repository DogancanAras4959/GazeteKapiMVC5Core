using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class TagNewsMapping : IEntityTypeConfiguration<TagNews>
    {
        public void Configure(EntityTypeBuilder<TagNews> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.news).WithMany(x => x.tagNewsListForNews).HasForeignKey(x => x.NewsId);
            builder.HasOne(x => x.tag).WithMany(x => x.tagNewsForTag).HasForeignKey(x => x.TagId);
        }
    }
}
