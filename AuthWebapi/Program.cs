using AuthWebapi.Controllers;
using AuthWebapi.ExtensionClasses;
using AuthWebapi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerExplorer()
    .InjectDbContex(builder.Configuration)
    .AddAppConfig(builder.Configuration)
    .AddIdentityHandlerAndStores()
    .ConfigureIdentityOptions()
    .AddIdentityAuth(builder.Configuration);


var app = builder.Build();

app.configureSwaggerExplorer()
    .ConfigCors(builder.Configuration)
    .AddIdentityAuthMiddleware();

app.MapControllers();

app.MapGroup("/api")
    .MapIdentityApi<AppUser>();

app.MapGroup("/api")
    .MapIdentityUserEndpoints()
    .MapAccountEndpoint()
    .MapAuthorizationDemoEndPoints();

app.Run();