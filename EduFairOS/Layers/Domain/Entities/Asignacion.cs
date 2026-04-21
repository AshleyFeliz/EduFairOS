// Models/Asignacion.cs
using System;
namespace EduFairOS.Models
{
	/// <summary>
	/// Clase que representa la asignación de un participante a una actividad
	/// </summary>
	public class Asignacion : EntidadBase
	{
		private int _idParticipante;
		private int _idActividad;
		private DateTime _fechaAsignacion;
		private string _estado;
		private decimal _puntuacion;

		public int IdParticipante
		{
			get { return _idParticipante; }
			set { _idParticipante = value; }
		}

		public int IdActividad
		{
			get { return _idActividad; }
			set { _idActividad = value; }
		}

		public DateTime FechaAsignacion
		{
			get { return _fechaAsignacion; }
			set { _fechaAsignacion = value; }
		}

		public string Estado
		{
			get { return _estado; }
			set { _estado = value ?? "Pendiente"; }
		}

		public decimal Puntuacion
		{
			get { return _puntuacion; }
			set { _puntuacion = value; }
		}

		// Constructor sin parámetros
		public Asignacion()
		{
			_idParticipante = 0;
			_idActividad = 0;
			_fechaAsignacion = DateTime.Now;
			_estado = "Pendiente";
			_puntuacion = 0;
		}

		// Constructor con parámetros
		public Asignacion(int idParticipante, int idActividad)
		{
			IdParticipante = idParticipante;
			IdActividad = idActividad;
			_fechaAsignacion = DateTime.Now;
			_estado = "Pendiente";
			_puntuacion = 0;
		}
	}
}