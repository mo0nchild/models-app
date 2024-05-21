using Microsoft.EntityFrameworkCore;
using ModelsApp.Dal.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal.Entities
{
    [EntityTypeConfiguration(typeof(UserProfileConfiguration))]
    public class UserProfile : object
    {
        public int Id { get; set; } = default!;
        public Guid Guid { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string? ImageName { get; set; } = default;
        public string? Biography {  get; set; } = default;
        public DateTime DateTime { get; set; } = default!;

        public virtual Authorization Authorization { get; set; } = default!;
        public virtual List<Bookmark> Bookmarks { get; set; } = new();
        public virtual List<Comment> Comments { get; set; } = new();
        public virtual List<Model> Models { get; set; } = new();
    }
}
