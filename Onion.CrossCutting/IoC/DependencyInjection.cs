using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Onion.Application.Interfaces;
using Onion.Application.Mappings;
using Onion.Application.Services;
using Onion.Domain.Interfaces;
using Onion.Infrastructure.Context;
using Onion.Infrastructure.Repositories;

namespace Onion.CrossCutting.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
        
        // base
        services.AddScoped(typeof(IBaseServices<>), typeof(BaseServices<>));
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        
        // cliente
        services.AddScoped(typeof(IClienteServices), typeof(ClienteServices));
        services.AddScoped(typeof(IClienteRepository), typeof(ClienteRepository));
        
        // produto
        services.AddScoped(typeof(IProdutoServices), typeof(ProdutoServices));
        services.AddScoped(typeof(IProdutoRepository), typeof(ProdutoRepository));

        services.AddScoped<ViaCepServices>();
        return services;
    }
}