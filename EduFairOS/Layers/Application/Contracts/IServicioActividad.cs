using System.Collections.Generic;
using EduFairOS.Models;

namespace EduFairOS.Layers.Application.Contracts
{
	public interface IServicioActividad
	{
		bool CrearActividad(Actividad actividad);
		Actividad ObtenerActividad(int id);
		List<Actividad> ObtenerTodasActividades();
		bool ActualizarActividad(Actividad actividad);
		bool EliminarActividad(int id);
	}
}
