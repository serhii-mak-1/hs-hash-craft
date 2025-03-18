using HashCraft.API.Configuration;
using HashCraft.API.Services;
using HashCraft.API.Services.HashGenerationService;
using HashCraft.API.Tools;
using HashCraft.API.Tools.Sha1HashGenerator;
using HashCraft.Infrastructure.Configuration;
using HashCraft.Infrastructure.MessageServices;
using HashCraft.Infrastructure.MessageServices.RabbitMQ;
using HashCraft.Storage.DAL;
using HashCraft.Storage.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.Configure<HashCraftApiConfig>(builder.Configuration.GetSection("HashCraftApiConfig"));
builder.Services.Configure<InfrastructureConfig>(builder.Configuration.GetSection("InfrastructureConfig"));

builder.Services.AddControllers();

builder.Services.AddScoped<IHashGenerationService, HashGenerationService>();
builder.Services.AddScoped<IHashGenerator, Sha1Generator>();
builder.Services.AddScoped<IPublisherFactory, RabbitMqPublisherFactory>();

builder.Services.AddSingleton<DbContextFactory>();
builder.Services.AddSingleton<HashStorageService>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v0.0.1");
});

app.Run();
