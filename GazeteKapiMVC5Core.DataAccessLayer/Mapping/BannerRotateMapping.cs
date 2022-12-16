using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class BannerRotateMapping : IEntityTypeConfiguration<BannersRotate>
    {
        public void Configure(EntityTypeBuilder<BannersRotate> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RotateName);
        }
    }
}
