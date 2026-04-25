//Ashley Esmirna Feliz Rodríguez 2025-0903
using Microsoft.AspNetCore.Mvc;
using EduFairOS.Models;
using EduFairOS.Layers.Application.Services;
using System.Collections.Generic;

namespace EduFairOS.Layers.Presentacion.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StandsController : ControllerBase
	{
		private readonly ServicioStand _servicioStand;

		public StandsController(ServicioStand servicioStand)
		{
			_servicioStand = servicioStand;
		}

		// GET: api/Stands
		[HttpGet]
		public ActionResult<IEnumerable<Stand>> GetStands()
		{
			var stands = _servicioStand.ObtenerTodosStands();
			return Ok(stands);
		}

		// GET: api/Stands/
		[HttpGet("{id}")]
		public ActionResult<Stand> GetStand(int id)
		{
			var stand = _servicioStand.ObtenerStand(id);
			if (stand == null)
			{
				return NotFound();
			}
			return Ok(stand);
		}

		// POST: api/Stands
		[HttpPost]
		public ActionResult<Stand> PostStand(Stand stand)
		{
			try
			{
				_servicioStand.CrearStand(stand);
				return CreatedAtAction(nameof(GetStand), new { id = stand.Id }, stand);
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT: api/Stands/
		[HttpPut("{id}")]
		public IActionResult PutStand(int id, Stand stand)
		{
			if (id != stand.Id)
			{
				return BadRequest();
			}

			try
			{
				_servicioStand.ActualizarStand(stand);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// DELETE: api/Stands/
		[HttpDelete("{id}")]
		public IActionResult DeleteStand(int id)
		{
			try
			{
				_servicioStand.EliminarStand(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}