namespace MTDiagnosticLogging.Service.Consumers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MassTransit;
    using Messages;

    public class ZingConsumer : IConsumer<Zing>
    {
        public async Task Consume(ConsumeContext<Zing> context)
        {
            using var client = new HttpClient();
            await client.GetAsync("https://google.com");
            await context.Publish(new Bing());
        }
    }
}