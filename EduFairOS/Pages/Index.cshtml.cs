using System;
using System.Collections.Generic;
using System.Linq;
using EduFairOS.Layers.Application.Contracts;
using EduFairOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduFairOS.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IServicioStand _servicioStand;
		private readonly IServicioEvento _servicioEvento;

		public List<Stand> Stands { get; set; } = new();
		public List<Evento> Eventos { get; set; } = new();

		[BindProperty]
		public Stand NewStand { get; set; } = new();

		public IndexModel(IServicioStand servicioStand, IServicioEvento servicioEvento)
		{
			_servicioStand = servicioStand;
			_servicioEvento = servicioEvento;
		}

		public void OnGet(int? editId)
		{
			Stands = _servicioStand.ObtenerTodosStands();
			Eventos = _servicioEvento.ObtenerTodosEventos();

			// Si nos pasan un ID por la URL, buscamos el stand y lo ponemos en el formulario
			if (editId.HasValue)
			{
				var standAEditar = Stands.FirstOrDefault(s => s.Id == editId.Value);
				if (standAEditar != null)
				{
					NewStand = standAEditar;
				}
			}
		}

		public IActionResult OnPost()
		{
			// Forzamos la recarga de datos
			Stands = _servicioStand.ObtenerTodosStands();
			Eventos = _servicioEvento.ObtenerTodosEventos();

			try
			{
				// LOG DE PRUEBA: Si llega aquí, el botón funciona.
				if (string.IsNullOrEmpty(NewStand.Nombre))
				{
					ModelState.AddModelError(string.Empty, "DEBUG: El nombre llegó vacío desde el formulario.");
					return Page();
				}

				bool resultado;
				if (NewStand.Id > 0)
				{
					resultado = _servicioStand.ActualizarStand(NewStand);
				}
				else
				{
					resultado = _servicioStand.CrearStand(NewStand);
				}

				if (resultado)
				{
					return RedirectToPage();
				}
				else
				{
					ModelState.AddModelError(string.Empty, "DEBUG: El Servicio devolvió 'false'. El repositorio no insertó nada.");
					return Page();
				}
			}
			catch (Exception ex)
			{
				// Esto tiene que escribir algo en el cuadro rojo sí o sí
				ModelState.AddModelError(string.Empty, "ERROR REAL: " + ex.Message + (ex.InnerException != null ? " -> " + ex.InnerException.Message : ""));
				return Page();
			}
		}

		public IActionResult OnPostDelete(int id)
		{
			try
			{
				
				_servicioStand.EliminarStand(id);
			}
			catch (Exception)
			{
				ModelState.AddModelError(string.Empty, "No se pudo eliminar el stand. Verifica si tiene actividades asociadas.");
			}

			return RedirectToPage();
		}

		public string GetEventoNombre(int eventoId)
		{
			var evento = Eventos.FirstOrDefault(e => e.Id == eventoId);
			return evento != null ? evento.Nombre : "N/A";
		}
	}
}