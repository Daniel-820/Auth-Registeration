using AuthWebapi.Models;

namespace AuthWebapi.ExtensionClasses
{
    public static class AppConfigExtension
    {
        public static WebApplication ConfigCors(this WebApplication app
            ,IConfiguration config)
        {
            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200", "http://192.168.29.239:443")
            .AllowAnyMethod()
            .AllowAnyHeader()
            );

            return app;
        }

        public static IServiceCollection AddAppConfig(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.Configure<AppSettings>(config.GetSection("AppSettings"));

            return services;

        }
    }
}