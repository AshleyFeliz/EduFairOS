
using System;
using System.Collections.Generic;
using EduFairOS.Models; // Usamos el namespace que mantuvimos en las entidades
using EduFairOS.Layers.Infrastructure.Data;

namespace EduFairOS.Layers.Application.Services
{
	/// <summary>
	/// Clase de servicio para la lógica de negocio de Eventos
	/// Actúa como intermediaria entre la capa de presentación y la de datos
	/// </summary>
	public class ServicioEvento
	{
		private RepositorioEvento _repositorio;

		public ServicioEvento()
		{
			_repositorio = new RepositorioEvento();
		}

		public bool CrearEvento(Evento evento)
		{
			if (evento == null) throw new ArgumentNullException(nameof(evento));
			if (string.IsNullOrEmpty(evento.Nombre) || evento.Nombre.Length < 3)
				throw new Exception("El nombre del evento debe tener al menos 3 caracteres");

			// Reemplazo de evento.ValidarFechas() que faltaba en el modelo del maestro
			if (evento.FechaInicio >= evento.FechaFin)
				throw new Exception("La fecha de inicio debe ser anterior a la fecha de fin");

			if (evento.FechaInicio < DateTime.Now.Date)
				throw new Exception("La fecha de inicio no puede ser en el pasado");

			if (string.IsNullOrEmpty(evento.Lugar))
				throw new Exception("El lugar del evento es obligatorio");

			return _repositorio.Agregar(evento);
		}

		public Evento ObtenerEvento(int id)
		{
			if (id <= 0) throw new ArgumentException("El ID debe ser mayor a 0");
			Evento evento = _repositorio.ObtenerPorId(id);
			if (evento == null) throw new Exception($"No se encontró evento con ID {id}");
			return evento;
		}

		public List<Evento> ObtenerTodosEventos()
		{
			return _repositorio.ObtenerTodos();
		}

		public bool ActualizarEvento(Evento evento)
		{
			if (evento == null) throw new ArgumentNullException(nameof(evento));
			if (!_repositorio.Existe(evento.Id))
				throw new Exception($"No existe evento con ID {evento.Id}");

			// Reemplazo de evento.ValidarFechas()
			if (evento.FechaInicio >= evento.FechaFin)
				throw new Exception("La fecha de inicio debe ser anterior a la fecha de fin");

			return _repositorio.Actualizar(evento);
		}

		public bool CancelarEvento(int id)
		{
			Evento evento = ObtenerEvento(id);
			evento.Estado = "Cancelado";
			return _repositorio.Actualizar(evento);
		}

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

		public bool FinalizarEvento(int id)
		{
			Evento evento = ObtenerEvento(id);
			evento.Estado = "Finalizado";
			return _repositorio.Actualizar(evento);
		}
		/// <summary>
		/// Elimina un evento
		/// </summary>
		public bool EliminarEvento(int id)
		{
			var evento = ObtenerEvento(id);
			if (evento == null) return false;

			return _repositorio.Eliminar(id);
		}
	}
}
