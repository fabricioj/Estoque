using AutoMapper;
using Estoque.Api.Models;
using Estoque.Dados.Repositorios;
using Estoque.Dados.Utilidades;
using Estoque.Negocio.Entidades;
using Estoque.Negocio.Interfaces;
using Estoque.Negocio.Repositorios;
using Estoque.Negocio.Utilidades;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Estoque.Api.Utilities
{
    public static class InjecaoDependenciaExtension
    {
        public const string VersaoDoc = "v1";

        public static IServiceCollection AdicionarPrepararControllers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, true));
                    });

            services.ResolverDependencias(configuration);

            return services;
        }

        public static IServiceCollection ResolverDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EstoqueContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection"), options2 =>
                {
                    options2.MigrationsAssembly(typeof(EstoqueContext).Assembly.GetName().Name);
                    //options2.UseRelationalNulls(true);
                });
            });

            services.AddTransient<ITransacao, Transacao>();
            services.AddTransient(typeof(IBDRepositorio<>), typeof(BDRepositorio<>));
            services.AddTransient<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddTransient<IProdutoRepositorio, ProdutoRepositorio>();

            var mapperConfig = new MapperConfiguration(options =>
            {
                options.CreateMap<Categoria, CategoriaView>();
                options.CreateMap<Produto, ProdutoView>()
                       .ForMember((prodView) => prodView.CategoriaDescricao, (prop) => prop.MapFrom(prod => prod.Categoria != null ? prod.Categoria.Descricao : string.Empty));

                options.CreateMap<CategoriaView, Categoria>();
                options.CreateMap<ProdutoView, Produto>();
            });

            services.AddSingleton(mapperConfig.CreateMapper());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(VersaoDoc, new OpenApiInfo
                {
                    Title = "Gestão de estoque",
                    Version = VersaoDoc,
                    Description = "API REST para gestão de estoque"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
                
            });

            return services;
        }

        public static void ConfigurarDependencias(this IApplicationBuilder app, IConfiguration configuration, EstoqueContext estoqueContext)
        {
            estoqueContext.Database.Migrate();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{VersaoDoc}/swagger.json", "Gestão de estoque");
            });
        }

    }
}
