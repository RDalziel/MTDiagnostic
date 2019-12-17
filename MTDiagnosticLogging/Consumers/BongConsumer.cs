namespace MTDiagnosticLogging.Service.Consumers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MassTransit;
    using Messages;

    public class BongConsumer : IConsumer<Bong>
    {
        public async Task Consume(ConsumeContext<Bong> context)
        {
            using var client = new HttpClient();
            await client.GetAsync("https://google.com");
        }
    }
}