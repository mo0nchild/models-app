﻿using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.ModelInfo.Commons;
using System.ComponentModel.DataAnnotations;

namespace ModelsApp.Api.Models.Models.Requests
{
    public class AddModelRequest : IMappingTarget<NewModelData>
    {
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина названия в диапазоне от 5 до 50 символов")]
        [Required(ErrorMessage = "Необходимо указать название")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, MinimumLength = 5, ErrorMessage = "Длина описания в диапазоне от 5 до 200 символов")]
        [Required(ErrorMessage = "Необходимо указать описания")]
        public string Description { get; set; } = String.Empty;
        public IFormFile Image { get; set; } = default!;

        [Required(ErrorMessage = "Необходимо указать категорию модели")]
        public string CategoryName { get; set; } = default!;

        public int Vertices { get; set; } = default!;
        public int Triangles { get; set; } = default!;

        public double MemorySize { get; set; } = default!;
        public IFormFile File { get; set; } = default!;

        public double LightIntensity { get; set; } = default!;
        public double SkyIntensity { get; set; } = default!;
        public double LightRadius { get; set; } = default!;
        public double LightHeight { get; set; } = default!;

        public string? SceneColor { get; set; } = string.Empty;
        public double CameraX { get; set; } = default!;
        public double CameraY { get; set; } = default!;
        public double CameraZ { get; set; } = default!;

        public double TargetX {  get; set; } = default!;
        public double TargetY { get; set; } = default!;
        public double TargetZ { get; set; } = default!;
    }
}
