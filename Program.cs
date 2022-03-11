using Microsoft.EntityFrameworkCore;
using a02_shopsystem.Model;
using a02_shopsystem.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("ShopsystemDatabase");

builder.Services.AddDbContext<ShopsystemContext>(
    options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var app = builder.Build();
app.MapControllers();
app.Run();