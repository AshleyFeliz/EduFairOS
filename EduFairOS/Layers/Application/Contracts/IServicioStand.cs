using System.Collections.Generic;
using EduFairOS.Models;

namespace EduFairOS.Layers.Application.Contracts
{
	public interface IServicioStand
	{
		bool CrearStand(Stand stand);
		Stand ObtenerStand(int id);
		List<Stand> ObtenerTodosStands();
		bool ActualizarStand(Stand stand);
		bool EliminarStand(int id);
	}
}