using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexCart.Application.Common.Interfaces;
using NexCart.Domain.Interfaces;
using NexCart.Infrastructure.Identity;
using NexCart.Infrastructure.Persistence;
using NexCart.Infrastructure.Persistence.Repositories;

namespace NexCart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Use SQLite for easy local development (no Docker/PostgreSQL required)
        services.AddDbContext<NexCartDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection") 
                ?? "Data Source=nexcart.db"));

        // Repositories & Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // JWT
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
