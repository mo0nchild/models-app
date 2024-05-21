using Microsoft.EntityFrameworkCore;
using ModelsApp.Dal.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal.Entities
{
    [EntityTypeConfiguration(typeof(AuthorizationConfiguration))]
    public class Authorization : object
    {
        public int Id { get; set; } = default;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public int UserProfileId { get; set; } = default!;
        public virtual UserProfile UserProfile { get; set; } = default!;
    }
}
