using Microsoft.EntityFrameworkCore;
using ModelsApp.Dal.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal.Entities
{
    [EntityTypeConfiguration(typeof(CommentConfiguration))]
    public class Comment : object
    {
        public int Id { get; set; }
        public Guid Guid { get; set; } = Guid.Empty;

        public double Rating { get; set; } = default!;
        public string Text { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = default!;

        public int UserId { get; set; } = default!;
        public virtual UserProfile User { get; set; } = default!;

        public int ModelId { get; set; } = default!;
        public virtual Model Model { get; set; } = default!;
    }
}
