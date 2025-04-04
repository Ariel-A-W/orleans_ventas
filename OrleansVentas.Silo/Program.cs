using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using System.Net;

var builder = Host.CreateDefaultBuilder()
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering() // Usa localhost como host del silo                  
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "OrleansVentas";
            })
            .Configure<EndpointOptions>(options =>
            {
                options.AdvertisedIPAddress = IPAddress.Loopback;
                options.GatewayPort = 30000; // Puerto para la puerta de enlace
                options.SiloPort = 11111; // Puerto para el silo
            });
    })
    .ConfigureLogging(logging => logging.AddConsole());

await builder.RunConsoleAsync();
