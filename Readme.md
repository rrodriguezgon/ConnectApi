# Configuración

Project: Host
File: appsettings.json

Configuration:

 "OrderMailBoxApiConfiguration": {
    "BaseUrl": "http://helios.telepizza.corp:90",
    "EndPoints": {
      "orders": "api/orders",
      "usersshops": "api/usersshops"
    },
    "ApiGatewayConfiguration": {
      "BaseUrl": "https://api.dev.aws.telepizza.com",
      "Resource": "token",
      "ConsumerKey": "kDa6waaIQfftAgNzY6zMUCtj9wka",
      "ConsumerSecret": "nZJhGP3QPOOewZATf9mi5kPsdnIa"
    }
  }

File: Startup.cs

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

 ----------------------------------------------------------

Project: Application

Folder: Services/Common
Folder: Services/OrderMailBox

---------------------------

Project: Api

File: OrdersController.cs