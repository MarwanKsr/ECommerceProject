using Identity.Configuration;
using Identity.DbContext;
using Identity.Initializer;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
          options =>
          {
              options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;

              options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

              options.Lockout.MaxFailedAccessAttempts = 4;

              options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

              options.User.RequireUniqueEmail = false;

              options.SignIn.RequireConfirmedEmail = false;
              options.SignIn.RequireConfirmedPhoneNumber = false;

              options.Password.RequireDigit = true;
              options.Password.RequireLowercase = true;
              options.Password.RequireUppercase = true;
              options.Password.RequireNonAlphanumeric = false;
              options.Password.RequiredLength = 8;
          })
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

var key = Encoding.ASCII.GetBytes(apiConfig.SecretKey);

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == StatusCodes.Status200OK)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }
            return Task.CompletedTask;
        };
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
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

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetService<DBInitializer>();

    initializer.Seed().GetAwaiter().GetResult();
}

app.MapControllers();

app.Run();
