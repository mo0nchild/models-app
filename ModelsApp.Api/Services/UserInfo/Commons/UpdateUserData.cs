using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.UserInfo.Commons
{
    public class UpdateUserData : IMappingTarget<UserProfile>
    {
        public Guid UUID { get; set; } = Guid.Empty;

        public string Name { get; set; } = default!;
        public string? Biography { get; set; } = default!;

        public IFormFile? Image { get; set; } = default!;
    }
}
