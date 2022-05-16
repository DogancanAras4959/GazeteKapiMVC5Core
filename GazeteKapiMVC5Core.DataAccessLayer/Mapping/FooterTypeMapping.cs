using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class FooterTypeMapping : IEntityTypeConfiguration<FooterType>
    {
        public void Configure(EntityTypeBuilder<FooterType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TypeName).HasMaxLength(30);
        }
    }
}
