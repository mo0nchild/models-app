namespace ModelsApp.Api.Commons.Exceptions
{
    public class ApiException : Exception
    {
        public Type TypeReference { get; set; } = default!;
        public ApiException(string message, Type type) : base(message) 
        {
            this.TypeReference = type;
        }
    }
}
