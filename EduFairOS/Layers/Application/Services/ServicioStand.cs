
using System;
using System.Collections.Generic;
using EduFairOS.Models;
using EduFairOS.Layers.Infrastructure.Data;

namespace EduFairOS.Layers.Application.Services
{
	public class ServicioStand
	{
		private RepositorioStand _repositorio;

		public ServicioStand()
		{
			_repositorio = new RepositorioStand();
		}

		public bool CrearStand(Stand stand)
		{
			if (stand == null) throw new ArgumentNullException(nameof(stand));
			if (string.IsNullOrEmpty(stand.Nombre))
				throw new Exception("El nombre del stand es obligatorio");
			return _repositorio.Agregar(stand);
		}

		public Stand ObtenerStand(int id)
		{
			return _repositorio.ObtenerPorId(id);
		}

		public List<Stand> ObtenerTodosStands()
		{
			return _repositorio.ObtenerTodos();
		}

		public bool ActualizarStand(Stand stand)
		{
			if (stand == null || stand.Id <= 0) return false;
			var existente = ObtenerStand(stand.Id);
			if (existente == null) return false;
			return _repositorio.Actualizar(stand);
		}

		public bool EliminarStand(int id)
		{
			var stand = ObtenerStand(id);
			if (stand == null) return false;
			return _repositorio.Eliminar(id);
		}
	}
}