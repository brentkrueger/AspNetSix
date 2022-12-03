using WeatherAspNet.Data;
using WeatherAspNet.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace WeatherAspNet;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Month));

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddRazorPages();

        builder.Services.AddApplicationInsightsTelemetry();

        ConfigureIdentity(builder);
        ConfigureDI(builder);
        ConfigureAuthentication(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }

    private static void ConfigureIdentity(WebApplicationBuilder builder)
    {
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }

    private static void ConfigureDI(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(new ClientCredentialsTokenRequest
        {
            Address = new string(builder.Configuration["IdentityServerBaseUrl"] + "connect/token"),
            ClientId = "aspnetWebApp",
            ClientSecret = builder.Configuration["AspnetWebAppClientSecret"],
            Scope = "read:weatherforecast"
        });

        builder.Services.AddHttpClient<IWeatherService, WebApiWeatherService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["WeatherWebApiBaseUrl"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }).AddHttpMessageHandler<ProtectedApiBearerTokenHandler>();

        // The DelegatingHandler has to be registered as a Transient Service
        builder.Services.AddTransient<ProtectedApiBearerTokenHandler>();
        
        builder.Services.AddHttpClient<IIdentityServerClient, IdentityServerClient>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["IdentityServerBaseUrl"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

    }

    private static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication()
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });
    }
}