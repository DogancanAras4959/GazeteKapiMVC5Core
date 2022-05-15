using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class SeoCheckMetaMapping : IEntityTypeConfiguration<SeoCheckMeta>
    {
        public void Configure(EntityTypeBuilder<SeoCheckMeta> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Requirement).HasMaxLength(220);
            builder.Property(x => x.TypeLevel).HasMaxLength(70);
            builder.Property(x => x.metaCode).HasMaxLength(120);
            builder.HasOne(x => x.seoScoreToMeta).WithMany(x => x.seoMetas).HasForeignKey(x => x.SeoScoreId);
        }
    }
}
