namespace StarWarsAPI.Services
{
    public class DebounceService : IDebounceService
    {
        private CancellationTokenSource? _cts;

        public async Task DebounceAsync(Func<Task> action, int milliseconds = 500) 
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                await Task.Delay(milliseconds, _cts.Token);
                await action();
            }
            catch(TaskCanceledException)
            {
            }
        }
 
        //private async Task SearchAsync()
        //{
        //    await _debounceService.DebounceAsync(<Task>action async () =>
        //    {
        //        //search method g0es h3r3
        //    });
        //}
    }
}
