using OrderMailboxHub.Host.Extensions;
using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Seedwork.Application.Extensions;
using Seedwork.CrossCutting.Configuration;
using Seedwork.CrossCutting.SwaggerExtensions;
using Seedwork.Serilog.HttpContextLogger.Extensions;
using Seedwork.SeriLog.LevelSwitch.Extensions;
using Seedwork.SeriLog.LevelSwitch.Services;
using Seedwork.Version.Extensions;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Swashbuckle.AspNetCore.Swagger;
using System;
using Seedwork.CrossCutting.MVC.Extensions;
using OrderMailboxHub.Application.Services.OrderMailBox.Configuration;
using Seedwork.CrossCutting.HttpClientInvoker.Extensions;
using OrderMailboxHub.Application.Services.TPGESCOM.Configuration;
using OrderMailboxHub.Host.Filters;
using OrderMailboxHub.Host.Authorization;
using Seedwork.IAM.Services.Extensions;

namespace OrderMailboxHub.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            RedisCacheConfigurationOptions = Configuration.GetSection("RedisCache").Get<RedisCacheConfigurationOptions>();
            ElasticsearchConfigurationOptions = Configuration.GetSection("Elasticsearch").Get<ElasticsearchConfigurationOptions>();

            SetLogger();
        }

        public IConfiguration Configuration { get; }
        public RedisCacheConfigurationOptions RedisCacheConfigurationOptions { get; }
        public ElasticsearchConfigurationOptions ElasticsearchConfigurationOptions { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(o => o.AddScheme("apikey", a => a.HandlerType = typeof(ApiKeyHandler)));

            services
                .AddLogLevelSwitchServices()
                .AddConfigurationApplication()
                .AddIAMServices()
                .AddRestClientBase()
                .Configure<ApiKeyOptions>(Configuration.GetSection(nameof(ApiKeyOptions)))
                .Configure<OrderMailBoxApiConfiguration>(Configuration.GetSection(nameof(OrderMailBoxApiConfiguration)))
                .Configure<TPGESCOMApiConfiguration>(Configuration.GetSection(nameof(TPGESCOMApiConfiguration)))
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton(Configuration)
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("1.0.0", new Info
                    {
                        Title = "OrderMailboxHub Api",
                        Version = "1.0.0"
                    });

                    options.OperationFilter<ApiKeyHeaderFilter>();

                    options.DocumentFilter<LowercaseDocumentFilter>();
                    options.DocumentFilter<VersionDocumentFilter>();

                    options.OperationFilter<MultipleOperationsWithSameVerbFilter>();
                    options.OperationFilter<ApplicationAndApplicationUserWso2Auth>();

                    options.DescribeAllEnumsAsStrings();
                    options.DescribeAllParametersInCamelCase();
                })
                .AddCors(corsOptions =>
                {
                    corsOptions.AddPolicy("AllowAllOrigins", builder =>
                    {
                        builder
                        .WithOrigins("http://localhost", "http://*.telepizza.com", "http://*.telepizza.int", "http://*.telepizza.es")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                })
                .AddDistributedRedisCache(option =>
                {
                    option.Configuration = RedisCacheConfigurationOptions.Configuration;
                    option.InstanceName = RedisCacheConfigurationOptions.InstanceName;
                })
                .AddMvc(config =>
                {
                    config.Filters.Add(typeof(ExceptionFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

             app
                .UseSerilogHttpContextLogger()
                .UseLogLevelSwitch()
                .UseVersion()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "OrderMailboxHub Docs");
                })
                .UseAuthentication()
                .UseCors("AllowAllOrigins")
                .UseMvc();
        }

        private void SetLogger()
            => Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithCorrelationId()
                .Enrich.WithCorrelationIdHeader()
                .MinimumLevel.ControlledBy(LogLevelService.LoggingLevelSwitch)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(ElasticsearchConfigurationOptions.UriString))
                {
                    IndexFormat = ElasticsearchConfigurationOptions.IndexFormat,
                    AutoRegisterTemplate = ElasticsearchConfigurationOptions.AutoRegisterTemplate
                })
                .CreateLogger();
    }
}
