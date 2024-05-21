using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.UserInfo.Commons;

namespace ModelsApp.Api.Models.Account.Responses
{
    public class AccountResponse : IMappingTarget<UserData>
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string? Biography { get; set; } = default!;
        public string? ImageName { get; set; } = default!;
    }
}
