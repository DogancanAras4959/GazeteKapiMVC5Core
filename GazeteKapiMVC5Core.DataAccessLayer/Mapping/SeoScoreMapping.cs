using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class SeoScoreMapping : IEntityTypeConfiguration<SeoScore>
    {
        public void Configure(EntityTypeBuilder<SeoScore> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Note);
            builder.Property(x => x.UniqeCode);
            builder.HasOne(x => x.news).WithMany(x => x.seoScoreNews).HasForeignKey(x => x.NewsId);
        }
    }
}
