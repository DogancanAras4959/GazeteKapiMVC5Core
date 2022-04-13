using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayer.Mapping
{
    public class AuthorizesMapping : IEntityTypeConfiguration<Authorizes>
    {
        public void Configure(EntityTypeBuilder<Authorizes> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AuthorizeName).HasMaxLength(70);
            builder.Property(x => x.AuthorizeCode).HasMaxLength(70);
        }
    }
}
