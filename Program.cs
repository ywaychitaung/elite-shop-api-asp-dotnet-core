using System.Text;
using AutoMapper;
using elite_shop.Data;
using elite_shop.Helpers;
using elite_shop.Mapper;
using elite_shop.Models.Settings;
using elite_shop.Repositories.Implementations;
using elite_shop.Repositories.Interfaces;
using elite_shop.Services.ModelServices.Implementations;
using elite_shop.Services.ModelServices.Interfaces;
using elite_shop.Services.UtilityServices.Implementations;
using elite_shop.Services.UtilityServices.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Set up configuration based on environment
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Register RateLimitSettings as a strongly typed configuration
builder.Services.Configure<RateLimitSetting>(builder.Configuration.GetSection("RateLimitSetting"));

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDatabase")));

// Register Redis as a singleton service
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("localhost:6379", true);
    return ConnectionMultiplexer.Connect(configuration);
});

// Register services and repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginRateLimitService, LoginRateLimitService>();

// Register helpers
builder.Services.AddScoped<EncryptionHelper>();
builder.Services.AddScoped<JwtHelper>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication configuration
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

// Register AutoMapper with your profiles manually
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new UserMapper());  // Add your mapping profiles here
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Load Kestrel settings from appsettings.json for development only
if (builder.Environment.IsDevelopment())
{
    // Load Kestrel settings from appsettings.Development.json
    var kestrelConfig = builder.Configuration.GetSection("Kestrel:Endpoints:Http:Url").Value;
    if (!string.IsNullOrEmpty(kestrelConfig))
    {
        builder.WebHost.UseUrls(kestrelConfig);
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
