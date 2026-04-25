//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;
using System.Linq;
using EduFairOS.Models;
using EduFairOS.Layers.Application.Contracts;
using EduFairOS.Layers.Infrastructure.Interfaces;


namespace EduFairOS.Layers.Application.Services
{
	/// Servicio de aplicación para manejar la lógica de negocio de eventos.
	/// Actúa como intermediaria entre la capa de presentación y la capa de datos.
	
	public class ServicioEvento : IServicioEvento
	{
		private readonly IRepositorio<Evento> _repositorio;

		/// Constructor que recibe un repositorio de eventos.
		public ServicioEvento(IRepositorio<Evento> repositorio)
		{
			_repositorio = repositorio;
		}

	
		/// Crea un nuevo evento aplicando validaciones de negocio.
		public bool CrearEvento(Evento evento)
		{
			if (evento == null) throw new ArgumentNullException(nameof(evento));
			if (string.IsNullOrEmpty(evento.Nombre) || evento.Nombre.Length < 3)
				throw new Exception("El nombre del evento debe tener al menos 3 caracteres");


			if (evento.FechaInicio >= evento.FechaFin)
				throw new Exception("La fecha de inicio debe ser anterior a la fecha de fin");

			if (evento.FechaInicio < DateTime.Now.Date)
				throw new Exception("La fecha de inicio no puede ser en el pasado");

			if (string.IsNullOrEmpty(evento.Lugar))
				throw new Exception("El lugar del evento es obligatorio");

			return _repositorio.Agregar(evento);
		}

		/// Obtiene un evento por su ID.
	
		public Evento ObtenerEvento(int id)
		{
			if (id <= 0) throw new ArgumentException("El ID debe ser mayor a 0");
			Evento evento = _repositorio.ObtenerPorId(id);
			if (evento == null) throw new Exception($"No se encontró evento con ID {id}");
			return evento;
		}
		/// Obtiene un evento por su nombre.
		
		public Evento ObtenerEvento(string nombre)
		{
			if (string.IsNullOrWhiteSpace(nombre)) return null;
			return _repositorio.ObtenerPor(e => e.Nombre.Equals(nombre.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
		}
		/// Obtiene todos los eventos activos.
		
		public List<Evento> ObtenerTodosEventos()
		{
			return _repositorio.ObtenerTodos();
		}

		/// Actualiza un evento existente aplicando validaciones de negocio.
		
		public bool ActualizarEvento(Evento evento)
		{
			if (evento == null) throw new ArgumentNullException(nameof(evento));
			if (!_repositorio.Existe(evento.Id))
				throw new Exception($"No existe evento con ID {evento.Id}");


			if (evento.FechaInicio >= evento.FechaFin)
				throw new Exception("La fecha de inicio debe ser anterior a la fecha de fin");

			return _repositorio.Actualizar(evento);
		}

		/// Cancela un evento cambiando su estado a "Cancelado".
		
		public bool CancelarEvento(int id)
		{
			Evento evento = ObtenerEvento(id);
			evento.Estado = "Cancelado";
			return _repositorio.Actualizar(evento);
		}

		/// Activa un evento cambiando su estado a "Activo" si la fecha de inicio no ha pasado.
		
		public bool ActivarEvento(int id)
		{
			Evento evento = ObtenerEvento(id);
			if (evento.FechaInicio > DateTime.Now)
			{
				evento.Estado = "Activo";
				return _repositorio.Actualizar(evento);
			}
			throw new Exception("No se puede activar un evento cuya fecha de inicio ya pasó");
		}

		/// Finaliza un evento cambiando su estado a "Finalizado".
		
		public bool FinalizarEvento(int id)
		{
			Evento evento = ObtenerEvento(id);
			evento.Estado = "Finalizado";
			return _repositorio.Actualizar(evento);
		}
		/// Elimina un evento por su ID.
		
		public bool EliminarEvento(int id)
		{
			var evento = ObtenerEvento(id);
			if (evento == null) return false;

			return _repositorio.Eliminar(id);
		}
	}
}