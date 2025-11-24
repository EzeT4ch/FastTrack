using FastTrack.Api.Extensions;
using FastTrack.Application.Extensions;
using FastTrack.Persistence;
using FastTrack.Persistence.Extensions;
using FastTrack.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      policy =>
                      {
                          // REEMPLAZA ESTE ORIGEN CON EL PUERTO DE TU CLIENTE (e.g., Angular/React)
                          policy.WithOrigins("http://localhost:4200",
                                             "https://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
builder.Services.AddOpenApi();
builder.Services.RegisterApplicationServices();

builder.Services.AddPersistence(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddRepositories();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    FastTrackDbContext context = scope.ServiceProvider.GetRequiredService<FastTrackDbContext>();
    await context.Database.MigrateAsync();
    if (app.Environment.IsEnvironment("Test"))
    {
        await FastTrackDbSeeder.SeedAsync(context);
        Console.WriteLine("[FastTrack] Seed de entorno de Test ejecutado correctamente ✅");
    }
}
app.UseCors("MyAllowSpecificOrigins");
app.MapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", async () =>
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<FastTrackDbContext>();
        await context.Database.MigrateAsync();


        await FastTrackDbSeeder.SeedAsync(context);
        Console.WriteLine($"[FastTrack] Seed ejecutado en entorno {app.Environment.EnvironmentName} ✅");
    }
});

app.UseHttpsRedirection();

app.Run();