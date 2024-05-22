using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.ModelInfo.Commons;

namespace ModelsApp.Api.Models.Models.Requests
{
    public class AddModelRequest : IMappingTarget<NewModelData>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;
        public IFormFile Image { get; set; } = default!;
        public string CategoryName { get; set; } = default!;

        public int Vertices { get; set; } = default!;
        public int Triangles { get; set; } = default!;

        public double MemorySize { get; set; } = default!;
        public IFormFile File { get; set; } = default!;

        public double LightIntensity { get; set; } = default!;
        public double SkyIntensity { get; set; } = default!;
        public double LightRadius { get; set; } = default!;
        public double LightHeight { get; set; } = default!;

        public string? SceneColor { get; set; } = string.Empty;
        public double CameraX { get; set; } = default!;
        public double CameraY { get; set; } = default!;
        public double CameraZ { get; set; } = default!;
    }
}
