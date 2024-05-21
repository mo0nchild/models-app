using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelsApp.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal.Configurations
{
    public class AuthorizationConfiguration : IEntityTypeConfiguration<Authorization>
    {
        public AuthorizationConfiguration() : base() { }
        public void Configure(EntityTypeBuilder<Authorization> builder)
        {
            builder.ToTable(nameof(Authorization), "public");
            builder.HasKey(item => item.Id);
            builder.HasIndex(item => item.Id).IsUnique();
            builder.HasIndex(item => item.Login).IsUnique();

            builder.Property(item => item.Login).HasMaxLength(50);
            builder.Property(item => item.Password).HasMaxLength(100);

            builder.HasOne(item => item.UserProfile).WithOne(item => item.Authorization)
                .HasForeignKey((Authorization item) => item.UserProfileId)
                .HasPrincipalKey((UserProfile item) => item.Id);
        }
    }
}
