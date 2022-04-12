using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Mapping
{
    public class MenuItemsMapping : IEntityTypeConfiguration<MenuItems>
    {
        public void Configure(EntityTypeBuilder<MenuItems> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ItemName).HasMaxLength(50);
            builder.Property(x => x.slug).HasMaxLength(70);
            builder.HasOne(x => x.type).WithMany(x => x.items).HasForeignKey(x => x.TypeId);
        }
    }
}
