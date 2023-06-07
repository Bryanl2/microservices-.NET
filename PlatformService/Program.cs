using Data;
using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using SyncDataServices;
using SyncDataServices.Http;
using System;


var builder = WebApplication.CreateBuilder(args);
//ConfigurationManager Configuration = builder.Configuration;
IConfiguration Configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.Production.json")
                                    .Build();
IWebHostEnvironment env= builder.Environment;


// // Add services to the container.
if(env.IsProduction())
{
    Console.WriteLine("----> using in sql database");
    Console.WriteLine("----> this");
     builder.Services.AddDbContext<AppDbContext>(opt =>opt.UseSqlServer(Configuration["ConnectionString:PlatformsConn"]));    
}else
{
    Console.WriteLine("----> using in memory database");
    builder.Services.AddDbContext<AppDbContext>(opt =>opt.UseInMemoryDatabase("InMem"));    
}

builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient,HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen();

Console.WriteLine($"---> CommandService Endpoint {Configuration["CommandService"]}");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
PrepDb.PrepPolulation(app,env.IsProduction());
app.UseCors(x =>
{
    x.AllowAnyOrigin();
    x.AllowAnyHeader();
    x.AllowAnyMethod();
});
app.Run();
