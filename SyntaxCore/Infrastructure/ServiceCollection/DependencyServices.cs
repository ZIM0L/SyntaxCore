namespace SyntaxCore.Infrastructure.ServiceCollection
{
    public static class DependencyServices
    {
        public static IServiceCollection AddDependencyService(this IServiceCollection services)
        {
            services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(typeof(DependencyServices).Assembly));
            return services;
        }
    }
}