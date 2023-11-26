using SharedLibrary.Configuration;
using PaymentApi.Services.Payments;
using PaymentApi.Services.Payments.Providers.Iyzico.Models;
using PaymentApi.Services.Payments.Providers.Iyzico;
using SharedLibrary.Models;
using SharedLibrary.Base.Services;
using PaymentApi.Services.Orders;
using PaymentApi.Services.Products;
using SharedLibrary.RabbitMQSender;
using PaymentApi.Services.ShoppingCard;
using PaymentApi.RabbitMQReceiver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SD.GatewayAPIBase = builder.Configuration["ServiceUrls:GatewayAPIBase"];

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rabbitMQSetting = new RabbitMQSetting();
builder.Configuration.GetSection(RabbitMQSetting.SECTION_NAME).Bind(rabbitMQSetting);
builder.Services.AddSingleton(rabbitMQSetting);
RabbitMQSetting.SetUpInstance(rabbitMQSetting);

var iyzicoPaymentSettings = new IyzicoPaymentSettings();
builder.Configuration.GetSection(IyzicoPaymentSettings.SECTION_NAME).Bind(iyzicoPaymentSettings);
builder.Services.AddSingleton(iyzicoPaymentSettings);
IyzicoPaymentSettings.SetUpInstance(iyzicoPaymentSettings);

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCardService, ShoppingCardService>();
builder.Services.AddScoped<IPaymentService, IyzicoPaymentService>();
builder.Services.AddSingleton<IRabbitMQSender, RabbitMQSender>();

builder.Services.AddHostedService<RabbitMQPaymentReceiver>();

builder.Services.AddSingleton(builder.Configuration.GetSection(IyzicoPaymentSettings.SECTION_NAME));
builder.Services.AddScoped<IPaymentService, IyzicoPaymentService>();

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
