//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;
namespace EduFairOS.Models
{
	
	// Clase que representa una Actividad dentro de un Stand
	
	public class Actividad : EntidadBase
	{
		private string _nombre;
		private string _descripcion;
		private DateTime _horaInicio;
		private DateTime _horaFin;
		private int _idStand;
		private int _monitorId;
		private int _capacidadMaxima;
		private int _participantesActuales;
		private List<int> _participantesIds;
		private string _nivel;

		public string Nombre
		{
			get { return _nombre; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("El nombre no puede estar vacío");
				_nombre = value.Trim();
				ActualizarFecha();
			}
		}

		public string Descripcion
		{
			get { return _descripcion; }
			set { _descripcion = value ?? string.Empty; ActualizarFecha(); }
		}

		public DateTime HoraInicio
		{
			get { return _horaInicio; }
			set { _horaInicio = value; ActualizarFecha(); }
		}

		public DateTime HoraFin
		{
			get { return _horaFin; }
			set { _horaFin = value; ActualizarFecha(); }
		}

		public int IdStand
		{
			get { return _idStand; }
			set { _idStand = value; }
		}

		public int MonitorId
		{
			get { return _monitorId; }
			set { _monitorId = value; ActualizarFecha(); }
		}

		public int CapacidadMaxima
		{
			get { return _capacidadMaxima; }
			set
			{
				if (value <= 0 || value > 1000)
					throw new ArgumentException("Capacidad debe estar entre 1 y 1000");
				_capacidadMaxima = value;
				ActualizarFecha();
			}
		}

		public int ParticipantesActuales
		{
			get { return _participantesActuales; }
			set { _participantesActuales = value; }
		}

		public string Nivel
		{
			get { return _nivel; }
			set { _nivel = value ?? "Básico"; ActualizarFecha(); }
		}

		// Constructor sin parámetros
		public Actividad() : base()
		{
			_nombre = string.Empty;
			_descripcion = string.Empty;
			_horaInicio = DateTime.Now;
			_horaFin = DateTime.Now.AddHours(1);
			_idStand = 0;
			_monitorId = 0;
			_capacidadMaxima = 30;
			_participantesActuales = 0;
			_participantesIds = new List<int>();
			_nivel = "Básico";
		}

		// Constructor con datos esenciales
		public Actividad(string nombre, DateTime horaInicio, DateTime horaFin) : this()
		{
			Nombre = nombre;
			HoraInicio = horaInicio;
			HoraFin = horaFin;
		}
	}
}