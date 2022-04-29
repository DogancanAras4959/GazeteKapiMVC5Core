using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class membersMapping : IEntityTypeConfiguration<members>
    {
        public void Configure(EntityTypeBuilder<members> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.nameSurname).HasMaxLength(100);
            builder.Property(x => x.description).HasMaxLength(500);
            builder.Property(x => x.emailAdress).HasMaxLength(150);
            builder.Property(x => x.jobs).HasMaxLength(250);
            builder.Property(x => x.phoneNumber).HasMaxLength(15);
        }
    }
}
