namespace MTDiagnosticLogging.Service
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MassTransit;
    using Messages;

    public class PingConsumer : IConsumer<Ping>
    {
        public async Task Consume(ConsumeContext<Ping> context)
        {
            using var client = new HttpClient();
            await client.GetAsync("https://google.com");
        }
    }
}