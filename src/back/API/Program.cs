using API.API;
using API.BL.Implementations;
using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
using API.Services.Geocoding.Implementations;
using API.Services.Geocoding.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using API.Services.Geocoding.Implementations;
using API.Services.Geocoding.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using API.Services.JWTCreation.Interfaces;
using API.Services.JWTCreation.Implementations;
using API.Services.E_mail.Interfaces;
using API.Services.E_mail.Implementations;

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

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite( builder.Configuration.GetConnectionString("DefaultConnection")));

// INJECTIONS
builder.Services.AddScoped<DataContext>();
builder.Services.AddScoped<IGeocodingService, GeocodingService>();
builder.Services.AddScoped<IProsumerBL, ProsumerBL>();
builder.Services.AddScoped<IProsumerDAL, ProsumerDAL>();
builder.Services.AddScoped<IDsoBL, DsoBL>();
builder.Services.AddScoped<IDsoDAL, DsoDAL>();
builder.Services.AddScoped<IBaseBL, BaseBL>();
builder.Services.AddScoped<IBaseDAL, BaseDAL>();
builder.Services.AddScoped<IJWTCreator, JWTCreator>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IUserDAL, UserDAL>();
builder.Services.AddScoped<IDeviceDAL, DeviceDAL>();

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

app.Run();
