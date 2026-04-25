using System;
using System.Collections.Generic;
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

		public void OnGet()
		{
			Eventos = _servicioEvento.ObtenerTodosEventos();
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
				_servicioEvento.CrearEvento(NewEvento);
				return RedirectToPage();
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return Page();
			}
		}
	}
}