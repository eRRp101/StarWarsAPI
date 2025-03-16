namespace BlazorApp2.Services
{
    public interface IDebounceService
    {
        Task DebounceAsync(Func<Task> action, int milliseconds = 500);
    }
}
