using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Net;

namespace Settings
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseLibuv()
                .UseKestrel((kestrelOptions) => // Specify Kestrel as the server to be used by the web host.
                {
                    kestrelOptions.AddServerHeader = true;
                    kestrelOptions.ListenAnyIP( 10000, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });

                })
                //.ConfigureKestrel((kestrelOptions) =>   // Configures Kestrel options but does not register an IServer.
                //{
                //    kestrelOptions.AddServerHeader = true;
                //    kestrelOptions.ListenAnyIP( 10000, listenOptions =>
                //    {
                //        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                //    });
                //})
                .UseUrls(new UriBuilder("http", Dns.GetHostName(), 10000).Uri.ToString().ToLower() )
                .UseStartup<HttpServer>()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();

                    config.AddJsonFile("Config.json", optional: true, reloadOnChange: true);

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    // load settings to Kestrel server from Config.json section Kestrel 
                    services.Configure<KestrelServerOptions>(hostingContext.Configuration.GetSection("Kestrel"));
                    services.Configure<HttpServer>(hostingContext.Configuration.GetSection("Server"));

                    services.AddRouting();

                    services.AddSingleton<HttpServer>();
                })
                .ConfigureLogging( configureLogging: (hostingContext, logging) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom
                        .Configuration(hostingContext.Configuration, "Serilog")
                        .CreateLogger();
                    logging.AddSerilog(dispose: true);
                })
                .UseSerilog()
                .Build();

            builder.Start();
        }
    }
}
