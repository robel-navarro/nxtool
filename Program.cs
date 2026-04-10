using Microsoft.EntityFrameworkCore;
using nxtool;
using nxtool.Data;
using nxtool.Helpers;
using nxtool.Middleware;
using nxtool.Models;
using nxtool.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<NxManufService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new NxManufService(config);
});

builder.Services.AddDbContext<NxToolContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("NxToolDb")));

builder.Services.AddScoped<TokenService>();

Console.WriteLine($"SecretKey = {Environment.GetEnvironmentVariable("SecretKey")}");

builder.Services.Configure<SecretsConfig>(options =>
{
    options.SecretKey = Environment.GetEnvironmentVariable("SecretKey");
});
Console.WriteLine($"DB Path = {builder.Configuration.GetConnectionString("NxToolDb")}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<TokenMiddleware>();

app.MapControllers();

app.Run();
