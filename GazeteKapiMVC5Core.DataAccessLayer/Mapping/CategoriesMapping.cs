using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayer.Mapping
{
    public class CategoriesMapping : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CategoryName).HasMaxLength(90);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.HasOne(x => x.user).WithMany(x => x.categoriesList).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.typeToCategories).WithMany(x => x.categoriesList).HasForeignKey(x => x.TypeId);
            builder.HasOne(x => x.stylePosts).WithMany(x => x.categories).HasForeignKey(x => x.StyleId);
        }
    }
}
