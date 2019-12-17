namespace MTDiagnosticLogging.Service.Consumers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MassTransit;
    using Messages;

    public class DingConsumer : IConsumer<Ding>
    {
        public async Task Consume(ConsumeContext<Ding> context)
        {
            using var client = new HttpClient();
            await client.GetAsync("https://google.com");
            await context.Publish(new Zing());
        }
    }
}