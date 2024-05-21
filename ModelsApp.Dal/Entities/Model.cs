using Microsoft.EntityFrameworkCore;
using ModelsApp.Dal.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal.Entities
{
    [EntityTypeConfiguration(typeof(ModelConfiguration))]
    public class Model : object
    {
        public int Id { get; set; } = default!;
        public Guid Guid { get; set; } = Guid.Empty;

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;

        public int Downloads { get; set; } = default;
        public int Views { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;

        public string? ImageName { get; set; } = default;
        public int OwnerId { get; set; } = default!;
        public virtual UserProfile Owner { get; set; } = default!;

        public int CategoryId { get; set; } = default!;
        public virtual ModelCategory Category { get; set; } = default!;

        public int InfoId { get; set; } = default!;
        public virtual ModelInfo Info { get; set; } = default!;

        public virtual List<Bookmark> Bookmarks { get; set; } = new();
        public virtual List<Comment> Comments { get; set; } = new();
    }
}
