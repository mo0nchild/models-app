using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.UserInfo.Commons
{
    public class UserData : IMappingTarget<UserProfile>
    {
        public Guid Guid { get; set; } = Guid.Empty; 
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string? Biography { get; set; } = default!;
        public string? ImageName { get; set; } = default!;
    }
}
