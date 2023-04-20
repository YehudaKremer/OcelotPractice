using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotPractice.Services;

var builder = WebHost.CreateDefaultBuilder(args);

builder.UseKestrel(options =>
    {
        options.ConfigureHttpsDefaults(options =>
            options.ClientCertificateMode = ClientCertificateMode.AllowCertificate);
    })
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config
            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            .AddJsonFile("ocelot.json")
            .AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddOcelot();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerForOcelot(context.Configuration);
        services.AddAuthenticationServices(context);
    })
    .ConfigureLogging((hostingContext, logging) =>
    {
        //add your logging
    })
    .UseIISIntegration()
    .Configure(app =>
    {
        app.UseHttpsRedirection();

        app.UseSwaggerForOcelotUI(setupUiAction: uiOpt =>
        {
            uiOpt.DocumentTitle = "Gateway documentation";
        });

        app.UseAuthentication();
        app.UseOcelot().Wait();
    });

var app = builder.Build();

app.Run();
