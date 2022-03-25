using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class PublishTypeMapping : IEntityTypeConfiguration<PublishType>
    {
        public void Configure(EntityTypeBuilder<PublishType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TypeName).HasMaxLength(70).IsRequired();
            builder.HasOne(x => x.user).WithMany(x => x.publishTypeList).HasForeignKey(x => x.UserId);
        }
    }
}
