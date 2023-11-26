using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderApi.DbContexts;
using OrderApi.Models;
using OrderApi.RabbitMQReceiver;
using OrderApi.Services.Orders;
using OrderApi.Services.Products;
using SharedLibrary.Base.Services;
using SharedLibrary.Configuration;
using SharedLibrary.Models;
using SharedLibrary.RabbitMQSender;
using SharedLibrary.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SD.GatewayAPIBase = builder.Configuration["ServiceUrls:GatewayAPIBase"];

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

builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IOrderCommandService, OrderCommandService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IRabbitMQSender, RabbitMQSender>();

builder.Services.AddHostedService<RabbitMQCheckoutReceiver>();

builder.Services.AddHttpClient();
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
