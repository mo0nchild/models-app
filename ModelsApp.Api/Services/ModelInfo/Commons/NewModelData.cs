using AutoMapper;
using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.ModelInfo.Commons
{
    public class NewModelData : IMappingTarget<Model>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;
        public IFormFile Image { get; set; } = default!;
        public string CategoryName { get; set; } = default!;

        public Guid OwnerUuid { get; set; } = Guid.Empty;
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

        public double TargetX { get; set; } = default!;
        public double TargetY { get; set; } = default!;
        public double TargetZ { get; set; } = default!;

        public virtual void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<NewModelData, Model>().ForMember(p => p.Info, 
                options => options.MapFrom(p => new ModelsApp.Dal.Entities.ModelInfo()
                {
                    TargetX = p.TargetX, TargetY = p.TargetY, TargetZ = p.TargetZ,
                    CameraX = p.CameraX, CameraY = p.CameraY, CameraZ = p.CameraZ,
                    LightHeight = p.LightHeight, 
                    LightRadius = p.LightRadius, 
                    SkyIntensity = p.SkyIntensity, LightIntensity = p.LightIntensity,
                    SceneColor = p.SceneColor,
                    MemorySize = p.MemorySize,
                    Triangles = p.Triangles, Vertices = p.Vertices,
                }));
        }
    }
}
