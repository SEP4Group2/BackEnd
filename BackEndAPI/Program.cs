using DataAccess;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using AppContext = DataAccess.AppContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//3000
// builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddDbContext<AppContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IPlantManager, PlantManagerImpl>();
builder.Services.AddScoped<IPlantPresetManager, PlantPresetManagerImpl>();
builder.Services.AddScoped<IPlantDAO, PlantDAO>();
builder.Services.AddScoped<IPlantPresetDAO, PlantPresetDAO>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();