using AutoMapper;
using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.ModelInfo.Commons;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.BookmarkInfo.Commons
{
    public class BookmarkItemData : IMappingTarget<Model>
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;

        public int Downloads { get; set; } = default;
        public int Views { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;
        public double Rating { get; set; } = default!;

        public string CategoryName { get; set; } = string.Empty;
        public string? ImageName { get; set; } = default;
        public virtual void ConfigureMapping(Profile profile)
        {
            var averageFilter = (Model p) => p.Comments.Sum(op => (double)op.Rating / p.Comments.Count());
            profile.CreateMap<Model, BookmarkItemData>()
                .ForMember(p => p.CategoryName, options => options.MapFrom(p => p.Category.Name))
                .ForMember(item => item.Rating, options => options.MapFrom(p => averageFilter(p)));
        }
    }
    public class BookmarkListData : object
    {
        public List<BookmarkItemData> Items { get; set; } = new();
        public int AllCount { get; set; } = default!;
    }
}
