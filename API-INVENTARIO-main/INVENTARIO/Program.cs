using INVENTARIO;
using INVENTARIO.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var cadena = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
builder.Services.AddDbContext<SampleContext>(
    options =>
    {
        options.UseSqlServer(cadena);
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<cifrado>();


List<string> CorsOriginAllowed = builder.Configuration.GetSection("AllowedOrigins").Get<List<string>>();
string[] origins = CorsOriginAllowed != null ? CorsOriginAllowed.ToArray() : new string[] { "*" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .WithOrigins(origins)
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        );
});
//builder.Services.AddScoped<GenerarCodigoCupon>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "INVENTARIO v1"));
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
