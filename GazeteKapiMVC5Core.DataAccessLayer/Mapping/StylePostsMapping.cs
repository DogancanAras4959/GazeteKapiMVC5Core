using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class StylePostsMapping : IEntityTypeConfiguration<StylePosts>
    {
        public void Configure(EntityTypeBuilder<StylePosts> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StyleName).HasMaxLength(50);
        }
    }
}
