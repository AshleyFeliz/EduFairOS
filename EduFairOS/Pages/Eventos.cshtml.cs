using System;
using System.Collections.Generic;
using System.Linq;
using EduFairOS.Layers.Application.Contracts;
using EduFairOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduFairOS.Pages
{
	public class EventosModel : PageModel
	{
		private readonly IServicioEvento _servicioEvento;

		public List<Evento> Eventos { get; set; } = new();

		[BindProperty]
		public Evento NewEvento { get; set; } = new();

		public EventosModel(IServicioEvento servicioEvento)
		{
			_servicioEvento = servicioEvento;
		}

		public void OnGet(int? editId)
		{
			Eventos = _servicioEvento.ObtenerTodosEventos();

			// Si nos pasan un ID por la URL, cargamos el evento en el formulario
			if (editId.HasValue)
			{
				var eventoAEditar = Eventos.FirstOrDefault(e => e.Id == editId.Value);
				if (eventoAEditar != null)
				{
					NewEvento = eventoAEditar;
				}
			}
		}

		public IActionResult OnPost()
		{
			Eventos = _servicioEvento.ObtenerTodosEventos();

			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				if (NewEvento.Id > 0)
				{
					// Editar evento existente
					_servicioEvento.ActualizarEvento(NewEvento);
				}
				else
				{
					// Crear nuevo evento
					_servicioEvento.CrearEvento(NewEvento);
				}

				return RedirectToPage();
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return Page();
			}
		}

		public IActionResult OnPostDelete(int id)
		{
			try
			{
				_servicioEvento.EliminarEvento(id);
			}
			catch (Exception)
			{
				ModelState.AddModelError(string.Empty, "No se pudo eliminar el evento. Verifica si tiene stands asociados.");
			}

			return RedirectToPage();
		}
	}
}
