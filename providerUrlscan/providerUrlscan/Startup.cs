using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using providerUrlscan.Configuration.ModelsOptions;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace providerUrlscan
{
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            ElasticsearchOptions = Configuration.GetSection("Elasticsearch").Get<ElasticsearchOptions>();

            SetLogger();
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        public ElasticsearchOptions ElasticsearchOptions { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Provider UrlsScan" });
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddControllers();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>  
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddSerilog();

            app.UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Provider UrlsScan");
                })
                .UseEndpoints(endpoints =>
                 {
                     endpoints.MapControllers();
                 });
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetLogger()
            => Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .MinimumLevel.Is(ElasticsearchOptions.MinimumLogEventLevel.Value)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(ElasticsearchOptions.UriString))
                {
                    IndexFormat = ElasticsearchOptions.IndexFormat,
                    AutoRegisterTemplate = ElasticsearchOptions.AutoRegisterTemplate
                })
                .CreateLogger();
    }
}
