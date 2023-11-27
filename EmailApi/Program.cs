using SharedLibrary.Configuration;
using System.Net.Mail;
using System.Net;
using EmailApi.Services;
using EmailApi.RabbitMQReceiver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rabbitMQSetting = new RabbitMQSetting();
builder.Configuration.GetSection(RabbitMQSetting.SECTION_NAME).Bind(rabbitMQSetting);
builder.Services.AddSingleton(rabbitMQSetting);
RabbitMQSetting.SetUpInstance(rabbitMQSetting);

builder.Services.AddScoped<IEmailMessageSender, EmailMessageSender>();
builder.Services.AddHostedService<RabbitMQMailReceiver>();

builder.Services.AddHttpClient();


var fromName = builder.Configuration["EmailSender:FromName"];
var host = builder.Configuration["EmailSender:SMTP:Host"];
var port = int.Parse(builder.Configuration["EmailSender:SMTP:Port"]);
var email = builder.Configuration["EmailSender:SMTP:UserName"];
var password = builder.Configuration["EmailSender:SMTP:Password"];

builder.Services
    .AddFluentEmail(email, fromName) // base setup
    .AddSmtpSender(new SmtpClient()    // set email provider
    {
        Host = host,
        Port = port,
        Credentials = new NetworkCredential(email, password),
        EnableSsl = true,
    });

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
