using System.Text;
using AuthWebapi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthWebapi.ExtensionClasses
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlerAndStores(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<AppUser>()
                .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();
            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;

            });
            return services;
        }


        public static IServiceCollection AddIdentityAuth(
            this IServiceCollection services,
            IConfiguration config)
        {

            var jwtSecret = config["AppSettings:JWTSecret"];
            var validIssuer = config["AppSettings:Issuer"];
            var validAudience = config["AppSettings:Audience"];

            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new Exception("JWT Secret Key is missing in configuration.");
            }

            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

            var appSettings = new AppSettings()
            {
                signInKey = signInKey,
                validIssuer = validIssuer,
                validAudience =validAudience

            };

            services.AddSingleton(appSettings);

            // 🔹 Add Authentication & Authorization
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,  // Change to `true` if you have an issuer
                    ValidateAudience = true, // Change to `true` if you have an audience
                    ValidateLifetime = true,  // Reject expired tokens
                   ValidIssuer = appSettings.validIssuer,
                   ValidAudience=appSettings.validAudience,
                    IssuerSigningKey = signInKey,
                    ClockSkew = TimeSpan.Zero // No tolerance for expired tokens
                };
            });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy=new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

                options.AddPolicy("HasLibraryId", policy => policy.RequireClaim("libraryID"));
                options.AddPolicy("FemalesOnly", policy => policy.RequireClaim("gender", "female"));
                options.AddPolicy("Under10", policy => policy.RequireAssertion(context =>
                {
                    var ageClaim = context.User.Claims.FirstOrDefault(x => x.Type == "age");
                    if (ageClaim == null) return false;

                    return int.TryParse(ageClaim.Value, out int age) && age < 10;
                }));


            });

            return services;
        }

        public static WebApplication AddIdentityAuthMiddleware(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
