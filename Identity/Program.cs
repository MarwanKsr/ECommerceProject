using Identity.Configuration;
using Identity.DbContext;
using Identity.Initializer;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DBInitializer>();
builder.Services.AddScoped<ApplicationUserManager>();
builder.Services.AddScoped<ApplicationSignInManager>();
builder.Services.AddScoped<IAuthService, AuthService>();

var apiConfig = new ApiConfig();
builder.Configuration.GetSection(ApiConfig.SECTION_NAME).Bind(apiConfig);
builder.Services.AddSingleton(apiConfig);
ApiConfig.SetUpInstance(apiConfig);

//var appSettingsSection = builder.Configuration.GetSection("ApiConfig");
//builder.Services.Configure<ApiConfig>(appSettingsSection);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

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
