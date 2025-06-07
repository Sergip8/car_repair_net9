 public static class DatabaseConfiguration
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddCarRepairDbContext(connectionString);
            services.AddScoped<IDbSeederService, DbSeederService>();
        }

        public static void ConfigureInMemoryDatabase(this IServiceCollection services)
        {
            services.AddCarRepairInMemoryDbContext();
            services.AddScoped<IDbSeederService, DbSeederService>();
        }

        public static async Task InitializeDatabase(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarRepairDbContext>();
            var seeder = scope.ServiceProvider.GetRequiredService<IDbSeederService>();

            // For SQL Server: Apply migrations
            if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                await context.Database.EnsureCreatedAsync();
            }
            // else
            // {
            //     await context.Database.MigrateAsync();
            //     // For In-Memory: Ensure database is created
            // }
            
            // Seed data
            await seeder.SeedAsync();
        }

        public static async Task InitializeInMemoryDatabase(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarRepairDbContext>();
            var seeder = scope.ServiceProvider.GetRequiredService<IDbSeederService>();

            // Ensure the in-memory database is created
            await context.Database.EnsureCreatedAsync();
            
            // Seed data
            await seeder.SeedAsync();
        }
    }
