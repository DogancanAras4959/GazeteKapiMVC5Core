using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class GuestMapping : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.GuestName).HasMaxLength(70).IsRequired();
            builder.Property(x => x.GuestImage).HasMaxLength(100);
            builder.Property(x => x.Biography).HasMaxLength(700);
            builder.Property(x => x.Email).HasMaxLength(100);
            builder.Property(x => x.Facebook).HasMaxLength(150);
            builder.Property(x => x.Twitter).HasMaxLength(150);
            builder.Property(x => x.Youtube).HasMaxLength(150);
            builder.Property(x => x.Instagram).HasMaxLength(150);
            builder.Property(x => x.Gmail).HasMaxLength(150);
            builder.HasOne(x => x.users).WithMany(x => x.guestList).HasForeignKey(x => x.UserId);
        }
    }
}
