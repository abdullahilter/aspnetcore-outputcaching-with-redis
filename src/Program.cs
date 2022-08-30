using aspnetcore_outputcaching_with_redis;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TestContext>(options => options.UseInMemoryDatabase(nameof(TestContext)));
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("localhost"));

builder.Services.AddRedisOutputCache();

var app = builder.Build();

app.UseOutputCache();

app.RegisterCustomerEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();