
using System;
using System.Collections.Generic;
using EduFairOS.Models;
using EduFairOS.Layers.Infrastructure.Data;

namespace EduFairOS.Layers.Application.Services
{
	public class ServicioActividad
	{
		private RepositorioActividad _repositorio;

		public ServicioActividad()
		{
			_repositorio = new RepositorioActividad();
		}

		public bool CrearActividad(Actividad actividad)
		{
			if (actividad == null) throw new ArgumentNullException(nameof(actividad));
			if (string.IsNullOrEmpty(actividad.Nombre))
				throw new Exception("El nombre de la actividad es obligatorio");
			return _repositorio.Agregar(actividad);
		}

		public Actividad ObtenerActividad(int id)
		{
			return _repositorio.ObtenerPorId(id);
		}

		public List<Actividad> ObtenerTodasActividades()
		{
			return _repositorio.ObtenerTodos();
		}

		public bool ActualizarActividad(Actividad actividad)
		{
			if (actividad == null || actividad.Id <= 0) return false;
			var existente = ObtenerActividad(actividad.Id);
			if (existente == null) return false;
			return _repositorio.Actualizar(actividad);
		}

		public bool EliminarActividad(int id)
		{
			var actividad = ObtenerActividad(id);
			if (actividad == null) return false;
			return _repositorio.Eliminar(id);
		}
	}
}