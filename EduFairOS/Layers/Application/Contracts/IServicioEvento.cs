//Ashley Esmirna Feliz Rodríguez 2025-0903
using System.Collections.Generic;
using EduFairOS.Models;

namespace EduFairOS.Layers.Application.Contracts
{
	public interface IServicioEvento
	{
		bool CrearEvento(Evento evento);
		Evento ObtenerEvento(int id);
		List<Evento> ObtenerTodosEventos();
		bool ActualizarEvento(Evento evento);
		bool EliminarEvento(int id);
		bool CancelarEvento(int id);
		bool ActivarEvento(int id);
		bool FinalizarEvento(int id);
	}
}
