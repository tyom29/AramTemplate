using Aram.BFF.Api;
using Aram.BFF.Application;
using Aram.BFF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddApplicationInsightsTelemetry();
}

var app = builder.Build();
{
    app.UseExceptionHandler();
    app.AddInfrastructureMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}