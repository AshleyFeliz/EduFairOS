using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using EduFairOS.Models;
using EduFairOS.Layers.Infrastructure.Interfaces;

namespace EduFairOS.Layers.Infrastructure.Data
{
	public class RepositorioParticipante : IRepositorio<Participante>
	{
		private readonly ConexionBD _conexion;

		public RepositorioParticipante(ConexionBD conexion) { _conexion = conexion; }
		public RepositorioParticipante() { _conexion = ConexionBD.ObtenerInstancia(); }

		public bool Agregar(Participante participante)
		{
			string query = @"INSERT INTO Participantes (Nombre, Edad, Grado, Institucion, Telefono, Correo, FechaRegistro, FechaCreacion, FechaActualizacion, Activo)
                             VALUES (@Nombre, @Edad, @Grado, @Institucion, @Telefono, @Correo, @FechaRegistro, @FechaCreacion, @FechaActualizacion, @Activo)";
			SqlParameter[] parameters = {
				new SqlParameter("@Nombre", participante.Nombre), new SqlParameter("@Edad", participante.Edad),
				new SqlParameter("@Grado", participante.Grado), new SqlParameter("@Institucion", participante.Institucion),
				new SqlParameter("@Telefono", participante.Telefono ?? (object)DBNull.Value), new SqlParameter("@Correo", participante.Correo ?? (object)DBNull.Value),
				new SqlParameter("@FechaRegistro", participante.FechaRegistro), new SqlParameter("@FechaCreacion", participante.FechaCreacion),
				new SqlParameter("@FechaActualizacion", participante.FechaActualizacion), new SqlParameter("@Activo", participante.Activo)
			};
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		public Participante ObtenerPorId(int id)
		{
			string query = "SELECT * FROM Participantes WHERE IdParticipante = @Id AND Activo = 1";
			SqlParameter[] parameters = { new SqlParameter("@Id", id) };
			using var reader = _conexion.EjecutarReader(query, parameters);
			if (reader.Read())
			{
				return new Participante
				{
					Id = reader.GetInt32(reader.GetOrdinal("IdParticipante")),
					Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
					Edad = reader.GetInt32(reader.GetOrdinal("Edad")),
					Grado = reader.GetString(reader.GetOrdinal("Grado")),
					Institucion = reader.GetString(reader.GetOrdinal("Institucion")),
					Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? "" : reader.GetString(reader.GetOrdinal("Telefono")),
					Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? "" : reader.GetString(reader.GetOrdinal("Correo")),
					FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
					FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
					FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
					Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
				};
			}
			return null;
		}

		public List<Participante> ObtenerTodos()
		{
			string query = "SELECT * FROM Participantes WHERE Activo = 1 ORDER BY FechaRegistro DESC";
			var participantes = new List<Participante>();
			using var reader = _conexion.EjecutarReader(query);
			while (reader.Read())
			{
				participantes.Add(new Participante
				{
					Id = reader.GetInt32(reader.GetOrdinal("IdParticipante")),
					Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
					Edad = reader.GetInt32(reader.GetOrdinal("Edad")),
					Grado = reader.GetString(reader.GetOrdinal("Grado")),
					Institucion = reader.GetString(reader.GetOrdinal("Institucion")),
					Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? "" : reader.GetString(reader.GetOrdinal("Telefono")),
					Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? "" : reader.GetString(reader.GetOrdinal("Correo")),
					FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
					FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
					FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
					Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
				});
			}
			return participantes;
		}

		public bool Actualizar(Participante participante)
		{
			string query = @"UPDATE Participantes SET Nombre = @Nombre, Edad = @Edad, Grado = @Grado, Institucion = @Institucion, Telefono = @Telefono, Correo = @Correo, FechaActualizacion = @FechaActualizacion WHERE IdParticipante = @Id";
			SqlParameter[] parameters = {
				new SqlParameter("@Id", participante.Id), new SqlParameter("@Nombre", participante.Nombre), new SqlParameter("@Edad", participante.Edad),
				new SqlParameter("@Grado", participante.Grado), new SqlParameter("@Institucion", participante.Institucion),
				new SqlParameter("@Telefono", participante.Telefono ?? (object)DBNull.Value), new SqlParameter("@Correo", participante.Correo ?? (object)DBNull.Value),
				new SqlParameter("@FechaActualizacion", DateTime.Now)
			};
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		public bool Eliminar(int id)
		{
			string query = "UPDATE Participantes SET Activo = 0, FechaActualizacion = @FechaActualizacion WHERE IdParticipante = @Id";
			SqlParameter[] parameters = { new SqlParameter("@Id", id), new SqlParameter("@FechaActualizacion", DateTime.Now) };
			return _conexion.EjecutarNonQuery(query, parameters) > 0;
		}

		// MÃ©todos faltantes de la Interfaz
		public bool Existe(int id) => ObtenerPorId(id) != null;
		public int AgregarMultiples(List<Participante> entidades) { int c = 0; foreach (var e in entidades) if (Agregar(e)) c++; return c; }
		public List<Participante> ObtenerPor(Func<Participante, bool> predicate) => ObtenerTodos().Where(predicate).ToList();
		public int ObtenerCantidad() => ObtenerTodos().Count;
		public Participante ObtenerPrimero(Func<Participante, bool> predicate) => ObtenerTodos().FirstOrDefault(predicate);
		public bool ValidarEntidad(Participante entidad) => true;
		public int EliminarMultiples(List<int> ids) { int c = 0; foreach (var id in ids) if (Eliminar(id)) c++; return c; }
	}
}