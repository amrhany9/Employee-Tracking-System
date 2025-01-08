using back_end.Mediators.Attendance;

namespace back_end.Services
{
    public class AppActionService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppActionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var attendanceMediator = scope.ServiceProvider.GetRequiredService<IAttendanceMediator>();

            await Task.Run(() => attendanceMediator.SyncDailyLog(), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
