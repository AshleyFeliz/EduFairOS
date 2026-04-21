using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using EduFairOS.Models;
using EduFairOS.Layers.Infrastructure.Interfaces;

namespace EduFairOS.Layers.Infrastructure.Data
{
	public class RepositorioActividad : IRepositorio<Actividad>
	{
		private readonly ConexionBD _conexion;

		public RepositorioActividad(ConexionBD conexion) { _conexion = conexion; }
		public RepositorioActividad() { _conexion = ConexionBD.ObtenerInstancia(); }

		public bool Agregar(Actividad actividad)
		{
			string query = @"INSERT INTO Actividades (Nombre, Descripcion, HoraInicio, HoraFin, IdStand, MonitorId, CapacidadMaxima, ParticipantesActuales, Nivel, FechaCreacion, FechaActualizacion, Activo)
                             VALUES (@Nombre, @Descripcion, @HoraInicio, @HoraFin, @IdStand, @MonitorId, @CapacidadMaxima, @ParticipantesActuales, @Nivel, @FechaCreacion, @FechaActualizacion, @Activo)";
			SqlParameter[] parameters = {
				new SqlParameter("@Nombre", actividad.Nombre), new SqlParameter("@Descripcion", actividad.Descripcion ?? (object)DBNull.Value),
				new SqlParameter("@HoraInicio", actividad.HoraInicio), new SqlParameter("@HoraFin", actividad.HoraFin),
				new SqlParameter("@IdStand", actividad.IdStand), new SqlParameter("@MonitorId", actividad.MonitorId == 0 ? DBNull.Value : actividad.MonitorId),
				new SqlParameter("@CapacidadMaxima", actividad.CapacidadMaxima), new SqlParameter("@ParticipantesActuales", actividad.ParticipantesActuales),
				new SqlParameter("@Nivel", actividad.Nivel), new SqlParameter("@FechaCreacion", actividad.FechaCreacion),
				new SqlParameter("@FechaActualizacion", actividad.FechaActualizacion), new SqlParameter("@Activo", actividad.Activo)
			};
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		public Actividad ObtenerPorId(int id)
		{
			string query = "SELECT * FROM Actividades WHERE IdActividad = @Id AND Activo = 1";
			SqlParameter[] parameters = { new SqlParameter("@Id", id) };
			using var reader = _conexion.EjecutarReader(query, parameters);
			if (reader.Read())
			{
				return new Actividad
				{
					Id = reader.GetInt32(reader.GetOrdinal("IdActividad")),
					Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
					Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
					HoraInicio = reader.GetDateTime(reader.GetOrdinal("HoraInicio")),
					HoraFin = reader.GetDateTime(reader.GetOrdinal("HoraFin")),
					IdStand = reader.GetInt32(reader.GetOrdinal("IdStand")),
					MonitorId = reader.IsDBNull(reader.GetOrdinal("MonitorId")) ? 0 : reader.GetInt32(reader.GetOrdinal("MonitorId")),
					CapacidadMaxima = reader.GetInt32(reader.GetOrdinal("CapacidadMaxima")),
					ParticipantesActuales = reader.GetInt32(reader.GetOrdinal("ParticipantesActuales")),
					Nivel = reader.GetString(reader.GetOrdinal("Nivel")),
					FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
					FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
					Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
				};
			}
			return null;
		}

		public List<Actividad> ObtenerTodos()
		{
			string query = "SELECT * FROM Actividades WHERE Activo = 1 ORDER BY HoraInicio";
			var actividades = new List<Actividad>();
			using var reader = _conexion.EjecutarReader(query);
			while (reader.Read())
			{
				actividades.Add(new Actividad
				{
					Id = reader.GetInt32(reader.GetOrdinal("IdActividad")),
					Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
					Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
					HoraInicio = reader.GetDateTime(reader.GetOrdinal("HoraInicio")),
					HoraFin = reader.GetDateTime(reader.GetOrdinal("HoraFin")),
					IdStand = reader.GetInt32(reader.GetOrdinal("IdStand")),
					MonitorId = reader.IsDBNull(reader.GetOrdinal("MonitorId")) ? 0 : reader.GetInt32(reader.GetOrdinal("MonitorId")),
					CapacidadMaxima = reader.GetInt32(reader.GetOrdinal("CapacidadMaxima")),
					ParticipantesActuales = reader.GetInt32(reader.GetOrdinal("ParticipantesActuales")),
					Nivel = reader.GetString(reader.GetOrdinal("Nivel")),
					FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
					FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
					Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
				});
			}
			return actividades;
		}

		public bool Actualizar(Actividad actividad)
		{
			string query = @"UPDATE Actividades SET Nombre = @Nombre, Descripcion = @Descripcion, HoraInicio = @HoraInicio, HoraFin = @HoraFin, IdStand = @IdStand, MonitorId = @MonitorId, CapacidadMaxima = @CapacidadMaxima, ParticipantesActuales = @ParticipantesActuales, Nivel = @Nivel, FechaActualizacion = @FechaActualizacion WHERE IdActividad = @Id";
			SqlParameter[] parameters = {
				new SqlParameter("@Id", actividad.Id), new SqlParameter("@Nombre", actividad.Nombre), new SqlParameter("@Descripcion", actividad.Descripcion ?? (object)DBNull.Value),
				new SqlParameter("@HoraInicio", actividad.HoraInicio), new SqlParameter("@HoraFin", actividad.HoraFin), new SqlParameter("@IdStand", actividad.IdStand),
				new SqlParameter("@MonitorId", actividad.MonitorId == 0 ? DBNull.Value : actividad.MonitorId), new SqlParameter("@CapacidadMaxima", actividad.CapacidadMaxima),
				new SqlParameter("@ParticipantesActuales", actividad.ParticipantesActuales), new SqlParameter("@Nivel", actividad.Nivel), new SqlParameter("@FechaActualizacion", DateTime.Now)
			};
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		public bool Eliminar(int id)
		{
			string query = "UPDATE Actividades SET Activo = 0, FechaActualizacion = @FechaActualizacion WHERE IdActividad = @Id";
			SqlParameter[] parameters = { new SqlParameter("@Id", id), new SqlParameter("@FechaActualizacion", DateTime.Now) };
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		// MÃ©todos faltantes de la Interfaz
		public bool Existe(int id) => ObtenerPorId(id) != null;
		public int AgregarMultiples(List<Actividad> entidades) { int c = 0; foreach (var e in entidades) if (Agregar(e)) c++; return c; }
		public List<Actividad> ObtenerPor(Func<Actividad, bool> predicate) => ObtenerTodos().Where(predicate).ToList();
		public int ObtenerCantidad() => ObtenerTodos().Count;
		public Actividad ObtenerPrimero(Func<Actividad, bool> predicate) => ObtenerTodos().FirstOrDefault(predicate);
		public bool ValidarEntidad(Actividad entidad) => true;
		public int EliminarMultiples(List<int> ids) { int c = 0; foreach (var id in ids) if (Eliminar(id)) c++; return c; }
	}
}