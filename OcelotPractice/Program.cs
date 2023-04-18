using Microsoft.AspNetCore;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebHost.CreateDefaultBuilder(args);

builder.UseKestrel()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config
            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            .AddOcelotWithSwaggerSupport((o) =>
            {
                o.Folder = "Configuration";
            })
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddOcelot();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerForOcelot(context.Configuration);
    })
    .ConfigureLogging((hostingContext, logging) =>
    {
        //add your logging
    })
    .UseIISIntegration()
    .Configure(app =>
    {
        app.UseSwaggerForOcelotUI(opt =>
        {
            // swaggerForOcelot options
        }, uiOpt =>
        {
            //swaggerUI options
            uiOpt.DocumentTitle = "Gateway documentation";
        });

        app.UseOcelot().Wait();
    });

var app = builder.Build();

app.Run();
