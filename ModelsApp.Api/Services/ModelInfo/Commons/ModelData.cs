using AutoMapper;
using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.UserInfo.Commons;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.ModelInfo.Commons
{
    public class ModelData : IMappingTarget<Model>
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;

        public int Downloads { get; set; } = default;
        public int Views { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;

        public string CategoryName { get; set; } = string.Empty;
        public string? ImageName { get; set; } = default;

        public UserData Owner { get; set; } = default!;
        public ModelFileInfo Info { get; set; } = default!;
        public virtual void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<Model, ModelData>()
                .ForMember(p => p.CategoryName, options => options.MapFrom(p => p.Category.Name));
        }
    }
}
