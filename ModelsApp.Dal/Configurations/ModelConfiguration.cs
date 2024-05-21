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
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public ModelConfiguration() : base() { }
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.ToTable(nameof(Model), "public");
            builder.HasKey(item => item.Id);
            builder.HasIndex(item => item.Id).IsUnique();
            builder.HasIndex(item => item.Guid).IsUnique();

            builder.Property(item => item.Name).HasMaxLength(50);
            builder.Property(item => item.Description).HasMaxLength(200);

            builder.HasOne(item => item.Owner).WithMany(item => item.Models)
                .HasForeignKey((Model item) => item.OwnerId)
                .HasPrincipalKey((UserProfile item) => item.Id);
            builder.HasOne(item => item.Category).WithMany(item => item.Models)
                .HasForeignKey((Model item) => item.CategoryId)
                .HasPrincipalKey((ModelCategory item) => item.Id);

            builder.HasOne(item => item.Info).WithOne(item => item.Model)
                .HasForeignKey((Model item) => item.InfoId)
                .HasPrincipalKey((ModelInfo item) => item.Id);
        }
    }
}
