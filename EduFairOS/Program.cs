// Program.cs
using EduFairOS.Models;
using EduFairOS.Layers.Application.Services;
using EduFairOS.Layers.Infrastructure.Data;
using EduFairOS.Layers.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Repositories (Capa de Infraestructura)
builder.Services.AddScoped<IRepositorio<Evento>, RepositorioEvento>();
builder.Services.AddScoped<IRepositorio<Participante>, RepositorioParticipante>();
builder.Services.AddScoped<IRepositorio<Stand>, RepositorioStand>();
builder.Services.AddScoped<IRepositorio<Actividad>, RepositorioActividad>();

// Register Services (Capa de Aplicación)
builder.Services.AddScoped<ServicioEvento>();
builder.Services.AddScoped<ServicioParticipante>();
builder.Services.AddScoped<ServicioStand>();
builder.Services.AddScoped<ServicioActividad>();

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

app.Run();