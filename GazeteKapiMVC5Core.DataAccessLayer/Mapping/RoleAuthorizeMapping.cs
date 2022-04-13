using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayer.Mapping
{
    public class RoleAuthorizeMapping : IEntityTypeConfiguration<RoleAuthorize>
    {
        public void Configure(EntityTypeBuilder<RoleAuthorize> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.role).WithMany(x => x.roleAuthroizeForRole).HasForeignKey(x => x.RoleId);
            builder.HasOne(x => x.authroize).WithMany(x => x.roleAuthroizeForRole).HasForeignKey(x => x.AuthorizeId);
        }
    }
}
