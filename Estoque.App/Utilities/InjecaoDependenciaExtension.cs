using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.App.Utilities
{
    public static class InjecaoDependenciaExtension
    {
        public static IServiceCollection ResolverDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(RestService.For<IEstoqueApi>(configuration["urlApi"], new RefitSettings
            {
                ExceptionFactory = (message) => Task.FromResult<Exception>(null)
            }));


            return services;
        }
    }
}
