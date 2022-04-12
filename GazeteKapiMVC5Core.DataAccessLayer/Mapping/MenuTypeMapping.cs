using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class MenuTypeMapping : IEntityTypeConfiguration<MenuTypes>
    {
        public void Configure(EntityTypeBuilder<MenuTypes> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.MenuName).HasMaxLength(50);
            builder.Property(x => x.Type).HasMaxLength(40);
            builder.HasOne(x => x.user).WithMany(x => x.typesList).HasForeignKey(x => x.UserId);
        }
    }
}
