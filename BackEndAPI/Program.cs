using System.Text;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AppContext = DataAccess.AppContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IPlantManager, PlantManagerImpl>();
builder.Services.AddScoped<INotificationSender, HttpClientNotificationSender>();
builder.Services.AddScoped<IPlantPresetManager, PlantPresetManagerImpl>();
builder.Services.AddScoped<IPlantDataManager, PlantDataManagerImpl>();
builder.Services.AddScoped<IPlantDAO, PlantDAO>();
builder.Services.AddScoped<IPlantPresetDAO, PlantPresetDAO>();
builder.Services.AddScoped<IPlantDataDAO, PlantDataDAO>();
builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<IUserManager, UserManagerImpl>();
builder.Services.AddScoped<IDeviceDAO, DeviceDAO>();
builder.Services.AddScoped<IDeviceManager, DeviceManagerImpl>();
builder.Services.AddScoped<IActionsSender, HttpClientActionsSender>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
    options.AddPolicy("AllowIotBridgeOrigin",
        builder => builder.WithOrigins("http://iotbridgeserver:5024")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var app = builder.Build();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowFrontendOrigin");
app.UseCors("AllowIotBridgeOrigin");
app.UseAuthorization();
app.MapControllers();

app.Run();