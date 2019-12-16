namespace MTDiagnosticLogging.WebService
{
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Hosting;

    public class BusService : IHostedService
    {
        public BusService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        private readonly IBusControl _busControl;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}