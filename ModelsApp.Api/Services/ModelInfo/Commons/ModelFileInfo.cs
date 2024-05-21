using ModelsApp.Api.Commons.Mapping;

namespace ModelsApp.Api.Services.ModelInfo.Commons
{
    using ModelInfoEntity = ModelsApp.Dal.Entities.ModelInfo;
    public class ModelFileInfo : IMappingTarget<ModelInfoEntity>
    {
        public int Vertices { get; set; } = default!;
        public int Triangles { get; set; } = default!;

        public double MemorySize { get; set; } = default!;

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
