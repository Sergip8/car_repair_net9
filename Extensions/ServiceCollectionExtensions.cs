using Microsoft.EntityFrameworkCore;

public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCarRepairDbContext(this IServiceCollection services, string? connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                // Use In-Memory Database
                services.AddDbContext<CarRepairDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "CarRepairInMemoryDb"));
            }
            // else
            // {
            //     // Use SQL Server
            //     services.AddDbContext<CarRepairDbContext>(options =>
            //         options.UseInMemoryDatabase(connectionString, b => 
            //             b.MigrationsAssembly("CarRepairApi")));
            // }

            return services;
        }

        public static IServiceCollection AddCarRepairInMemoryDbContext(this IServiceCollection services)
        {
            services.AddDbContext<CarRepairDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "CarRepairInMemoryDb"));

            return services;
        }
    }