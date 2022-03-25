using DOMAIN.DataAccessLayerLOG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayerLOG.Mapping
{
    public class ProcessesMapping : IEntityTypeConfiguration<Processes>
    {
        public void Configure(EntityTypeBuilder<Processes> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProcessesName).HasMaxLength(70);
        }
    }
}
