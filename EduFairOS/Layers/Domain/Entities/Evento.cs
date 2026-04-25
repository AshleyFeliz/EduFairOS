//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
namespace EduFairOS.Models
{
	
	// Clase que representa un Evento de Feria Escolar
	
	public class Evento : EntidadBase
	{
		private string _nombre;
		private DateTime _fechaInicio;
		private DateTime _fechaFin;
		private string _lugar;
		private string _descripcion;
		private string _estado;

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

		public DateTime FechaInicio
		{
			get { return _fechaInicio; }
			set
			{
				_fechaInicio = value;
				ActualizarFecha();
			}
		}

		public DateTime FechaFin
		{
			get { return _fechaFin; }
			set
			{
				if (value < FechaInicio)
					throw new ArgumentException("La fecha fin no puede ser anterior a la fecha inicio");
				_fechaFin = value;
				ActualizarFecha();
			}
		}

		public string Lugar
		{
			get { return _lugar; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("El lugar no puede estar vacío");
				_lugar = value.Trim();
				ActualizarFecha();
			}
		}

		public string Descripcion
		{
			get { return _descripcion; }
			set
			{
				_descripcion = value ?? string.Empty;
				ActualizarFecha();
			}
		}

		public string Estado
		{
			get { return _estado; }
			set
			{
				_estado = value ?? "Planificación";
				ActualizarFecha();
			}
		}

		// Constructor sin parámetros
		public Evento()
		{
			_nombre = string.Empty;
			_fechaInicio = DateTime.Now;
			_fechaFin = DateTime.Now.AddDays(1);
			_lugar = string.Empty;
			_descripcion = string.Empty;
			_estado = "Planificación";
		}

		// Constructor con parámetros
		public Evento(string nombre, DateTime fechaInicio, DateTime fechaFin, string lugar)
		{
			Nombre = nombre;
			FechaInicio = fechaInicio;
			FechaFin = fechaFin;
			Lugar = lugar;
			_descripcion = string.Empty;
			_estado = "Planificación";
		}
	}
}