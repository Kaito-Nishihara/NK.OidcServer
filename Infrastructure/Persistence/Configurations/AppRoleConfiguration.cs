using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(r => r.RowVersion)
                .IsRowVersion();

            builder.HasQueryFilter(r => !r.IsDeleted);
        }
    }
}
