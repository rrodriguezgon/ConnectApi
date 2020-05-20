using OrderMailboxHub.Host.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Seedwork.Application.Extensions;
using Seedwork.SeriLog.LevelSwitch.Extensions;
using Seedwork.Version.Extensions;
using System;

namespace OrderMailboxHub.Tests.IntegrationTests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddLogLevelSwitchServices()
                .AddConfigurationApplication()
                .AddMgmtApplication()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton(Configuration)
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        public void Configure(IApplicationBuilder app)
        {
            
            app
                .UseLogLevelSwitch()
                .UseVersion()
                .UseAuthentication()
                .UseMvc();
        }
    }
}
