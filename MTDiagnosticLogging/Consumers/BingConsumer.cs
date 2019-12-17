namespace MTDiagnosticLogging.Service.Consumers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MassTransit;
    using Messages;

    public class BingConsumer : IConsumer<Bing>
    {
        public async Task Consume(ConsumeContext<Bing> context)
        {
            using var client = new HttpClient();
            await client.GetAsync("https://google.com");
            await context.Publish(new Bong());
        }
    }
}