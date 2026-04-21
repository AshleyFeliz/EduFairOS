using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using EduFairOS.Models;
using EduFairOS.Layers.Infrastructure.Interfaces;

namespace EduFairOS.Layers.Infrastructure.Data
{
	public class RepositorioEvento : IRepositorio<Evento>
	{
		private readonly ConexionBD _conexion;

		public RepositorioEvento(ConexionBD conexion) { _conexion = conexion; }
		// Constructor sin parámetros para compatibilidad
		public RepositorioEvento() { _conexion = ConexionBD.ObtenerInstancia(); }

		public bool Agregar(Evento evento)
		{
			string query = @"INSERT INTO Eventos (Nombre, FechaInicio, FechaFin, Lugar, Descripcion, Estado, FechaCreacion, FechaActualizacion, Activo)
                             VALUES (@Nombre, @FechaInicio, @FechaFin, @Lugar, @Descripcion, @Estado, @FechaCreacion, @FechaActualizacion, @Activo)";
			SqlParameter[] parameters = {
				new SqlParameter("@Nombre", evento.Nombre), new SqlParameter("@FechaInicio", evento.FechaInicio),
				new SqlParameter("@FechaFin", evento.FechaFin), new SqlParameter("@Lugar", evento.Lugar),
				new SqlParameter("@Descripcion", evento.Descripcion), new SqlParameter("@Estado", evento.Estado),
				new SqlParameter("@FechaCreacion", evento.FechaCreacion), new SqlParameter("@FechaActualizacion", evento.FechaActualizacion),
				new SqlParameter("@Activo", evento.Activo)
			};
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		public Evento ObtenerPorId(int id)
		{
			string query = "SELECT * FROM Eventos WHERE IdEvento = @Id AND Activo = 1";
			SqlParameter[] parameters = { new SqlParameter("@Id", id) };
			using var reader = _conexion.EjecutarReader(query, parameters);
			if (reader.Read())
			{
				return new Evento
				{
					Id = reader.GetInt32(reader.GetOrdinal("IdEvento")),
					Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
					FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
					FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin")),
					Lugar = reader.GetString(reader.GetOrdinal("Lugar")),
					Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
					Estado = reader.GetString(reader.GetOrdinal("Estado")),
					FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
					FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
					Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
				};
			}
			return null;
		}

		public List<Evento> ObtenerTodos()
		{
			string query = "SELECT * FROM Eventos WHERE Activo = 1 ORDER BY FechaInicio DESC";
			var eventos = new List<Evento>();
			using var reader = _conexion.EjecutarReader(query);
			while (reader.Read())
			{
				eventos.Add(new Evento
				{
					Id = reader.GetInt32(reader.GetOrdinal("IdEvento")),
					Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
					FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
					FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin")),
					Lugar = reader.GetString(reader.GetOrdinal("Lugar")),
					Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
					Estado = reader.GetString(reader.GetOrdinal("Estado")),
					FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
					FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
					Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
				});
			}
			return eventos;
		}

		public bool Actualizar(Evento evento)
		{
			string query = @"UPDATE Eventos SET Nombre = @Nombre, FechaInicio = @FechaInicio, FechaFin = @FechaFin, Lugar = @Lugar, Descripcion = @Descripcion, Estado = @Estado, FechaActualizacion = @FechaActualizacion WHERE IdEvento = @Id";
			SqlParameter[] parameters = {
				new SqlParameter("@Id", evento.Id), new SqlParameter("@Nombre", evento.Nombre), new SqlParameter("@FechaInicio", evento.FechaInicio),
				new SqlParameter("@FechaFin", evento.FechaFin), new SqlParameter("@Lugar", evento.Lugar), new SqlParameter("@Descripcion", evento.Descripcion),
				new SqlParameter("@Estado", evento.Estado), new SqlParameter("@FechaActualizacion", DateTime.Now)
			};
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		public bool Eliminar(int id)
		{
			string query = "UPDATE Eventos SET Activo = 0, FechaActualizacion = @FechaActualizacion WHERE IdEvento = @Id";
			SqlParameter[] parameters = { new SqlParameter("@Id", id), new SqlParameter("@FechaActualizacion", DateTime.Now) };
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		// Métodos faltantes de la Interfaz del profesor
		public bool Existe(int id) => ObtenerPorId(id) != null;
		public int AgregarMultiples(List<Evento> entidades) { int c = 0; foreach (var e in entidades) if (Agregar(e)) c++; return c; }
		public List<Evento> ObtenerPor(Func<Evento, bool> predicate) => ObtenerTodos().Where(predicate).ToList();
		public int ObtenerCantidad() => ObtenerTodos().Count;
		public Evento ObtenerPrimero(Func<Evento, bool> predicate) => ObtenerTodos().FirstOrDefault(predicate);
		public bool ValidarEntidad(Evento entidad) => true;
		public int EliminarMultiples(List<int> ids) { int c = 0; foreach (var id in ids) if (Eliminar(id)) c++; return c; }
	}
}
