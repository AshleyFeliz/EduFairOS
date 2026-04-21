// Models/Persona.cs
using System;
namespace EduFairOS.Models
{
	/// <summary>
	/// Clase abstracta que representa una persona
	/// </summary>
	public abstract class Persona : EntidadBase
	{
		private string _nombre;
		private int _edad;
		private string _telefono;
		private string _correo;

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

		public int Edad
		{
			get { return _edad; }
			set
			{
				if (value < 5 || value > 100)
					throw new ArgumentException("Edad debe estar entre 5 y 100 años");
				_edad = value;
				ActualizarFecha();
			}
		}

		public string Telefono
		{
			get { return _telefono; }
			set
			{
				_telefono = value?.Trim() ?? string.Empty;
				ActualizarFecha();
			}
		}
		public string Correo
		{
			get { return _correo; }
			set
			{
				_correo = value?.Trim() ?? string.Empty;
				ActualizarFecha();
			}
		}

		// Constructor sin parámetros
		public Persona()
		{
			_nombre = string.Empty;
			_edad = 0;
			_telefono = string.Empty;
			_correo = string.Empty;
		}

		// Constructor con parámetros
		public Persona(string nombre, int edad, string telefono, string correo)
		{
			Nombre = nombre;
			Edad = edad;
			Telefono = telefono;
			Correo = correo;
		}
	}
}