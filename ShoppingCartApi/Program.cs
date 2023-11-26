using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Services;
using SharedLibrary.Configuration;
using SharedLibrary.Models;
using SharedLibrary.RabbitMQSender;
using SharedLibrary.Repository;
using ShoppingCardApi.Services.Products;
using ShoppingCartApi.DbContexts;
using ShoppingCartApi.Services.Cards;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SD.GatewayAPIBase = builder.Configuration["ServiceUrls:GatewayAPIBase"];

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICardCommandService, CardCommandService>();
builder.Services.AddScoped<ICardQueryService, CardQueryService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddScoped<IProductCommandService, ProductCommandService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddSingleton<IRabbitMQSender, RabbitMQSender>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var rabbitMQSetting = new RabbitMQSetting();
builder.Configuration.GetSection(RabbitMQSetting.SECTION_NAME).Bind(rabbitMQSetting);
builder.Services.AddSingleton(rabbitMQSetting);
RabbitMQSetting.SetUpInstance(rabbitMQSetting);

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
