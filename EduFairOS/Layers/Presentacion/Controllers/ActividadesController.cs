//Ashley Esmirna Feliz Rodríguez 2025-0903
using Microsoft.AspNetCore.Mvc;
using EduFairOS.Models;
using EduFairOS.Layers.Application.Services;
using System.Collections.Generic;

namespace EduFairOS.Layers.Presentacion.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ActividadesController : ControllerBase
	{
		private readonly ServicioActividad _servicioActividad;

		public ActividadesController(ServicioActividad servicioActividad)
		{
			_servicioActividad = servicioActividad;
		}

		// GET: api/Actividades
		[HttpGet]
		public ActionResult<IEnumerable<Actividad>> GetActividades()
		{
			var actividades = _servicioActividad.ObtenerTodasActividades();
			return Ok(actividades);
		}

		// GET: api/Actividades/
		[HttpGet("{id}")]
		public ActionResult<Actividad> GetActividad(int id)
		{
			var actividad = _servicioActividad.ObtenerActividad(id);
			if (actividad == null)
			{
				return NotFound();
			}
			return Ok(actividad);
		}

		// POST: api/Actividades
		[HttpPost]
		public ActionResult<Actividad> PostActividad(Actividad actividad)
		{
			try
			{
				_servicioActividad.CrearActividad(actividad);
				return CreatedAtAction(nameof(GetActividad), new { id = actividad.Id }, actividad);
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT: api/Actividades/
		[HttpPut("{id}")]
		public IActionResult PutActividad(int id, Actividad actividad)
		{
			if (id != actividad.Id)
			{
				return BadRequest();
			}

			try
			{
				_servicioActividad.ActualizarActividad(actividad);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// DELETE: api/Actividades/
		[HttpDelete("{id}")]
		public IActionResult DeleteActividad(int id)
		{
			try
			{
				_servicioActividad.EliminarActividad(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}