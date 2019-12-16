namespace MTDiagnosticLogging.WebService
{
    using System;
    using MassTransit;
    using MassTransit.Azure.ServiceBus.Core;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((m, o) => m.IncludeDiagnosticSourceActivities.Add("MassTransit"));
            services.AddApplicationInsightsTelemetry("b0a71c53-667e-4b17-bf49-191ac97521ad");
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingAzureServiceBus(cfg =>
                {
                    var host = cfg.Host("Endpoint=sb://mtdiagnost.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=pjb+3aGICpYrYeB+UbMhqo5BMUD9HPlnNOLB6zDjmHQ=",
                                        h =>
                                        {
                                            h.OperationTimeout = TimeSpan.FromSeconds(30);
                                        });
                }));
            });
            services.AddHostedService<BusService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}