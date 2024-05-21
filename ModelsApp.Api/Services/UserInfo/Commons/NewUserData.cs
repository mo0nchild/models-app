using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Dal.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AutoMapper;

namespace ModelsApp.Api.Services.UserInfo.Commons
{
    using BCryptType = BCrypt.Net.BCrypt;
    public class NewUserData : IMappingTarget<UserProfile>
    {
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;

        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Biography { get; set; } = default!;

        public IFormFile? Image { get; set; } = default!;
        public virtual void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<NewUserData, UserProfile>()
                .ForMember(p => p.Authorization, options => options.MapFrom(p => new Authorization()
                {
                    Login = p.Login,
                    Password = BCryptType.HashPassword(p.Password),
                }));
        }
    }
}
