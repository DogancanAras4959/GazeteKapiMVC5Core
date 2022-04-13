using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayer.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DisplayName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.UserName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(70).IsRequired();
            builder.Property(x => x.EmailAdress).HasMaxLength(70);
            builder.Property(x => x.ProfileImage).HasMaxLength(50);
            builder.HasOne(x => x.roles).WithMany(x => x.usersForRoles).HasForeignKey(x => x.RoleId);

        }
    }
}
