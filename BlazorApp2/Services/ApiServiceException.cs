namespace BlazorApp2.Services
{
    public class ApiServiceException : Exception
    {
        public ApiServiceException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
