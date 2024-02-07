using Business.Intefaces;
using Business.Notificacoes;
using Business.Services;
using Business.Services.Base;
using Business.Services.Interfaces;
using Business.Services.ProdutoServices;
using Business.Services.ProdutoServices.Interfaces;
using DAL;
using DAL.Repository;
using DAL.Repository.Interfaces;
using DAL.Repository.ProdutoRepositories;
using DAL.Repository.ProdutoRepositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;



namespace API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<BancoAPIContext>();

            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<MailService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            services.AddScoped<IProdutoFotoService, ProdutoFotoService>();

            services.AddScoped<IProdutoFotoRepository, ProdutoFotoRepository>();

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            services.AddScoped<IEncomendaRepository, EncomendaRepository>();

            services.AddScoped<IProdutoService, ProdutoService>();

            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<IEncomendaService, EncomendaService>();

            return services;
        }
    }
}
