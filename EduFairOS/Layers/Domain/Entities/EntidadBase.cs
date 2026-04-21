// Models/EntidadBase.cs
using System;
namespace EduFairOS.Models
{
	/// <summary>
	/// Clase base para todas las entidades con campos comunes
	/// </summary>
	public abstract class EntidadBase
	{
		public int Id { get; set; }
		public DateTime FechaCreacion { get; set; } = DateTime.Now;
		public DateTime FechaActualizacion { get; set; } = DateTime.Now;
		public bool Activo { get; set; } = true;

		/// <summary>
		/// Método para actualizar la fecha de modificación
		/// </summary>
		protected void ActualizarFecha()
		{
			FechaActualizacion = DateTime.Now;
		}
	}
}