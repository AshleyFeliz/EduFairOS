//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;
using EduFairOS.Models;
using EduFairOS.Layers.Application.Contracts;
using EduFairOS.Layers.Infrastructure.Interfaces;


namespace EduFairOS.Layers.Application.Services
{
	
	/// Servicio que maneja las operaciones relacionadas con actividades.
	/// Implementa la lógica de negocio para crear, obtener, actualizar y eliminar actividades.
	
	public class ServicioActividad : IServicioActividad
	{
		private readonly IRepositorio<Actividad> _repositorio;

		/// Constructor que recibe un repositorio de actividades.
		
		public ServicioActividad(IRepositorio<Actividad> repositorio)
		{
			_repositorio = repositorio;
		}

		/// Crea una nueva actividad.
		/// Valida que la actividad no sea nula y que tenga un nombre.
		
		public bool CrearActividad(Actividad actividad)
		{
			if (actividad == null) throw new ArgumentNullException(nameof(actividad));
			if (string.IsNullOrEmpty(actividad.Nombre))
				throw new Exception("El nombre de la actividad es obligatorio");
			return _repositorio.Agregar(actividad);
		}

		/// Obtiene una actividad por su ID.
		
		public Actividad ObtenerActividad(int id)
		{
			return _repositorio.ObtenerPorId(id);
		}

		/// Obtiene todas las actividades.
		
		public List<Actividad> ObtenerTodasActividades()
		{
			return _repositorio.ObtenerTodos();
		}

		/// Actualiza una actividad existente.
		/// Valida que la actividad exista antes de actualizar.
		
		public bool ActualizarActividad(Actividad actividad)
		{
			if (actividad == null || actividad.Id <= 0) return false;
			var existente = ObtenerActividad(actividad.Id);
			if (existente == null) return false;
			return _repositorio.Actualizar(actividad);
		}

		/// Elimina una actividad por su ID.
		/// Valida que la actividad exista antes de eliminar.
		
		public bool EliminarActividad(int id)
		{
			var actividad = ObtenerActividad(id);
			if (actividad == null) return false;
			return _repositorio.Eliminar(id);
		}
	}
}