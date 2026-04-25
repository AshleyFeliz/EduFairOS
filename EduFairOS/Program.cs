//Ashley Esmirna Feliz Rodríguez 2025-0903

/// Punto de entrada principal para la aplicación EduFairOS.
/// Esta clase configura el host de la aplicación web, registra servicios y define el pipeline de middleware.
using EduFairOS.Models;
using EduFairOS.Layers.Application.Contracts;
using EduFairOS.Layers.Application.Services;
using EduFairOS.Layers.Infrastructure.Data;
using EduFairOS.Layers.Infrastructure.Interfaces;
using EduFairOS.Layers.Persistences.Repositories;

/// Crea el constructor de la aplicación web.

var builder = WebApplication.CreateBuilder(args);

/// Agrega servicios de controladores para manejar solicitudes HTTP.
builder.Services.AddControllers();

/// Agrega servicios para explorar y generar documentación de API con Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// Agrega soporte para páginas Razor.
builder.Services.AddRazorPages();

/// Registra la conexión a la base de datos como singleton.
builder.Services.AddSingleton<ConexionBD>();

/// Registra repositorios en el contenedor de dependencias.
/// Cada repositorio implementa IRepositorio para las entidades correspondientes.
builder.Services.AddScoped<IRepositorio<Evento>, RepositorioEvento>();
builder.Services.AddScoped<IRepositorio<Participante>, RepositorioParticipante>();
builder.Services.AddScoped<IRepositorio<Stand>, RepositorioStand>();
builder.Services.AddScoped<IRepositorio<Actividad>, RepositorioActividad>();

/// Registra servicios de aplicación en el contenedor de dependencias.
/// Estos servicios contienen la lógica de negocio.
builder.Services.AddScoped<IServicioEvento, ServicioEvento>();
builder.Services.AddScoped<IServicioParticipante, ServicioParticipante>();
builder.Services.AddScoped<IServicioStand, ServicioStand>();
builder.Services.AddScoped<IServicioActividad, ServicioActividad>();

/// Construye la aplicación web con la configuración definida.
var app = builder.Build();

/// Configura el pipeline de middleware para el entorno de desarrollo.
/// Incluye Swagger para documentación de API.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

/// Fuerza el uso de HTTPS para todas las solicitudes.
app.UseHttpsRedirection();

/// Habilita el uso de archivos estáticos y el enrutamiento.
app.UseStaticFiles();
app.UseRouting();

/// Habilita la autorización para las rutas de la aplicación.
app.UseAuthorization();

/// Mapea los controladores y las páginas Razor a las rutas de la aplicación.
app.MapRazorPages();
app.MapControllers();

/// Ejecuta la aplicación.
app.Run();