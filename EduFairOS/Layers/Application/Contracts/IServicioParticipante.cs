//Ashley Esmirna Feliz Rodríguez 2025-0903
using System.Collections.Generic;
using EduFairOS.Models;

namespace EduFairOS.Layers.Application.Contracts
{
	public interface IServicioParticipante
	{
		bool RegistrarParticipante(Participante participante);
		Participante ObtenerParticipante(int id);
		List<Participante> ObtenerTodosParticipantes();
		List<Participante> ObtenerPorInstitucion(string institucion);
		List<Participante> ObtenerPorCategoriaEdad(string categoria);
		bool ActualizarParticipante(Participante participante);
		bool EliminarParticipante(int id);
		List<Participante> BuscarPorNombre(string nombre);
		string GenerarEstadisticas();
	}
}
