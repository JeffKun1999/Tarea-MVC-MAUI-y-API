using API_desde_cero_JR.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using API_desde_cero_JR.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<API_desde_cero_JRContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BDs_JR_para_API_desde_ceroContext") ?? throw new InvalidOperationException("Connection string 'API_desde_cero_JRContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapBurgerEndpoints();

app.MapPromoEndpoints();



app.Run();
