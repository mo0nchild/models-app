using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelsApp.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal.Configurations
{
    public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        public BookmarkConfiguration() : base() { }
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.ToTable(nameof(Bookmark), "public");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(item => item.User).WithMany(item => item.Bookmarks)
                .HasForeignKey((Bookmark item) => item.UserId)
                .HasPrincipalKey((UserProfile item) => item.Id);

            builder.HasOne(item => item.Model).WithMany(item => item.Bookmarks)
                .HasForeignKey((Bookmark item) => item.ModelId)
                .HasPrincipalKey((Model item) => item.Id);
        }
    }
}
