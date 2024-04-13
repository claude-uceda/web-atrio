using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Webatrio.Employee.Api.Modules;
using Webatrio.Employee.Core.Entities;
using Webatrio.Employee.Core.Repositories;
using Webatrio.Employee.Core.Repositories.InMemory;
using Webatrio.Employee.Services;

namespace Webatrio.Employee.Api.Startup
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            //todo use assembly scan

            services.TryAddScoped<IReadOnlyRepository<Person>>((_)=> new InMemoryRepository<Person>());            
            services.TryAddScoped<IRepository<Person>>((_)=> new InMemoryRepository<Person>());

            return services;
        }


        public static WebApplication MapEndpoints(this WebApplication app)
        {
            var modules = ServiceExtensions.Discover<IModule>();
            foreach (var module in modules)
            {
                module.MapEndpoints(app);
            }
            return app;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            var types = DiscoverTypes<IService>();
            var itypes = DiscoverInterfaces<IService>().Where(x => x != typeof(IService)).ToArray();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var itype in interfaces)
                {
                    if (!itype.IsGenericType)
                    {
                        if (itypes.Contains(itype))
                            services.TryAddScoped(itype, type);
                    }
                    else
                    {
                        var gtype = itype.GetGenericTypeDefinition();
                        if (itypes.Contains(gtype))
                            services.TryAddScoped(itype, type);
                    }
                }
            }

            foreach (var item in types)
                services.AddScoped(item, item);
            return services;
        }

        public static IEnumerable<Type> DiscoverTypes<TType>()
        {
            return typeof(TType).Assembly
                .GetTypes()
                .Where(p => p.IsClass && !p.IsAbstract && p.IsAssignableTo(typeof(TType)))
            ;
        }

        public static IEnumerable<Type> DiscoverInterfaces<TType>()
        {
            return typeof(TType).Assembly
                .GetTypes()
                .Where(p => p.IsInterface && p.IsAssignableTo(typeof(TType)))
            ;
        }

        public static IEnumerable<TModule> Discover<TModule>()
        {
            return DiscoverTypes<TModule>()
                .Select(Activator.CreateInstance)
                .Cast<TModule>()
            ;
        }
    }
}
