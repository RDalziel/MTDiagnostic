namespace MTDiagnosticLogging.Service
{
    using System;
    using MassTransit;
    using MassTransit.Azure.ServiceBus.Core;
    using MassTransit.Context;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .UseWindowsService()
                       .ConfigureAppConfiguration(cfg =>
                       {
                           cfg.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                       })
                       .ConfigureLogging((context, builder) =>
                       {
                           builder.AddConsole();
                           builder.SetMinimumLevel(LogLevel.Trace);
                       })
                       .ConfigureServices((hostContext, services) =>
                       {
                           var provider      = services.BuildServiceProvider();
                           var loggerFactory = provider.GetService<ILoggerFactory>();

                           var hostConfig = hostContext.Configuration;
                           services.AddApplicationInsightsTelemetryWorkerService("b0a71c53-667e-4b17-bf49-191ac97521ad");
                           services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((m, o) =>
                           {
                               m.IncludeDiagnosticSourceActivities.Add("MassTransit");
                               m.IncludeDiagnosticSourceActivities.Remove("Microsoft.Azure.ServiceBus");
                               m.IncludeDiagnosticSourceActivities.Remove("Microsoft.Azure.EventHubs");
                           });
                           LogContext.ConfigureCurrentLogContext(loggerFactory);
                           services.AddMassTransit(x =>
                           {

                               x.AddConsumersFromNamespaceContaining<PingConsumer>();
                               x.AddBus(context => Bus.Factory.CreateUsingAzureServiceBus(
                                                                                          cfg =>
                                                                                          {
                                                                                              var host =
                                                                                                  cfg.Host("Endpoint=sb://mtdiagnost.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=pjb+3aGICpYrYeB+UbMhqo5BMUD9HPlnNOLB6zDjmHQ=",
                                                                                                           h => { });

                                                                                              cfg.ReceiveEndpoint("ping",
                                                                                                                  re =>
                                                                                                                  {
                                                                                                                      re.Consumer<PingConsumer>(context);
                                                                                                                  });
                                                                                          }));
                           });

                           services.AddHostedService<WindowsService>();
                       });
        }
    }
}