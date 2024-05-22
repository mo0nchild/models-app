using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.ModelInfo.Commons;
using System.ComponentModel.DataAnnotations;

namespace ModelsApp.Api.Models.Models.Requests
{
    public class UpdateModelRequest : IMappingTarget<UpdateModelData>
    {
        [Required(ErrorMessage = "Необходимо указать UUID модели")]
        public Guid UUID { get; set; } = Guid.Empty;

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина названия в диапазоне от 5 до 50 символов")]
        [Required(ErrorMessage = "Необходимо указать название")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо указать категорию модели")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(200, MinimumLength = 5, ErrorMessage = "Длина описания в диапазоне от 5 до 200 символов")]
        [Required(ErrorMessage = "Необходимо указать описания")]
        public string Description { get; set; } = String.Empty;
    }
}
