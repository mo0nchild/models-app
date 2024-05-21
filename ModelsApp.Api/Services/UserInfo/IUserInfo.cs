
namespace ModelsApp.Api.Services.UserInfo
{
    using ModelsApp.Api.Models.Authorization.Requests;
    using ModelsApp.Api.Services.UserInfo.Commons;
    public interface IUserInfo
    {
        public Task<UserData?> Authorization(string login, string password);
        public Task AddUser(NewUserData userData);
        public Task UpdateUser(UpdateUserData userData);
        public Task DeleteUser(Guid uuid);

        public Task<UserData?> GetByUUID(Guid guid);
        public Task<UserData?> GetByEmail(string email);
    }
}
