// Models/Participante.cs
using System;
namespace EduFairOS.Models
{
	/// <summary>
	/// Clase que representa un Participante en la Feria Escolar
	/// </summary>
	public class Participante : Persona
	{
		private string _grado;
		private string _institucion;
		private DateTime _fechaRegistro;
		private int _actividadesCompletadas;

		public string Grado
		{
			get { return _grado; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Grado no puede estar vacío");
				_grado = value.Trim();
				ActualizarFecha();
			}
		}

		public string Institucion
		{
			get { return _institucion; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Institución no puede estar vacía");
				_institucion = value.Trim();
				ActualizarFecha();
			}
		}

		public DateTime FechaRegistro
		{
			get { return _fechaRegistro; }
			set { _fechaRegistro = value; }
		}

		public int ActividadesCompletadas
		{
			get { return _actividadesCompletadas; }
			set { _actividadesCompletadas = value; }
		}

		// Constructor sin parámetros
		public Participante() : base()
		{
			_grado = string.Empty;
			_institucion = string.Empty;
			_fechaRegistro = DateTime.Now;
			_actividadesCompletadas = 0;
		}

		// Constructor con datos esenciales
		public Participante(string nombre, int edad, string grado, string institucion)
			: base(nombre, edad, string.Empty, string.Empty)
		{
			Grado = grado;
			Institucion = institucion;
			_fechaRegistro = DateTime.Now;
			_actividadesCompletadas = 0;
		}
	}
}