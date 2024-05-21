using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal.Entities
{
    [Table(nameof(ModelCategory), Schema = "public")]
    public class ModelCategory : object
    {
        [Key]
        public int Id { get; set; } = default!;
        public string Name { get; set; } = string.Empty;

        public virtual List<Model> Models { get; set; } = new();
    }
}
