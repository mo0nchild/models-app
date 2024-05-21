using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal.Entities
{
    [Table(nameof(ModelInfo), Schema = "public")]
    public class ModelInfo : object
    {
        [Key]
        public int Id { get; set; } = default!;
        public int Vertices { get; set; } = default!;
        public int Triangles { get; set; } = default!;

        public double MemorySize { get; set; } = default!;
        public string Filename { get; set; } = string.Empty;
        
        public double LightIntensity { get; set; } = default!;
        public double SkyIntensity { get; set; } = default!;
        public double LightRadius { get; set; } = default!;
        public double LightHeight { get; set; } = default!;
        
        public string? SceneColor { get; set; } = string.Empty;
        public double CameraX { get; set; } = default!;
        public double CameraY { get; set; } = default!;
        public double CameraZ { get; set; } = default!;

        public virtual Model Model { get; set; } = default!;
    }
}
