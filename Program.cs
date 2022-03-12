using Microsoft.EntityFrameworkCore;
using a02_shopsystem.Model;
using a02_shopsystem.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ShopsystemDatabase");

builder.Services.AddDbContext<ShopsystemContext>(
    options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var app = builder.Build();

// only deploy swagger in dev env
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();