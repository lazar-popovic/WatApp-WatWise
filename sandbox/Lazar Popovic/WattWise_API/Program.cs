using Microsoft.EntityFrameworkCore;
using WattWise_API.BL.Implementation;
using WattWise_API.BL.Interface;
using WattWise_API.DAL.Implementation;
using WattWise_API.DAL.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBaseBL, BaseBL>();
builder.Services.AddScoped<IBaseDAL, BaseDAL>();
builder.Services.AddDbContext<BaseDAL>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddPolicy(name: "ActorsOrigins",
   policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ActorsOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
