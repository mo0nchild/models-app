using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.UserInfo.Commons;
using System.ComponentModel.DataAnnotations;

namespace ModelsApp.Api.Models.Account.Requests
{
    public class UpdateAccountRequest : IMappingTarget<UpdateUserData>
    {
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Длина имени пользователя в диапазоне от 5 до 50 символов")]
        [Required(ErrorMessage = "Необходимо указать имя пользователя")]
        public string Name { get; set; } = default!;
        public string? Biography { get; set; } = default!;

        public IFormFile? Image { get; set; } = default!;
    }
}
