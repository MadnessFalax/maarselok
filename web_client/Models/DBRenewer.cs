using CS_projekt.data;

namespace web_client.Models
{
    public class DBRenewer : IHostedService, IDisposable
    {
        private Timer _timer;

        private void RefreshData(object? state)
        {
            TableOperation<StudentTable>.ForceRefreshAll();
            TableOperation<SchoolTable>.ForceRefreshAll();
            TableOperation<ProgramTable>.ForceRefreshAll();
            TableOperation<ApplicationTable>.ForceRefreshAll();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RefreshData, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
