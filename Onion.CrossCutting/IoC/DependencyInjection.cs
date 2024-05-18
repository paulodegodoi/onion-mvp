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
        // context and auto mapper
        services.AddDbContext<AppDbContext>();
        services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
        
        // base repositories e services
        services.AddScoped(typeof(IBaseServices<,>), typeof(BaseServices<,>));
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        
        // cliente
        services.AddScoped<IClienteServices, ClienteServices>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        
        // produto
        services.AddScoped<IProdutoServices, ProdutoServices>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        // shipping services
        services.AddScoped<IShippingServices, ShippingServices>();

        // viacep services
        services.AddScoped<ViaCepServices>();
        return services;
    }
}