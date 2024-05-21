using System.ComponentModel.DataAnnotations;

namespace ModelsApp.Api.Models.Authorization.Requests
{
    /// <summary> 
    /// Данные для авторизации в профиль
    /// </summary>
    public class LoginRequest : object
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
    }
}
