using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using SkudWebApplication;
using SkudWebApplication.Behaviors;
using SkudWebApplication.Components;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Classes;
using SkudWebApplication.Services.Interfaces;
using NLog;
using NLog.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SkudWebApplication.Repos;
using Microsoft.AspNetCore.Components.Authorization;
using SkudWebApplication.States;
using System.Net;

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseNLog();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddOptions();
    builder.Services.AddAuthorizationCore();

    builder.Services.AddBlazorBootstrap();
    builder.Services.AddMudServices();
    builder.Services.AddTransient<MudLocalizer, RusMudLocalizer>();

    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    builder.Services.AddMediatR(x => {
        x.RegisterServicesFromAssembly(typeof(Program).Assembly);
    });

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddScoped<IAccessGroupService, AccessGroupService>();
    builder.Services.AddScoped<ICardService, CardService>();
    builder.Services.AddScoped<IControllerService, ControllerService>();
    builder.Services.AddScoped<IEventService, EventService>();
    builder.Services.AddScoped<ILocationService, LocationService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IWorkerGroupService, WorkerGroupService>();
    builder.Services.AddScoped<IWorkerService, WorkerService>();
    builder.Services.AddScoped<IAccount, Account>();

    builder.Services.AddSingleton<IApiDomainsManager, ApiDomainsManager>();
    builder.Services.AddScoped<IApiProvider, HttpClientApiProvider>();


    builder.Services.AddAutoMapper(typeof(AppMappingProfile));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();



    //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    //.AddCookie(options =>
    //{
    //    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    //    options.SlidingExpiration = true;
    //    options.AccessDeniedPath = "/Login";
    //    options.LoginPath = "/Login";
    //});



    builder.Services.AddDbContext<WebAppContext>(ConfigureWebAppContextConnection);

    void ConfigureWebAppContextConnection(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("Development"))
            .EnableSensitiveDataLogging();
    }

    builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseStatusCodePages(async context =>
    {
        var response = context.HttpContext.Response;

        if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
            response.StatusCode == (int)HttpStatusCode.Forbidden)
            response.Redirect("/Login");
    });

    app.UseStaticFiles();
    app.UseFileServer();
    app.UseAntiforgery();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}




