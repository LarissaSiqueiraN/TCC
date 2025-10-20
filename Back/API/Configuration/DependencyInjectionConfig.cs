using Business.Intefaces;
using Business.Notificacoes;
using Business.Services;
using Business.Services.Interfaces;
using DAL;
using DAL.Repository;
using DAL.Repository.Interfaces;

namespace API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<BancoAPIContext>();

            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            services.AddScoped<IAnaliseService, AnaliseService>();

            services.AddScoped<IAnaliseRepository, AnaliseRepository>();

            return services;
        }
    }
}
