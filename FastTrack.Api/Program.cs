using FastTrack.Persistence;
using FastTrack.Persistence.Extensions;
using FastTrack.Persistence.Seed;
using FastTrack.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
