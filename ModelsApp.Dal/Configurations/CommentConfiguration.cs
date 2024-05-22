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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public CommentConfiguration() : base() { }
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(nameof(Comment), "public", table =>
            {
                table.HasCheckConstraint("ValidRating", "\"Rating\" BETWEEN 0 AND 5");
            });
            builder.HasKey(item => item.Id);
            builder.HasIndex(item => item.Id).IsUnique();
            builder.HasIndex(item => item.Guid).IsUnique();

            builder.Property(item => item.Text).HasMaxLength(200);

            builder.HasOne(item => item.User).WithMany(item => item.Comments)
                .HasForeignKey((Comment item) => item.UserId)
                .HasPrincipalKey((UserProfile item) => item.Id);
            builder.HasOne(item => item.Model).WithMany(item => item.Comments)
                .HasForeignKey((Comment item) => item.ModelId)
                .HasPrincipalKey((Model item) => item.Id);
        }
    }
}
