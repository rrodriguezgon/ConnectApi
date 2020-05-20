using Microsoft.Extensions.DependencyInjection;
using Seedwork.Data;
using Seedwork.Domain;
using System.Linq;
using System.Reflection;

namespace OrderMailboxHub.Host.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurationApplication(this IServiceCollection services)
        {
            var namespacePrefix = $"{nameof(OrderMailboxHub)}";

            var applicationAssembly = Assembly.Load(new AssemblyName($"{namespacePrefix}.{nameof(Application)}"));

            return services
                .Scan(typeSourceSelector => typeSourceSelector
                    .FromAssemblies(applicationAssembly)
                        .AddClasses()
                            .AsMatchingInterface()
                            .WithScopedLifetime()
                );
        }
    }
}
