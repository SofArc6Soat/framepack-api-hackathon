using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace UseCases.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddUseCasesDependencyServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoUseCase, ProdutoUseCase>();
            services.AddScoped<IClienteUseCase, ClienteUseCase>();
            services.AddScoped<IFuncionarioUseCase, FuncionarioUseCase>();
            services.AddScoped<IUsuarioUseCase, UsuarioUseCase>();
        }
    }
}