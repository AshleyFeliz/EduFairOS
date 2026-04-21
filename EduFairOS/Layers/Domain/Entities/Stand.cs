// Models/Stand.cs
using System.Collections.Generic;
using System;
namespace EduFairOS.Models
{
	/// <summary>
	/// Clase que representa un Stand dentro de un Evento
	/// </summary>
	public class Stand : EntidadBase
	{
		private string _nombre;
		private string _ubicacion;
		private string _categoria;
		private int _encargadoId;
		private int _idEvento;
		private string _descripcion;
		private List<int> _actividadesIds;
		private bool _ocupado;

		public string Nombre
		{
			get { return _nombre; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("El nombre no puede estar vacío");
				_nombre = value.Trim();
				ActualizarFecha();
			}
		}

		public string Ubicacion
		{
			get { return _ubicacion; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Ubicación no puede estar vacía");
				_ubicacion = value.Trim();
				ActualizarFecha();
			}
		}

		public string Categoria
		{
			get { return _categoria; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Categoría no puede estar vacía");
				_categoria = value.Trim();
				ActualizarFecha();
			}
		}

		public int EncargadoId
		{
			get { return _encargadoId; }
			set { _encargadoId = value; ActualizarFecha(); }
		}

		public int IdEvento
		{
			get { return _idEvento; }
			set { _idEvento = value; }
		}

		public string Descripcion
		{
			get { return _descripcion; }
			set { _descripcion = value ?? string.Empty; ActualizarFecha(); }
		}

		public bool Ocupado
		{
			get { return _ocupado; }
			set { _ocupado = value; }
		}

		// Constructor sin parámetros
		public Stand() : base()
		{
			_nombre = string.Empty;
			_ubicacion = string.Empty;
			_categoria = string.Empty;
			_encargadoId = 0;
			_descripcion = string.Empty;
			_idEvento = 0;
			_actividadesIds = new List<int>();
			_ocupado = false;
		}

		// Constructor con datos esenciales
		public Stand(string nombre, string ubicacion, string categoria, int idEvento)
		{
			Nombre = nombre;
			Ubicacion = ubicacion;
			Categoria = categoria;
			IdEvento = idEvento;
			_encargadoId = 0;
			_descripcion = string.Empty;
			_actividadesIds = new List<int>();
			_ocupado = false;
		}
	}
}