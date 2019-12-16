namespace MTDiagnosticLogging.Service
{
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class WindowsService : IHostedService
    {
        public WindowsService(IBusControl busControl, ILoggerFactory loggerFactory)
        {
            _busControl = busControl;
            _log = loggerFactory.CreateLogger<WindowsService>();
        }

        private readonly IBusControl _busControl;
        private readonly ILogger _log;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("Starting bus...");
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("Stopping bus...");
            return _busControl.StopAsync(cancellationToken);
        }
    }
}