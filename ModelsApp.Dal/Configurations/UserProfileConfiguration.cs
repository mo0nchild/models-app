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
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfiguration() : base() { }
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable(nameof(UserProfile), "public");
            builder.HasKey(item => item.Id);
            builder.HasIndex(item => item.Id).IsUnique();
            builder.HasIndex(item => item.Guid).IsUnique();
            builder.HasIndex(item => item.Email).IsUnique();

            builder.Property(item => item.Name).HasMaxLength(50);
            builder.Property(item => item.Email).HasMaxLength(100);
        }
    }
}
