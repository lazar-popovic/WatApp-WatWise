using API.BL.Implementations;
using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
using API.Services.Geocoding.Implementations;
using API.Services.Geocoding.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using API.Services.DeviceSimulatorService.Implementation;
using API.Services.DeviceSimulatorService.Interfaces;
using API.Services.JWTCreation.Interfaces;
using API.Services.JWTCreation.Implementations;
using API.Services.E_mail.Interfaces;
using API.Services.E_mail.Implementations;
using Hangfire;
using Hangfire.Storage.SQLite;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHangfire(hangfire => 
{
    hangfire.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
    hangfire.UseSimpleAssemblyNameTypeSerializer();
    hangfire.UseRecommendedSerializerSettings();
    hangfire.UseColouredConsoleLogProvider();
    hangfire.UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection"));

    RecurringJob.AddOrUpdate<IDeviceSimulatorService>(x => x.HourlyUpdate(), "0 0 * ? * *");
});

builder.Services.AddHangfireServer();


builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite( builder.Configuration.GetConnectionString("DefaultConnection")));

var ipRateLimitingSection = builder.Configuration.GetSection("IpRateLimiting");
var ipRateLimitPoliciesSection = builder.Configuration.GetSection("IpRateLimitPolicies");


//RATE LIMITING
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(ipRateLimitingSection);
builder.Services.Configure<IpRateLimitPolicies>(ipRateLimitPoliciesSection);
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();


// INJECTIONS
builder.Services.AddScoped<DataContext>();
builder.Services.AddScoped<IGeocodingService, GeocodingService>();
builder.Services.AddScoped<IJWTCreator, JWTCreator>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IUserDAL, UserDAL>();
builder.Services.AddScoped<IDeviceSimulatorService, DeviceSimulatorService>();
builder.Services.AddScoped<IDeviceDAL, DeviceDAL>();
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IAuthDAL, AuthDAL>();
builder.Services.AddScoped<IAuthBL, AuthBL>();
builder.Services.AddScoped<IDeviceBL, DeviceBL>();
builder.Services.AddScoped<ILocationDAL, LocationDAL>();
builder.Services.AddTransient<IRecurringJobManager, RecurringJobManager>();

//JWT Authenthication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});




var app = builder.Build();

app.UseIpRateLimiting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();
app.MapHangfireDashboard();


app.Run();
