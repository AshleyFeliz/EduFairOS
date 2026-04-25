//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;
using System.Linq;
using EduFairOS.Models;
using EduFairOS.Layers.Application.Contracts;
using EduFairOS.Layers.Infrastructure.Interfaces;

namespace EduFairOS.Layers.Application.Services
{
	
	/// Servicio de aplicación para manejar la lógica de negocio de participantes.
	/// Actúa como intermediaria entre la capa de presentación y la capa de datos.
	public class ServicioParticipante : IServicioParticipante
	{
		private readonly IRepositorio<Participante> _repositorio;

		/// Constructor que recibe un repositorio de participantes.
		
		public ServicioParticipante(IRepositorio<Participante> repositorio)
		{
			_repositorio = repositorio;
		}


		/// Método privado que determina la categoría de edad de un participante.
		
		private string ObtenerCategoria(int edad)
		{
			if (edad <= 12) return "Infantil";
			if (edad <= 17) return "Juvenil";
			return "Adulto";
		}

		/// Registra un nuevo participante aplicando validaciones de negocio.
		
		public bool RegistrarParticipante(Participante participante)
		{
			if (participante == null) throw new ArgumentNullException(nameof(participante));


			if (string.IsNullOrEmpty(participante.Nombre) || string.IsNullOrEmpty(participante.Institucion))
				throw new Exception("Los datos del participante son inválidos");

			if (participante.Edad < 5 || participante.Edad > 20)
				throw new Exception("La edad del participante debe estar entre 5 y 20 años");


			if (!string.IsNullOrEmpty(participante.Correo) && !participante.Correo.Contains("@"))
				throw new Exception("El correo del participante es inválido");

			participante.FechaRegistro = DateTime.Now;
			return _repositorio.Agregar(participante);
		}

		/// Obtiene un participante por su ID.
		
		public Participante ObtenerParticipante(int id)
		{
			Participante participante = _repositorio.ObtenerPorId(id);
			if (participante == null) throw new Exception($"No se encontró participante con ID {id}");
			return participante;
		}

		/// Obtiene todos los participantes activos.
		
		public List<Participante> ObtenerTodosParticipantes()
		{
			return _repositorio.ObtenerTodos();
		}

		/// Obtiene participantes filtrados por institución.
		
		public List<Participante> ObtenerPorInstitucion(string institucion)
		{
			return _repositorio.ObtenerPor(p => p.Institucion.Equals(institucion, StringComparison.OrdinalIgnoreCase));
		}

		/// Obtiene participantes filtrados por categoría de edad.
		
		public List<Participante> ObtenerPorCategoriaEdad(string categoria)
		{

			return _repositorio.ObtenerPor(p => ObtenerCategoria(p.Edad).Equals(categoria, StringComparison.OrdinalIgnoreCase));
		}

		/// Actualiza un participante existente aplicando validaciones de negocio.
		
		public bool ActualizarParticipante(Participante participante)
		{
			if (participante == null || participante.Id <= 0) return false;
			Participante existente = ObtenerParticipante(participante.Id);
			if (existente == null) return false;

			if (string.IsNullOrEmpty(participante.Nombre) || string.IsNullOrEmpty(participante.Institucion))
				throw new Exception("Los datos del participante son inválidos");

			return _repositorio.Actualizar(participante);
		}

		/// Elimina un participante por su ID.
		
		public bool EliminarParticipante(int id)
		{
			Participante participante = ObtenerParticipante(id);
			if (participante == null) return false;
			return _repositorio.Eliminar(id);
		}

		/// Busca participantes por nombre (búsqueda parcial).
		
		public List<Participante> BuscarPorNombre(string nombre)
		{
			return _repositorio.ObtenerPor(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
		}

		/// Genera estadísticas de los participantes registrados.
		
		public string GenerarEstadisticas()
		{
			var participantes = _repositorio.ObtenerTodos();
			string stats = "ESTADÍSTICAS DE PARTICIPANTES\n";
			stats += "=============================\n\n";
			stats += $"Total de participantes: {participantes.Count}\n";

			if (participantes.Count == 0) return stats;

			Dictionary<string, int> porCategoria = new Dictionary<string, int>();
			foreach (Participante p in participantes)
			{
				string cat = ObtenerCategoria(p.Edad);
				if (!porCategoria.ContainsKey(cat)) porCategoria[cat] = 0;
				porCategoria[cat]++;
			}

			stats += "\nPor Categoría de Edad:\n";
			foreach (var kvp in porCategoria)
			{
				stats += $"  {kvp.Key}: {kvp.Value}\n";
			}

			int sumaEdades = 0;
			foreach (Participante p in participantes) sumaEdades += p.Edad;
			double promedio = (double)sumaEdades / participantes.Count;
			stats += $"\nEdad Promedio: {promedio:F2} años\n";

			return stats;
		}
	}
}