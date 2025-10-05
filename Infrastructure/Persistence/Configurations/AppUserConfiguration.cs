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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);

            // 値オブジェクト Email → 所有エンティティとしてマッピング
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                     .HasColumnName("Email")
                     .IsRequired()
                     .HasMaxLength(256);
            });

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.RowVersion)
                .IsRowVersion();

            // ソフトデリートのクエリフィルター
            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }
}
