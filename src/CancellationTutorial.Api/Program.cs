using CancellationTutorial.CoreLib;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddTransient<IRepository, Repository>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/weatherforecastwithCancel",
    async (CancellationToken cancellationToken, [FromServices] IRepository repository) =>
    {
        app.Logger.LogInformation("Starting to do slow work");
        var forecast = await repository.GetAsync(cancellationToken);
        app.Logger.LogInformation("Finished slow delay of some seconds.");

        return Results.Ok(new
        {
            count = forecast.Count(),
            forecast,
        });
    }).WithName("weatherforecastwithCancel");

app.Run();