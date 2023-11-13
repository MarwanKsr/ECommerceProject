using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductApi.Configuration;
using ProductApi.DbContexts;
using ProductApi.Mapper;
using ProductApi.Repository.Images;
using ProductApi.Repository.Products;
using ProductApi.StorageFactory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
builder.Services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
builder.Services.AddScoped<IStorageServiceFactory, StorageServiceFactory>();
builder.Services.AddScoped<IMediaService,  MediaService>();

var hostAppSetting = new HostAppSetting();
builder.Configuration.GetSection(HostAppSetting.SECTION_NAME).Bind(hostAppSetting);
builder.Services.AddSingleton(hostAppSetting);
HostAppSetting.SetUpInstance(hostAppSetting);

IMapper mapper = ProductMapperProfiles.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
