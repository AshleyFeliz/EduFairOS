
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EduFairOS.Layers.Infrastructure.Data
{
	public class ConexionBD
	{
		private static ConexionBD _instancia;
		private string _cadenaConexion;
		private SqlConnection _conexion;

		public string CadenaConexion
		{
			get { return _cadenaConexion; }
			set { _cadenaConexion = value; }
		}

		private ConexionBD()
		{
			// Usamos tu conexión confiable
			_cadenaConexion = @"Server=ASHLEYFELIZPC\MSSQLSERVER01;Database=EduFairOS;Trusted_Connection=True;TrustServerCertificate=True;";
			_conexion = new SqlConnection(_cadenaConexion);
		}

		public static ConexionBD ObtenerInstancia()
		{
			if (_instancia == null)
			{
				_instancia = new ConexionBD();
			}
			return _instancia;
		}

		public bool Conectar()
		{
			try
			{
				// Verificamos de forma más segura el estado de la conexión
				if (_conexion.State != ConnectionState.Open)
				{
					if (_conexion.State == ConnectionState.Broken)
						_conexion.Close(); // La reiniciamos si está "rota"

					_conexion.Open();
					Console.WriteLine("Conexión establecida exitosamente.");
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al conectar: {ex.Message}");
				return false;
			}
		}

		public bool Desconectar()
		{
			try
			{
				if (_conexion.State == ConnectionState.Open)
				{
					_conexion.Close();
					Console.WriteLine("Conexión cerrada.");
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al desconectar: {ex.Message}");
				return false;
			}
		}

		public SqlConnection ObtenerConexion()
		{
			return _conexion;
		}

		public bool EstaConectado()
		{
			return _conexion.State == ConnectionState.Open;
		}

		public int EjecutarComando(string sql)
		{
			try
			{
				Conectar(); // Aseguramos que conecte siempre
				using SqlCommand command = new SqlCommand(sql, _conexion);
				return command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al ejecutar comando: {ex.Message}");
				return 0;
			}
		}

		public SqlDataReader EjecutarConsulta(string sql)
		{
			try
			{
				Conectar();
				SqlCommand command = new SqlCommand(sql, _conexion);
				return command.ExecuteReader();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al ejecutar consulta: {ex.Message}");
				throw;
			}
		}

		// Métodos con parámetros ajustados para la arquitectura del maestro
		public int EjecutarNonQuery(string query, SqlParameter[] parameters)
		{
			Conectar(); // Forzamos a que siempre se abra antes de ejecutar
			using SqlCommand command = new SqlCommand(query, _conexion);
			command.Parameters.AddRange(parameters);
			return command.ExecuteNonQuery();
		}

		public SqlDataReader EjecutarReader(string query, SqlParameter[] parameters = null)
		{
			Conectar();
			SqlCommand command = new SqlCommand(query, _conexion);
			if (parameters != null) command.Parameters.AddRange(parameters);

			// Le quitamos el "CloseConnection" para respetar el Singleton del maestro
			return command.ExecuteReader();
		}
	}
}