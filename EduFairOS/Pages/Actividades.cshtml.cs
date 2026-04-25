using System;
using System.Collections.Generic;
using System.Linq;
using EduFairOS.Layers.Application.Contracts;
using EduFairOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduFairOS.Pages
{
	public class ActividadesModel : PageModel
	{
		private readonly IServicioActividad _servicioActividad;
		private readonly IServicioStand _servicioStand;

		public List<Actividad> Actividades { get; set; } = new();
		public List<Stand> Stands { get; set; } = new();

		[BindProperty]
		public Actividad NewActividad { get; set; } = new();

		public ActividadesModel(IServicioActividad servicioActividad, IServicioStand servicioStand)
		{
			_servicioActividad = servicioActividad;
			_servicioStand = servicioStand;
		}

		public void OnGet(int? editId)
		{
			Actividades = _servicioActividad.ObtenerTodasActividades();
			Stands = _servicioStand.ObtenerTodosStands();

			if (editId.HasValue)
			{
				var actividadAEditar = Actividades.FirstOrDefault(a => a.Id == editId.Value);
				if (actividadAEditar != null)
				{
					NewActividad = actividadAEditar;
				}
			}
		}

		public IActionResult OnPost()
		{
			
			Actividades = _servicioActividad.ObtenerTodasActividades();
			Stands = _servicioStand.ObtenerTodosStands();

			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (NewActividad.IdStand <= 0)
			{
				ModelState.AddModelError("NewActividad.IdStand", "Seleccione un stand.");
				return Page();
			}

			try
			{
				if (NewActividad.Id > 0)
				{
					
					_servicioActividad.ActualizarActividad(NewActividad);
				}
				else
				{
					_servicioActividad.CrearActividad(NewActividad);
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
				
				_servicioActividad.EliminarActividad(id);
			}
			catch (Exception)
			{
				ModelState.AddModelError(string.Empty, "Hubo un error al intentar eliminar la actividad.");
			}

			return RedirectToPage();
		}

		public string GetStandNombre(int standId)
		{
			var stand = Stands.FirstOrDefault(s => s.Id == standId);
			return stand != null ? stand.Nombre : "N/A";
		}
	}
}