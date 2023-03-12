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

builder.Services.AddScoped<IBaseBL, BaseBL>();
//builder.Services.AddScoped<IConfiguration, UserLoginController>();
builder.Services.AddScoped<IUserLoginBL, UserLoginBL>();
builder.Services.AddScoped<ILoginDAL, UserLoginDAL>();
builder.Services.AddScoped<IBaseDAL, BaseDAL>();
builder.Services.AddScoped<IGeocodingService, GeocodingService>();

// INJECTIONS
builder.Services.AddScoped<DataContext>();
builder.Services.AddScoped<IGeocodingService, GeocodingService>();
builder.Services.AddScoped<IProsumerBL, ProsumerBL>();
builder.Services.AddScoped<IProsumerDAL, ProsumerDAL>();

//JWT Authenthication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
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

app.UseCors();

app.UseCors();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
