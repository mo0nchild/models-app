using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.UserInfo.Commons;

namespace ModelsApp.Api.Models.Account.Requests
{
    public class UpdateAccountRequest : IMappingTarget<UpdateUserData>
    {
        public string Name { get; set; } = default!;
        public string? Biography { get; set; } = default!;

        public IFormFile? Image { get; set; } = default!;
    }
}
