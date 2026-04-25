//Ashley Esmirna Feliz Rodríguez 2025-0903
using Microsoft.AspNetCore.Mvc;
using EduFairOS.Models;
using EduFairOS.Layers.Application.Services;
using System.Collections.Generic;

namespace EduFairOS.Layers.Presentacion.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ParticipantesController : ControllerBase
	{
		private readonly ServicioParticipante _servicioParticipante;

		public ParticipantesController(ServicioParticipante servicioParticipante)
		{
			_servicioParticipante = servicioParticipante;
		}

		// GET: api/Participantes
		[HttpGet]
		public ActionResult<IEnumerable<Participante>> GetParticipantes()
		{
			var participantes = _servicioParticipante.ObtenerTodosParticipantes();
			return Ok(participantes);
		}

		// GET: api/Participantes/
		[HttpGet("{id}")]
		public ActionResult<Participante> GetParticipante(int id)
		{
			try
			{
				var participante = _servicioParticipante.ObtenerParticipante(id);
				return Ok(participante);
			}
			catch (System.Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		// POST: api/Participantes
		[HttpPost]
		public ActionResult<Participante> PostParticipante(Participante participante)
		{
			try
			{
				_servicioParticipante.RegistrarParticipante(participante);
				return CreatedAtAction(nameof(GetParticipante), new { id = participante.Id }, participante);
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT: api/Participantes/
		[HttpPut("{id}")]
		public IActionResult PutParticipante(int id, Participante participante)
		{
			if (id != participante.Id)
			{
				return BadRequest();
			}

			try
			{
				_servicioParticipante.ActualizarParticipante(participante);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// DELETE: api/Participantes/
		[HttpDelete("{id}")]
		public IActionResult DeleteParticipante(int id)
		{
			try
			{
				_servicioParticipante.EliminarParticipante(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}