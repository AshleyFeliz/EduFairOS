using System;
using System.Collections.Generic;
using System.Linq;
using EduFairOS.Layers.Application.Contracts;
using EduFairOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduFairOS.Pages
{
	public class ParticipantesModel : PageModel
	{
		private readonly IServicioParticipante _servicioParticipante;

		public List<Participante> Participantes { get; set; } = new();

		[BindProperty]
		public Participante NewParticipante { get; set; } = new();

		public ParticipantesModel(IServicioParticipante servicioParticipante)
		{
			_servicioParticipante = servicioParticipante;
		}

		public void OnGet(int? editId)
		{
			Participantes = _servicioParticipante.ObtenerTodosParticipantes();

			if (editId.HasValue)
			{
				var participanteAEditar = Participantes.FirstOrDefault(p => p.Id == editId.Value);
				if (participanteAEditar != null)
				{
					NewParticipante = participanteAEditar;
				}
			}
		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
			{
				Participantes = _servicioParticipante.ObtenerTodosParticipantes();
				return Page();
			}

			try
			{
				if (NewParticipante.Id > 0)
				{
					
					_servicioParticipante.ActualizarParticipante(NewParticipante);
				}
				else
				{
					_servicioParticipante.RegistrarParticipante(NewParticipante);
				}

				return RedirectToPage();
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				Participantes = _servicioParticipante.ObtenerTodosParticipantes();
				return Page();
			}
		}

		public IActionResult OnPostDelete(int id)
		{
			try
			{
				
				_servicioParticipante.EliminarParticipante(id);
			}
			catch (Exception)
			{
				ModelState.AddModelError(string.Empty, "Hubo un error al intentar eliminar el participante.");
			}

			return RedirectToPage();
		}
	}
}