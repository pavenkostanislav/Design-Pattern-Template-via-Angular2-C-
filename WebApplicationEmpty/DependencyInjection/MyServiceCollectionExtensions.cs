using TEST.Managers;

namespace TEST.Extensions.DependencyInjection
{
    public static class MyServiceCollectionExtensions
    {
        public static IServiceCollection AddMyReplace(this IServiceCollection services)
        {
            services.AddScoped<IGridManager<Data.Models.Chat>, ChatManager>();
			return services;
        }

    }
}
