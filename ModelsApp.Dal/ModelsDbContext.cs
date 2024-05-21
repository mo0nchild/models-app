using Microsoft.EntityFrameworkCore;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Dal
{
    public partial class ModelsDbContext : DbContext
    {
        public virtual DbSet<Authorization> Authorizations { get; set; } = default!;
        public virtual DbSet<UserProfile> UserProfiles { get; set; } = default!;
        public virtual DbSet<Model> Models { get; set; } = default!;
        public virtual DbSet<ModelInfo> ModelInfos { get; set; } = default!;

        public virtual DbSet<ModelCategory> ModelCategories { get; set; } = default!;
        public virtual DbSet<Comment> Comments { get; set; } = default!;
        public virtual DbSet<Bookmark> Bookmarks { get; set; } = default!;

        public ModelsDbContext(DbContextOptions<ModelsDbContext> options) : base(options) { }
        public ModelsDbContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder.UseLazyLoadingProxies());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
