using GazeteKapiMVC5Core.DataAccessLayerAbone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayerAbone.Mapping
{
    public class RolesMapping : IEntityTypeConfiguration<rols>
    {
        public void Configure(EntityTypeBuilder<rols> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.rolename).HasMaxLength(100);
        }
    }
}
