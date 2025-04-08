using AuthWebapi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthWebapi.ExtensionClasses
{
    public static class EFCoreExtensionConfig
    {
        public static IServiceCollection InjectDbContex(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DevDB")));

            return services;
        }
    }
}
