//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;

namespace EduFairOS.Layers.Infrastructure.Interfaces
{
	
	// Interfaz genérica para operaciones CRUD
	// Define el contrato para las clases de acceso a datos
	
	public interface IRepositorio<T> where T : class
	{
		bool Agregar(T entidad);
		T ObtenerPorId(int id);
		List<T> ObtenerTodos();
		bool Actualizar(T entidad);
		bool Eliminar(int id);
		bool Existe(int id);
		int AgregarMultiples(List<T> entidades);
		List<T> ObtenerPor(Func<T, bool> predicate);
		int ObtenerCantidad();
		T ObtenerPrimero(Func<T, bool> predicate);
		bool ValidarEntidad(T entidad);
		int EliminarMultiples(List<int> ids);
	}
}