//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;
using System.Linq;
using EduFairOS.Models;
using EduFairOS.Layers.Application.Contracts;
using EduFairOS.Layers.Infrastructure.Interfaces;

/// Espacio de nombres para los servicios de aplicación de EduFairOS.

namespace EduFairOS.Layers.Application.Services
{
	/// Servicio de aplicación para manejar la lógica de negocio de stands.
	/// Actúa como intermediaria entre la capa de presentación y la capa de datos.
	
	public class ServicioStand : IServicioStand
	{
		private readonly IRepositorio<Stand> _repositorio;

		/// Constructor que recibe un repositorio de stands.
		
		public ServicioStand(IRepositorio<Stand> repositorio)
		{
			_repositorio = repositorio;
		}
		/// Crea un nuevo stand aplicando validaciones de negocio.
		
		public bool CrearStand(Stand stand)
		{
			if (stand == null) throw new ArgumentNullException(nameof(stand));
			if (string.IsNullOrEmpty(stand.Nombre))
				throw new Exception("El nombre del stand es obligatorio");
			return _repositorio.Agregar(stand);
		}

		/// Obtiene un stand por su ID.
		
		public Stand ObtenerStand(int id)
		{
			return _repositorio.ObtenerPorId(id);
		}

		/// Obtiene un stand por su nombre.
		
		public Stand ObtenerStand(string nombre)
		{
			if (string.IsNullOrWhiteSpace(nombre)) return null;
			return _repositorio.ObtenerPor(s => s.Nombre.Equals(nombre.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
		}

		/// Obtiene todos los stands activos.
		
		public List<Stand> ObtenerTodosStands()
		{
			return _repositorio.ObtenerTodos();
		}

		/// Actualiza un stand existente.
		
		public bool ActualizarStand(Stand stand)
		{
			if (stand == null || stand.Id <= 0) return false;
			var existente = ObtenerStand(stand.Id);
			if (existente == null) return false;
			return _repositorio.Actualizar(stand);
		}

		/// Elimina un stand por su ID.
		
		public bool EliminarStand(int id)
		{
			var stand = ObtenerStand(id);
			if (stand == null) return false;
			return _repositorio.Eliminar(id);
		}
	}
}