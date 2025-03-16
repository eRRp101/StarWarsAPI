using System.Text.Json;

namespace BlazorApp2.Services
{
    public class ApiExceptionService
    {
        private readonly ILogger<ApiExceptionService> _logger;

        public ApiExceptionService(ILogger<ApiExceptionService> logger)
        {
            _logger = logger;
        }
        public void LogException(Exception ex, string context)
        {
            _logger.LogError(ex, $"An error occurred in {context}: {ex.Message}");
        }

        public string GetUserFriendlyErrorMessage(Exception ex)
        {
            return ex switch
            {
                HttpRequestException => "Failed to connect to the API. Please check your internet connection.",
                JsonException => "Failed to process the API response. The data may be invalid.",
                ApiServiceException => ex.Message,
                _ => "An unexpected error occurred. Please try again later."
            };
        }
    }
}
