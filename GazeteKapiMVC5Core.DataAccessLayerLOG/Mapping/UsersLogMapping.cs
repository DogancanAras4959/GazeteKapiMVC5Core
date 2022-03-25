using DOMAIN.DataAccessLayerLOG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayerLOG.Mapping
{
    public class UsersLogMapping : IEntityTypeConfiguration<UsersLog>
    {
        public void Configure(EntityTypeBuilder<UsersLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserNameLog).HasMaxLength(70);
        }
    }
}
