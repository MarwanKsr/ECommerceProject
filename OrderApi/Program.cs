using OrderApi.Configuration;
using OrderApi.RabbitMQReceiver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<RabbitMQCheckoutReceiver>();


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
