using GazeteKapiMVC5Core.DataAccessLayerAbone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayerAbone.Mapping
{
    public class UsersMapping : IEntityTypeConfiguration<users>
    {
        public void Configure(EntityTypeBuilder<users> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.email).HasMaxLength(75);
            builder.Property(x => x.namesurname).HasMaxLength(100);
            builder.Property(x => x.password).HasMaxLength(25);
            builder.Property(x => x.phoneNumber).HasMaxLength(13);
            builder.Property(x => x.username).HasMaxLength(50);
            builder.HasOne(x => x.roles).WithMany(x => x.userList).HasForeignKey(x => x.rolId);
        }
    }
}
