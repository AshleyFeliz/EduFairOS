// ProyectoFE/Layers/Presentación/Controllers/EventosController.cs
using Microsoft.AspNetCore.Mvc;
using EduFairOS.Models; // Usamos el namespace de nuestras entidades
using EduFairOS.Layers.Application.Services;

namespace EduFairOS.Layers.Presentacion.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EventosController : ControllerBase
	{
		private readonly ServicioEvento _servicioEvento;

		public EventosController(ServicioEvento servicioEvento)
		{
			_servicioEvento = servicioEvento;
		}

		// GET: api/Eventos
		[HttpGet]
		public ActionResult<IEnumerable<Evento>> GetEventos()
		{
			var eventos = _servicioEvento.ObtenerTodosEventos();
			return Ok(eventos);
		}

		// GET: api/Eventos/5
		[HttpGet("{id}")]
		public ActionResult<Evento> GetEvento(int id)
		{
			try
			{
				var evento = _servicioEvento.ObtenerEvento(id);
				return Ok(evento);
			}
			catch (System.Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		// POST: api/Eventos
		[HttpPost]
		public ActionResult<Evento> PostEvento(Evento evento)
		{
			try
			{
				_servicioEvento.CrearEvento(evento);
				return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT: api/Eventos/5
		[HttpPut("{id}")]
		public IActionResult PutEvento(int id, Evento evento)
		{
			if (id != evento.Id)
			{
				return BadRequest();
			}

			try
			{
				_servicioEvento.ActualizarEvento(evento);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// DELETE: api/Eventos/5
		[HttpDelete("{id}")]
		public IActionResult DeleteEvento(int id)
		{
			try
			{
				_servicioEvento.EliminarEvento(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}