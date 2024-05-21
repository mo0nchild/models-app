using Microsoft.EntityFrameworkCore;
using ModelsApp.Dal.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal.Entities
{
    [EntityTypeConfiguration(typeof(BookmarkConfiguration))]
    public class Bookmark : object
    {
        public int Id { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;

        public int UserId { get; set; } = default;
        public virtual UserProfile User { get; set; } = default!;

        public int ModelId { get; set; } = default!;
        public virtual Model Model { get; set; } = default!;
    }
}
