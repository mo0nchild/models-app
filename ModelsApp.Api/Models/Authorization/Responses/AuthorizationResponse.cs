namespace ModelsApp.Api.Models.Authorization.Responses
{
    public class AuthorizationResponse : object
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public string JwtToken { get; set; } = default!;
    }
}
