using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using order_api.Application.Interfaces;
using order_api.Application.Services;
using order_api.Infrastructure.DB;
using order_api.Infrastructure.Http;
using order_api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection("MongoConfig"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = sp.GetRequiredService<IOptions<MongoConfig>>().Value;
    return new MongoClient(config.ConnectionString);
});

builder.Services.AddHttpClient<IUserApiClient, UserApiClient>(client =>
{
    var usersApiBaseUrl = builder.Configuration["UsersApi:BaseUrl"]!;
    client.BaseAddress = new Uri(usersApiBaseUrl);
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();

app.Run();
