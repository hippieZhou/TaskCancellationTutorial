using System.Diagnostics;
using CoreLib;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<MyHttpClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Middleware

app.Use(async (httpContext, next) =>
{
    var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
    var routeName = endpoint!.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName;
    var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(routeName!);
    
    var watch = Stopwatch.StartNew();
    await next();
    watch.Stop();
    
    logger.LogInformation($"{routeName}:{watch.ElapsedMilliseconds}");
});

#endregion

#region API

app.MapGet("/page", (MyHttpClient client) =>
{
    var html = client.GetHtmlString();
    return Results.Ok(new
    {
        html.Length,
        html
    });
}).WithName(nameof(MyHttpClient.GetHtmlString));

app.MapGet("/pageasync", async (MyHttpClient client) =>
{
    var html = await client.GetHtmlStringAsync();
    return Results.Ok(new
    {
        html.Length,
        html
    });
}).WithName(nameof(MyHttpClient.GetHtmlStringAsync));

#endregion

app.Run();