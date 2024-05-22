using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.BookmarkInfo;
using ModelsApp.Api.Services.CommentInfo;
using ModelsApp.Api.Services.ModelInfo;
using ModelsApp.Api.Services.RequestLogging;
using ModelsApp.Api.Services.S3Storage;
using ModelsApp.Api.Services.UserInfo;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ModelsApp.Api.Services
{
    public static class Bootstrapper : object
    {
        public static async Task<IServiceCollection> AddApiServices(this IServiceCollection collection, IConfiguration configuration)
        {
            await collection.AddS3Storage(configuration);
            collection.AddAutoMapper(options =>
            {
                options.AddProfile(new AssemblyProfile(Assembly.GetExecutingAssembly()));
            });
            collection.AddTransient<IRequestLogging, RequestLogging.RequestLogging>();

            collection.AddTransient<ICommentInfo, CommentInfo.CommentInfo>();
            collection.AddTransient<IBookmarkInfo, BookmarkInfo.BookmarkInfo>();

            collection.AddTransient<IUserInfo, UserInfo.UserInfo>();
            collection.AddTransient<IModelInfo, ModelInfo.ModelInfo>();
            return collection;
        }
    }
}
