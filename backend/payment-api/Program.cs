using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using payment_api.Application.Interfaces;
using payment_api.Application.Services;
using payment_api.Infrastructure.DB;
using payment_api.Infrastructure.Http;
using payment_api.Infrastructure.Repositories;

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

builder.Services.AddHttpClient<IOrderApiClient, OrderApiClient>(client =>
{
    var ordersApiBaseUrl = builder.Configuration["OrdersApi:BaseUrl"]!;
    client.BaseAddress = new Uri(ordersApiBaseUrl);
});

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

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
