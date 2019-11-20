using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Settings
{
    public class HttpServer
    {
        /// <summary>
        /// will initialize when config 
        /// </summary>
        public int ListenPort { get; set; }

        public HttpServer(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void Configure(IApplicationBuilder builder)
        {
            builder.UseRouting();
//            builder.UseSession( new SessionOptions {IOTimeout = TimeSpan.FromMinutes(5)});
            builder.Map("/info", (option) => { option.UseMiddleware<MiddlewareInformation>(); });
            builder.UseMiddleware<MiddlewareInformation>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            return;
        }

        /// <summary>
        /// If we need our DI builder
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        //public IServiceProvider ConfigureServices(IServiceCollection services)
        //{
        //    return new DefaultServiceProviderFactory().CreateServiceProvider(services);
        //}
    }
}
