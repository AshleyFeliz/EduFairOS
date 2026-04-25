//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
namespace EduFairOS.Models
{
	
	// Clase base para todas las entidades con campos comunes
	
	public abstract class EntidadBase
	{
		public int Id { get; set; }
		public DateTime FechaCreacion { get; set; } = DateTime.Now;
		public DateTime FechaActualizacion { get; set; } = DateTime.Now;
		public bool Activo { get; set; } = true;

		
		// Método para actualizar la fecha de modificación
		
		protected void ActualizarFecha()
		{
			FechaActualizacion = DateTime.Now;
		}
	}
}