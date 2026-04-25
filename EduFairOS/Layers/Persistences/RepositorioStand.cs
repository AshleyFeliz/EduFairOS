//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using EduFairOS.Models;
using EduFairOS.Layers.Infrastructure.Interfaces;
using EduFairOS.Layers.Infrastructure.Data;

namespace EduFairOS.Layers.Persistences.Repositories
{
	public class RepositorioStand : IRepositorio<Stand>
	{
		private readonly ConexionBD _conexion;

		public RepositorioStand(ConexionBD conexion) { _conexion = conexion; }
		public RepositorioStand() { _conexion = ConexionBD.ObtenerInstancia(); }

		public bool Agregar(Stand stand)
		{
			string query = @"INSERT INTO Stands (Nombre, Ubicacion, Categoria, EncargadoId, IdEvento, Descripcion, Ocupado, FechaCreacion, FechaActualizacion, Activo)
                             VALUES (@Nombre, @Ubicacion, @Categoria, @EncargadoId, @IdEvento, @Descripcion, @Ocupado, @FechaCreacion, @FechaActualizacion, @Activo)";
			SqlParameter[] parameters = {
				new SqlParameter("@Nombre", stand.Nombre), new SqlParameter("@Ubicacion", stand.Ubicacion), new SqlParameter("@Categoria", stand.Categoria),
				new SqlParameter("@EncargadoId", stand.EncargadoId == 0 ? DBNull.Value : stand.EncargadoId), new SqlParameter("@IdEvento", stand.IdEvento),
				new SqlParameter("@Descripcion", stand.Descripcion ?? (object)DBNull.Value), new SqlParameter("@Ocupado", stand.Ocupado),
				new SqlParameter("@FechaCreacion", stand.FechaCreacion), new SqlParameter("@FechaActualizacion", stand.FechaActualizacion), new SqlParameter("@Activo", stand.Activo)
			};
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		public Stand ObtenerPorId(int id)
		{
			string query = "SELECT * FROM Stands WHERE IdStand = @Id AND Activo = 1";
			SqlParameter[] parameters = { new SqlParameter("@Id", id) };
			using var reader = _conexion.EjecutarReader(query, parameters);
			if (reader.Read())
			{
				return new Stand
				{
					Id = reader.GetInt32(reader.GetOrdinal("IdStand")),
					Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
					Ubicacion = reader.GetString(reader.GetOrdinal("Ubicacion")),
					Categoria = reader.GetString(reader.GetOrdinal("Categoria")),
					EncargadoId = reader.IsDBNull(reader.GetOrdinal("EncargadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EncargadoId")),
					IdEvento = reader.GetInt32(reader.GetOrdinal("IdEvento")),
					Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
					Ocupado = reader.GetBoolean(reader.GetOrdinal("Ocupado")),
					FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
					FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
					Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
				};
			}
			return null;
		}

		public List<Stand> ObtenerTodos()
		{
			string query = "SELECT * FROM Stands WHERE Activo = 1 ORDER BY Nombre";
			var stands = new List<Stand>();
			using var reader = _conexion.EjecutarReader(query);
			while (reader.Read())
			{
				stands.Add(new Stand
				{
					Id = reader.GetInt32(reader.GetOrdinal("IdStand")),
					Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
					Ubicacion = reader.GetString(reader.GetOrdinal("Ubicacion")),
					Categoria = reader.GetString(reader.GetOrdinal("Categoria")),
					EncargadoId = reader.IsDBNull(reader.GetOrdinal("EncargadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EncargadoId")),
					IdEvento = reader.GetInt32(reader.GetOrdinal("IdEvento")),
					Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
					Ocupado = reader.GetBoolean(reader.GetOrdinal("Ocupado")),
					FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
					FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
					Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
				});
			}
			return stands;
		}

		public bool Actualizar(Stand stand)
		{
			string query = @"UPDATE Stands SET Nombre = @Nombre, Ubicacion = @Ubicacion, Categoria = @Categoria, EncargadoId = @EncargadoId, IdEvento = @IdEvento, Descripcion = @Descripcion, Ocupado = @Ocupado, FechaActualizacion = @FechaActualizacion WHERE IdStand = @Id";
			SqlParameter[] parameters = {
				new SqlParameter("@Id", stand.Id), new SqlParameter("@Nombre", stand.Nombre), new SqlParameter("@Ubicacion", stand.Ubicacion),
				new SqlParameter("@Categoria", stand.Categoria), new SqlParameter("@EncargadoId", stand.EncargadoId == 0 ? DBNull.Value : stand.EncargadoId),
				new SqlParameter("@IdEvento", stand.IdEvento), new SqlParameter("@Descripcion", stand.Descripcion ?? (object)DBNull.Value),
				new SqlParameter("@Ocupado", stand.Ocupado), new SqlParameter("@FechaActualizacion", DateTime.Now)
			};
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		public bool Eliminar(int id)
		{
			string query = "UPDATE Stands SET Activo = 0, FechaActualizacion = @FechaActualizacion WHERE IdStand = @Id";
			SqlParameter[] parameters = { new SqlParameter("@Id", id), new SqlParameter("@FechaActualizacion", DateTime.Now) };
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		
		public bool Existe(int id) => ObtenerPorId(id) != null;
		public int AgregarMultiples(List<Stand> entidades) { int c = 0; foreach (var e in entidades) if (Agregar(e)) c++; return c; }
		public List<Stand> ObtenerPor(Func<Stand, bool> predicate) => ObtenerTodos().Where(predicate).ToList();
		public int ObtenerCantidad() => ObtenerTodos().Count;
		public Stand ObtenerPrimero(Func<Stand, bool> predicate) => ObtenerTodos().FirstOrDefault(predicate);
		public bool ValidarEntidad(Stand entidad) => true;
		public int EliminarMultiples(List<int> ids) { int c = 0; foreach (var id in ids) if (Eliminar(id)) c++; return c; }
	}
}