
using System;
using System.Collections.Generic;
using EduFairOS.Models;
using EduFairOS.Layers.Infrastructure.Data;

namespace EduFairOS.Layers.Application.Services
{
	public class ServicioParticipante
	{
		private RepositorioParticipante _repositorio;

		public ServicioParticipante()
		{
			_repositorio = new RepositorioParticipante();
		}

		// Método auxiliar para reemplazar ObtenerCategoriaEdad() que faltaba
		private string ObtenerCategoria(int edad)
		{
			if (edad <= 12) return "Infantil";
			if (edad <= 17) return "Juvenil";
			return "Adulto";
		}

		public bool RegistrarParticipante(Participante participante)
		{
			if (participante == null) throw new ArgumentNullException(nameof(participante));

			// Reemplazo de participante.ValidarDatos()
			if (string.IsNullOrEmpty(participante.Nombre) || string.IsNullOrEmpty(participante.Institucion))
				throw new Exception("Los datos del participante son inválidos");

			if (participante.Edad < 5 || participante.Edad > 20)
				throw new Exception("La edad del participante debe estar entre 5 y 20 años");

			// Reemplazo de participante.ValidarCorreo()
			if (!string.IsNullOrEmpty(participante.Correo) && !participante.Correo.Contains("@"))
				throw new Exception("El correo del participante es inválido");

			participante.FechaRegistro = DateTime.Now;
			return _repositorio.Agregar(participante);
		}

		public Participante ObtenerParticipante(int id)
		{
			Participante participante = _repositorio.ObtenerPorId(id);
			if (participante == null) throw new Exception($"No se encontró participante con ID {id}");
			return participante;
		}

		public List<Participante> ObtenerTodosParticipantes()
		{
			return _repositorio.ObtenerTodos();
		}

		public List<Participante> ObtenerPorInstitucion(string institucion)
		{
			return _repositorio.ObtenerPor(p => p.Institucion.Equals(institucion, StringComparison.OrdinalIgnoreCase));
		}

		public List<Participante> ObtenerPorCategoriaEdad(string categoria)
		{
			// Usamos nuestra función auxiliar adaptada
			return _repositorio.ObtenerPor(p => ObtenerCategoria(p.Edad).Equals(categoria, StringComparison.OrdinalIgnoreCase));
		}

		public bool ActualizarParticipante(Participante participante)
		{
			if (participante == null || participante.Id <= 0) return false;
			Participante existente = ObtenerParticipante(participante.Id);
			if (existente == null) return false;

			if (string.IsNullOrEmpty(participante.Nombre) || string.IsNullOrEmpty(participante.Institucion))
				throw new Exception("Los datos del participante son inválidos");

			return _repositorio.Actualizar(participante);
		}

		public bool EliminarParticipante(int id)
		{
			Participante participante = ObtenerParticipante(id);
			if (participante == null) return false;
			return _repositorio.Eliminar(id);
		}

		public List<Participante> BuscarPorNombre(string nombre)
		{
			return _repositorio.ObtenerPor(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
		}

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
				string cat = ObtenerCategoria(p.Edad); // Usamos el helper adaptado
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