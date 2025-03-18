using HashCraft.Infrastructure.Configuration;
using HashCraft.Infrastructure.MessageServices;
using HashCraft.Infrastructure.MessageServices.RabbitMQ;
using HashCraft.Storage.DAL;
using HashCraft.Storage.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DbContextFactory>();
builder.Services.AddSingleton<HashStorageService>();
builder.Services.AddHostedService<HashRecieverHostedService>();

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.Configure<InfrastructureConfig>(builder.Configuration.GetSection("InfrastructureConfig"));
builder.Services.AddTransient<ISubscriberFactory, RabbitMqSubscriberFactory>();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => 
    optionsBuilder.UseMySql(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"],
        new MySqlServerVersion(new Version(10, 5))));

var app = builder.Build();

app.UseHttpsRedirection();
app.Run();
