using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderApi.Configuration;
using OrderApi.DbContexts;
using OrderApi.Models;
using OrderApi.RabbitMQReceiver;
using OrderApi.RabbitMQSender;
using OrderApi.Repository;
using OrderApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var rabbitMQSetting = new RabbitMQSetting();
builder.Configuration.GetSection(RabbitMQSetting.SECTION_NAME).Bind(rabbitMQSetting);
builder.Services.AddSingleton(rabbitMQSetting);
RabbitMQSetting.SetUpInstance(rabbitMQSetting);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IOrderCommandService, OrderCommandService>();
builder.Services.AddSingleton<IRabbitMQSender, RabbitMQSender>();

builder.Services.AddHostedService<RabbitMQCheckoutReceiver>();
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

app.Run();
