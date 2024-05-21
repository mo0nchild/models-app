using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.UserInfo.Commons;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelsApp.Api.Models.Authorization.Requests
{
    /// <summary>
    /// Данные для регистрации профиля пользователя
    /// </summary>
    public class RegistrationRequest : IMappingTarget<NewUserData>
    {
        /// <summary> 
        /// Логин пользователя 
        /// </summary>
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина логина в диапазоне от 5 до 50 символов")]
        [Required(ErrorMessage = "Необходимо указать логин пользователя")]
        public string Login { get; set; } = default!;

        /// <summary> 
        /// Пароль пользователя 
        /// </summary>
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина пароля в диапазоне от 5 до 50 символов")]
        [Required(ErrorMessage = "Необходимо указать пароль пользователя")]
        public string Password { get; set; } = default!;

        /// <summary> 
        /// Имя пользователя
        /// </summary>
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Длина имени пользователя в диапазоне от 5 до 50 символов")]
        [Required(ErrorMessage = "Необходимо указать имя пользователя")]
        public string Name { get; set; } = default!;

        /// <summary> 
        /// Данные электронной почты
        /// </summary>
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Неверный формат почты")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Длина почты в диапазоне от 5 до 100 символов")]
        [Required(ErrorMessage = "Не установлено значение электронной почты"), DefaultValue("string")]
        public string Email { get; set; } = default!;

        /// <summary> 
        /// Данные о пользователе
        /// </summary>
        public string? Biography { get; set; } = default!;
        
        /// <summary> 
        /// Ищображение профиля пользователя;
        /// </summary>
        public IFormFile? Image { get; set; } = default!;
    }
}
