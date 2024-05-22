using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.CommentInfo.Commons;
using System.ComponentModel.DataAnnotations;

namespace ModelsApp.Api.Models.Comments.Requests
{
    public class AddCommentRequest : IMappingTarget<NewCommentData>
    {
        [Range(0, 5, ErrorMessage = "Значение рейтинга в диапазоне от 0 до 5")]
        [Required(ErrorMessage = "Необходимо указать значение рейтинга")]
        public double Rating { get; set; } = default!;

        [StringLength(200, MinimumLength = 5, ErrorMessage = "Длина текст комментария в диапазоне от 5 до 200 символов")]
        [Required(ErrorMessage = "Необходимо указать текст комментария")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо указать UUID модели")]
        public Guid ModelUuid { get; set; } = Guid.Empty;
    }
}
