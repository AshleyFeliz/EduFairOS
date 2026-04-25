# EduFairOS - Sistema de Gestión de Ferias Escolares

## Descripción
EduFairOS es una aplicación web desarrollada en ASP.NET Core para la gestión de ferias escolares. Permite administrar eventos, stands, actividades, participantes y asignaciones de manera eficiente.

## Arquitectura
El proyecto sigue una arquitectura en capas limpia (Clean Architecture):
- **Presentación**: Controladores API REST
- **Aplicación**: Servicios de negocio y contratos
- **Dominio**: Entidades, lógica de dominio y especificaciones
- **Infraestructura**: Acceso a datos y repositorios

## Documentación de entrega
- Documento de requerimientos y diseño: `docs/ENTREGA_EDUFAIROS.md`
- Diagrama de clases: `docs/DIAGRAMA_CLASES.md`

## Interfaz de usuario
La aplicación incluye un frontend básico con Razor Pages que permite:
- ver y crear stands en el dashboard principal (`/`)
- administrar eventos en `/Eventos`
- registrar y listar participantes en `/Participantes`
- crear y listar actividades en `/Actividades`

El frontend utiliza Bootstrap para un diseño responsivo y navegación consistente a través de `_Layout.cshtml`.

## Cómo ejecutar la aplicación
1. Asegúrate de tener SQL Server instalado y ejecuta el script `BD/CrearBD_EduFairOS.sql` para crear la base de datos.
2. Configura la cadena de conexión en `appsettings.json`.
3. Ejecuta `dotnet run` desde la raíz del proyecto.
4. Accede al frontend en `https://localhost:5001` (o el puerto configurado).
5. La documentación API Swagger está disponible en `https://localhost:5001/swagger`.

## Tecnologías Utilizadas
- ASP.NET Core 8.0
- Entity Framework Core (implícitamente a través de repositorios personalizados)
- SQL Server
- Swagger para documentación API
- Patrón Repository
- Inyección de dependencias

## Estructura del Proyecto

### Capa de Dominio (Domain)
#### Entidades
##### EntidadBase.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903

/// <summary>
/// Espacio de nombres para los modelos de la aplicación EduFairOS.
/// </summary>
namespace EduFairOS.Models
{

	/// <summary>
	/// Clase base abstracta para todas las entidades del dominio.
	/// Proporciona campos comunes como Id, fechas de creación y actualización, y estado activo.
	/// </summary>
	public abstract class EntidadBase
	{
		/// <summary>
		/// Identificador único de la entidad.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Fecha y hora en que se creó la entidad.
		/// </summary>
		public DateTime FechaCreacion { get; set; } = DateTime.Now;

		/// <summary>
		/// Fecha y hora de la última actualización de la entidad.
		/// </summary>
		public DateTime FechaActualizacion { get; set; } = DateTime.Now;

		/// <summary>
		/// Indica si la entidad está activa o no.
		/// </summary>
		public bool Activo { get; set; } = true;


		/// <summary>
		/// Método protegido para actualizar la fecha de modificación.
		/// Se llama automáticamente cuando se modifica la entidad.
		/// </summary>
		protected void ActualizarFecha()
		{
			FechaActualizacion = DateTime.Now;
		}
	}
}
```

##### Persona.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;

/// <summary>
/// Espacio de nombres para los modelos de la aplicación EduFairOS.
/// </summary>
namespace EduFairOS.Models
{

	/// <summary>
	/// Clase abstracta que representa una persona en el sistema.
	/// Hereda de EntidadBase y proporciona propiedades comunes como nombre, edad, teléfono y correo.
	/// </summary>
	public abstract class Persona : EntidadBase
	{
		private string _nombre;
		private int _edad;
		private string _telefono;
		private string _correo;

		/// <summary>
		/// Obtiene o establece el nombre de la persona.
		/// Valida que no esté vacío y actualiza la fecha de modificación.
		/// </summary>
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

		/// <summary>
		/// Obtiene o establece la edad de la persona.
		/// Valida que esté entre 5 y 100 años y actualiza la fecha de modificación.
		/// </summary>
		public int Edad
		{
			get { return _edad; }
			set
			{
				if (value < 5 || value > 100)
					throw new ArgumentException("Edad debe estar entre 5 y 100 años");
				_edad = value;
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Obtiene o establece el teléfono de la persona.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public string Telefono
		{
			get { return _telefono; }
			set
			{
				_telefono = value?.Trim() ?? string.Empty;
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Obtiene o establece el correo electrónico de la persona.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public string Correo
		{
			get { return _correo; }
			set
			{
				_correo = value?.Trim() ?? string.Empty;
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Constructor sin parámetros.
		/// Inicializa las propiedades con valores por defecto.
		/// </summary>
		public Persona()
		{
			_nombre = string.Empty;
			_edad = 0;
			_telefono = string.Empty;
			_correo = string.Empty;
		}

		/// <summary>
		/// Constructor con parámetros.
		/// Inicializa las propiedades con los valores proporcionados.
		/// </summary>
		/// <param name="nombre">El nombre de la persona.</param>
		/// <param name="edad">La edad de la persona.</param>
		/// <param name="telefono">El teléfono de la persona.</param>
		/// <param name="correo">El correo electrónico de la persona.</param>
		public Persona(string nombre, int edad, string telefono, string correo)
		{
			Nombre = nombre;
			Edad = edad;
			Telefono = telefono;
			Correo = correo;
		}
	}
}
```

##### Participante.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;

/// <summary>
/// Espacio de nombres para los modelos de la aplicación EduFairOS.
/// </summary>
namespace EduFairOS.Models
{

	/// <summary>
	/// Clase que representa un Participante en la Feria Escolar.
	/// Hereda de Persona y agrega propiedades específicas como grado, institución, fecha de registro y actividades completadas.
	/// </summary>
	public class Participante : Persona
	{
		private string _grado;
		private string _institucion;
		private DateTime _fechaRegistro;
		private int _actividadesCompletadas;

		/// <summary>
		/// Obtiene o establece el grado del participante.
		/// Valida que no esté vacío y actualiza la fecha de modificación.
		/// </summary>
		public string Grado
		{
			get { return _grado; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Grado no puede estar vacío");
				_grado = value.Trim();
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Obtiene o establece la institución del participante.
		/// Valida que no esté vacía y actualiza la fecha de modificación.
		/// </summary>
		public string Institucion
		{
			get { return _institucion; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Institución no puede estar vacía");
				_institucion = value.Trim();
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Obtiene o establece la fecha de registro del participante.
		/// </summary>
		public DateTime FechaRegistro
		{
			get { return _fechaRegistro; }
			set { _fechaRegistro = value; }
		}

		/// <summary>
		/// Obtiene o establece el número de actividades completadas por el participante.
		/// </summary>
		public int ActividadesCompletadas
		{
			get { return _actividadesCompletadas; }
			set { _actividadesCompletadas = value; }
		}

		/// <summary>
		/// Constructor sin parámetros.
		/// Inicializa las propiedades con valores por defecto.
		/// </summary>
		public Participante() : base()
		{
			_grado = string.Empty;
			_institucion = string.Empty;
			_fechaRegistro = DateTime.Now;
			_actividadesCompletadas = 0;
		}

		/// <summary>
		/// Constructor con datos esenciales.
		/// Inicializa el participante con nombre, edad, grado e institución.
		/// </summary>
		/// <param name="nombre">El nombre del participante.</param>
		/// <param name="edad">La edad del participante.</param>
		/// <param name="grado">El grado del participante.</param>
		/// <param name="institucion">La institución del participante.</param>
		public Participante(string nombre, int edad, string grado, string institucion)
			: base(nombre, edad, string.Empty, string.Empty)
		{
			Grado = grado;
			Institucion = institucion;
			_fechaRegistro = DateTime.Now;
			_actividadesCompletadas = 0;
		}
	}
}
```

##### Evento.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;

/// <summary>
/// Espacio de nombres para los modelos de la aplicación EduFairOS.
/// </summary>
namespace EduFairOS.Models
{

	/// <summary>
	/// Clase que representa un Evento de Feria Escolar.
	/// Contiene información sobre el nombre, fechas, lugar, descripción y estado del evento.
	/// </summary>
	public class Evento : EntidadBase
	{
		private string _nombre;
		private DateTime _fechaInicio;
		private DateTime _fechaFin;
		private string _lugar;
		private string _descripcion;
		private string _estado;

		/// <summary>
		/// Obtiene o establece el nombre del evento.
		/// Valida que no esté vacío y actualiza la fecha de modificación.
		/// </summary>
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

		/// <summary>
		/// Obtiene o establece la fecha de inicio del evento.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public DateTime FechaInicio
		{
			get { return _fechaInicio; }
			set
			{
				_fechaInicio = value;
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Obtiene o establece la fecha de fin del evento.
		/// Valida que no sea anterior a la fecha de inicio y actualiza la fecha de modificación.
		/// </summary>
		public DateTime FechaFin
		{
			get { return _fechaFin; }
			set
			{
				if (value < FechaInicio)
					throw new ArgumentException("La fecha fin no puede ser anterior a la fecha inicio");
				_fechaFin = value;
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Obtiene o establece el lugar del evento.
		/// Valida que no esté vacío y actualiza la fecha de modificación.
		/// </summary>
		public string Lugar
		{
			get { return _lugar; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("El lugar no puede estar vacío");
				_lugar = value.Trim();
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Obtiene o establece la descripción del evento.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public string Descripcion
		{
			get { return _descripcion; }
			set
			{
				_descripcion = value ?? string.Empty;
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Obtiene o establece el estado del evento (ej. Planificación, Activo, Finalizado).
		/// Actualiza la fecha de modificación.
		/// </summary>
		public string Estado
		{
			get { return _estado; }
			set
			{
				_estado = value ?? "Planificación";
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Constructor sin parámetros.
		/// Inicializa las propiedades con valores por defecto.
		/// </summary>
		public Evento()
		{
			_nombre = string.Empty;
			_fechaInicio = DateTime.Now;
			_fechaFin = DateTime.Now.AddDays(1);
			_lugar = string.Empty;
			_descripcion = string.Empty;
			_estado = "Planificación";
		}

		/// <summary>
		/// Constructor con parámetros.
		/// Inicializa el evento con nombre, fechas y lugar.
		/// </summary>
		/// <param name="nombre">El nombre del evento.</param>
		/// <param name="fechaInicio">La fecha de inicio del evento.</param>
		/// <param name="fechaFin">La fecha de fin del evento.</param>
		/// <param name="lugar">El lugar del evento.</param>
		public Evento(string nombre, DateTime fechaInicio, DateTime fechaFin, string lugar)
		{
			Nombre = nombre;
			FechaInicio = fechaInicio;
			FechaFin = fechaFin;
			Lugar = lugar;
			_descripcion = string.Empty;
			_estado = "Planificación";
		}
	}
}
```

##### Stand.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System.Collections.Generic;
using System;

/// <summary>
/// Espacio de nombres para los modelos de la aplicación EduFairOS.
/// </summary>
namespace EduFairOS.Models
{

	/// <summary>
	/// Clase que representa un Stand dentro de un Evento.
	/// Contiene información sobre el nombre, ubicación, categoría, encargado, evento asociado, descripción y estado de ocupación.
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

		/// <summary>
		/// Obtiene o establece el nombre del stand.
		/// Valida que no esté vacío y actualiza la fecha de modificación.
		/// </summary>
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

		/// <summary>
		/// Obtiene o establece la ubicación del stand.
		/// Valida que no esté vacía y actualiza la fecha de modificación.
		/// </summary>
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

		/// <summary>
		/// Obtiene o establece la categoría del stand.
		/// Valida que no esté vacía y actualiza la fecha de modificación.
		/// </summary>
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

		/// <summary>
		/// Obtiene o establece el ID del encargado del stand.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public int EncargadoId
		{
			get { return _encargadoId; }
			set { _encargadoId = value; ActualizarFecha(); }
		}

		/// <summary>
		/// Obtiene o establece el ID del evento al que pertenece el stand.
		/// </summary>
		public int IdEvento
		{
			get { return _idEvento; }
			set { _idEvento = value; }
		}

		/// <summary>
		/// Obtiene o establece la descripción del stand.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public string Descripcion
		{
			get { return _descripcion; }
			set { _descripcion = value ?? string.Empty; ActualizarFecha(); }
		}

		/// <summary>
		/// Obtiene o establece si el stand está ocupado.
		/// </summary>
		public bool Ocupado
		{
			get { return _ocupado; }
			set { _ocupado = value; }
		}

		/// <summary>
		/// Constructor sin parámetros.
		/// Inicializa las propiedades con valores por defecto.
		/// </summary>
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

		/// <summary>
		/// Constructor con datos esenciales.
		/// Inicializa el stand con nombre, ubicación, categoría e ID de evento.
		/// </summary>
		/// <param name="nombre">El nombre del stand.</param>
		/// <param name="ubicacion">La ubicación del stand.</param>
		/// <param name="categoria">La categoría del stand.</param>
		/// <param name="idEvento">El ID del evento al que pertenece el stand.</param>
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
```

##### Actividad.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;

/// <summary>
/// Espacio de nombres para los modelos de la aplicación EduFairOS.
/// </summary>
namespace EduFairOS.Models
{

	/// <summary>
	/// Clase que representa una Actividad dentro de un Stand.
	/// Contiene información sobre el nombre, descripción, horarios, capacidad, participantes y nivel de la actividad.
	/// </summary>
	public class Actividad : EntidadBase
	{
		private string _nombre;
		private string _descripcion;
		private DateTime _horaInicio;
		private DateTime _horaFin;
		private int _idStand;
		private int _monitorId;
		private int _capacidadMaxima;
		private int _participantesActuales;
		private List<int> _participantesIds;
		private string _nivel;

		/// <summary>
		/// Obtiene o establece el nombre de la actividad.
		/// Valida que no esté vacío y actualiza la fecha de modificación.
		/// </summary>
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

		/// <summary>
		/// Obtiene o establece la descripción de la actividad.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public string Descripcion
		{
			get { return _descripcion; }
			set { _descripcion = value ?? string.Empty; ActualizarFecha(); }
		}

		/// <summary>
		/// Obtiene o establece la hora de inicio de la actividad.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public DateTime HoraInicio
		{
			get { return _horaInicio; }
			set { _horaInicio = value; ActualizarFecha(); }
		}

		/// <summary>
		/// Obtiene o establece la hora de fin de la actividad.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public DateTime HoraFin
		{
			get { return _horaFin; }
			set { _horaFin = value; ActualizarFecha(); }
		}

		/// <summary>
		/// Obtiene o establece el ID del stand al que pertenece la actividad.
		/// </summary>
		public int IdStand
		{
			get { return _idStand; }
			set { _idStand = value; }
		}

		/// <summary>
		/// Obtiene o establece el ID del monitor de la actividad.
		/// Actualiza la fecha de modificación.
		/// </summary>
		public int MonitorId
		{
			get { return _monitorId; }
			set { _monitorId = value; ActualizarFecha(); }
		}

		/// <summary>
		/// Obtiene o establece la capacidad máxima de la actividad.
		/// Valida que esté entre 1 y 1000 y actualiza la fecha de modificación.
		/// </summary>
		public int CapacidadMaxima
		{
			get { return _capacidadMaxima; }
			set
			{
				if (value <= 0 || value > 1000)
					throw new ArgumentException("Capacidad debe estar entre 1 y 1000");
				_capacidadMaxima = value;
				ActualizarFecha();
			}
		}

		/// <summary>
		/// Obtiene o establece el número actual de participantes en la actividad.
		/// </summary>
		public int ParticipantesActuales
		{
			get { return _participantesActuales; }
			set { _participantesActuales = value; }
		}

		/// <summary>
		/// Obtiene o establece el nivel de la actividad (ej. Básico, Intermedio, Avanzado).
		/// Actualiza la fecha de modificación.
		/// </summary>
		public string Nivel
		{
			get { return _nivel; }
			set { _nivel = value ?? "Básico"; ActualizarFecha(); }
		}

		/// <summary>
		/// Constructor sin parámetros.
		/// Inicializa las propiedades con valores por defecto.
		/// </summary>
		public Actividad() : base()
		{
			_nombre = string.Empty;
			_descripcion = string.Empty;
			_horaInicio = DateTime.Now;
			_horaFin = DateTime.Now.AddHours(1);
			_idStand = 0;
			_monitorId = 0;
			_capacidadMaxima = 30;
			_participantesActuales = 0;
			_participantesIds = new List<int>();
			_nivel = "Básico";
		}

		/// <summary>
		/// Constructor con datos esenciales.
		/// Inicializa la actividad con nombre, hora de inicio y hora de fin.
		/// </summary>
		/// <param name="nombre">El nombre de la actividad.</param>
		/// <param name="horaInicio">La hora de inicio de la actividad.</param>
		/// <param name="horaFin">La hora de fin de la actividad.</param>
		public Actividad(string nombre, DateTime horaInicio, DateTime horaFin) : this()
		{
			Nombre = nombre;
			HoraInicio = horaInicio;
			HoraFin = horaFin;
		}
	}
}
```

##### Asignacion.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;

/// <summary>
/// Espacio de nombres para los modelos de la aplicación EduFairOS.
/// </summary>
namespace EduFairOS.Models
{

	/// <summary>
	/// Clase que representa la asignación de un participante a una actividad.
	/// Contiene información sobre el participante, la actividad, fecha de asignación, estado y puntuación.
	/// </summary>
	public class Asignacion : EntidadBase
	{
		private int _idParticipante;
		private int _idActividad;
		private DateTime _fechaAsignacion;
		private string _estado;
		private decimal _puntuacion;

		/// <summary>
		/// Obtiene o establece el ID del participante asignado.
		/// </summary>
		public int IdParticipante
		{
			get { return _idParticipante; }
			set { _idParticipante = value; }
		}

		/// <summary>
		/// Obtiene o establece el ID de la actividad asignada.
		/// </summary>
		public int IdActividad
		{
			get { return _idActividad; }
			set { _idActividad = value; }
		}

		/// <summary>
		/// Obtiene o establece la fecha de asignación.
		/// </summary>
		public DateTime FechaAsignacion
		{
			get { return _fechaAsignacion; }
			set { _fechaAsignacion = value; }
		}

		/// <summary>
		/// Obtiene o establece el estado de la asignación (ej. Pendiente, Confirmada, Cancelada).
		/// </summary>
		public string Estado
		{
			get { return _estado; }
			set { _estado = value ?? "Pendiente"; }
		}

		/// <summary>
		/// Obtiene o establece la puntuación obtenida en la actividad.
		/// </summary>
		public decimal Puntuacion
		{
			get { return _puntuacion; }
			set { _puntuacion = value; }
		}

		/// <summary>
		/// Constructor sin parámetros.
		/// Inicializa las propiedades con valores por defecto.
		/// </summary>
		public Asignacion()
		{
			_idParticipante = 0;
			_idActividad = 0;
			_fechaAsignacion = DateTime.Now;
			_estado = "Pendiente";
			_puntuacion = 0;
		}

		/// <summary>
		/// Constructor con parámetros.
		/// Inicializa la asignación con IDs de participante y actividad.
		/// </summary>
		/// <param name="idParticipante">El ID del participante.</param>
		/// <param name="idActividad">El ID de la actividad.</param>
		public Asignacion(int idParticipante, int idActividad)
		{
			IdParticipante = idParticipante;
			IdActividad = idActividad;
			_fechaAsignacion = DateTime.Now;
			_estado = "Pendiente";
			_puntuacion = 0;
		}
	}
}
```

#### Core
##### BaseRepository.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903

/// <summary>
/// Espacio de nombres para el núcleo del dominio de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Domain.Core
{
	/// <summary>
	/// Clase base abstracta para repositorios.
	/// Proporciona una base común para implementar repositorios de entidades.
	/// </summary>
	/// <typeparam name="T">El tipo de entidad que maneja el repositorio.</typeparam>
	public abstract class BaseRepository<T> where T : class
	{

	}
}
```

##### BaseSpecification.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Linq.Expressions;

/// <summary>
/// Espacio de nombres para el núcleo del dominio de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Domain.Core
{
    /// <summary>
    /// Clase base abstracta para especificaciones.
    /// Permite definir criterios de consulta para entidades de manera reutilizable.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad para la especificación.</typeparam>
    public abstract class BaseSpecification<T>
    {
        /// <summary>
        /// Obtiene el criterio de la especificación como una expresión lambda.
        /// </summary>
        public Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Constructor protegido que inicializa el criterio de la especificación.
        /// </summary>
        /// <param name="criteria">La expresión que define el criterio de filtrado.</param>
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
    }
}
```

##### Enums.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903

/// <summary>
/// Espacio de nombres para el núcleo del dominio de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Domain.Core
{
	/// <summary>
	/// Enumeración que define los posibles estados de un evento.
	/// </summary>
	public enum EstadoEvento
	{
		/// <summary>
		/// El evento está en fase de planificación.
		/// </summary>
		Planificacion,

		/// <summary>
		/// El evento está activo y en curso.
		/// </summary>
		Activo,

		/// <summary>
		/// El evento ha sido cancelado.
		/// </summary>
		Cancelado,

		/// <summary>
		/// El evento ha finalizado.
		/// </summary>
		Finalizado
	}

	/// <summary>
	/// Enumeración que define las categorías de edad para participantes.
	/// </summary>
	public enum CategoriaEdad
	{
		/// <summary>
		/// Categoría para edades infantiles.
		/// </summary>
		Infantil,

		/// <summary>
		/// Categoría para edades juveniles.
			/// </summary>
		Juvenil,

		/// <summary>
		/// Categoría para edades adultas.
		/// </summary>
		Adulto
	}
}
```

##### Exceptions.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;

/// <summary>
/// Espacio de nombres para el núcleo del dominio de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Domain.Core
{
	/// <summary>
	/// Excepción personalizada para la aplicación EduFairOS.
	/// Hereda de Exception y permite manejar errores específicos del dominio.
	/// </summary>
	public class EduFairException : Exception
	{
		/// <summary>
		/// Constructor sin parámetros.
		/// </summary>
		public EduFairException() : base() { }

		/// <summary>
		/// Constructor con mensaje de error.
		/// </summary>
		/// <param name="message">El mensaje que describe el error.</param>
		public EduFairException(string message) : base(message) { }

		/// <summary>
		/// Constructor con mensaje de error y excepción interna.
		/// </summary>
		/// <param name="message">El mensaje que describe el error.</param>
		/// <param name="innerException">La excepción interna que causó este error.</param>
		public EduFairException(string message, Exception innerException) : base(message, innerException) { }
	}
}
```

#### Application Layer
##### Contracts
###### IServicioActividad.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System.Collections.Generic;
using EduFairOS.Models;

/// <summary>
/// Espacio de nombres para los contratos de la capa de aplicación de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Application.Contracts
{
	/// <summary>
	/// Interfaz que define las operaciones para el servicio de actividades.
	/// Proporciona métodos para crear, obtener, actualizar y eliminar actividades.
	/// </summary>
	public interface IServicioActividad
	{
		/// <summary>
		/// Crea una nueva actividad.
		/// </summary>
		/// <param name="actividad">La actividad a crear.</param>
		/// <returns>True si la actividad se creó exitosamente, false en caso contrario.</returns>
		bool CrearActividad(Actividad actividad);

		/// <summary>
		/// Obtiene una actividad por su ID.
		/// </summary>
		/// <param name="id">El ID de la actividad.</param>
		/// <returns>La actividad encontrada, o null si no existe.</returns>
		Actividad ObtenerActividad(int id);

		/// <summary>
		/// Obtiene todas las actividades.
		/// </summary>
		/// <returns>Una lista de todas las actividades.</returns>
		List<Actividad> ObtenerTodasActividades();

		/// <summary>
		/// Actualiza una actividad existente.
		/// </summary>
		/// <param name="actividad">La actividad con los datos actualizados.</param>
		/// <returns>True si la actividad se actualizó exitosamente, false en caso contrario.</returns>
		bool ActualizarActividad(Actividad actividad);

		/// <summary>
		/// Elimina una actividad por su ID.
		/// </summary>
		/// <param name="id">El ID de la actividad a eliminar.</param>
		/// <returns>True si la actividad se eliminó exitosamente, false en caso contrario.</returns>
		bool EliminarActividad(int id);
	}
}
```

###### IServicioEvento.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System.Collections.Generic;
using EduFairOS.Models;

/// <summary>
/// Espacio de nombres para los contratos de la capa de aplicación de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Application.Contracts
{
	/// <summary>
	/// Interfaz que define las operaciones para el servicio de eventos.
	/// Proporciona métodos para crear, obtener, actualizar, eliminar y gestionar el estado de eventos.
	/// </summary>
	public interface IServicioEvento
	{
		/// <summary>
		/// Crea un nuevo evento.
		/// </summary>
		/// <param name="evento">El evento a crear.</param>
		/// <returns>True si el evento se creó exitosamente, false en caso contrario.</returns>
		bool CrearEvento(Evento evento);

		/// <summary>
		/// Obtiene un evento por su ID.
		/// </summary>
		/// <param name="id">El ID del evento.</param>
		/// <returns>El evento encontrado, o null si no existe.</returns>
		Evento ObtenerEvento(int id);

		/// <summary>
		/// Obtiene todos los eventos.
		/// </summary>
		/// <returns>Una lista de todos los eventos.</returns>
		List<Evento> ObtenerTodosEventos();

		/// <summary>
		/// Actualiza un evento existente.
		/// </summary>
		/// <param name="evento">El evento con los datos actualizados.</param>
		/// <returns>True si el evento se actualizó exitosamente, false en caso contrario.</returns>
		bool ActualizarEvento(Evento evento);

		/// <summary>
		/// Elimina un evento por su ID.
		/// </summary>
		/// <param name="id">El ID del evento a eliminar.</param>
		/// <returns>True si el evento se eliminó exitosamente, false en caso contrario.</returns>
		bool EliminarEvento(int id);

		/// <summary>
		/// Cancela un evento por su ID.
		/// </summary>
		/// <param name="id">El ID del evento a cancelar.</param>
		/// <returns>True si el evento se canceló exitosamente, false en caso contrario.</returns>
		bool CancelarEvento(int id);

		/// <summary>
		/// Activa un evento por su ID.
		/// </summary>
		/// <param name="id">El ID del evento a activar.</param>
		/// <returns>True si el evento se activó exitosamente, false en caso contrario.</returns>
		bool ActivarEvento(int id);

		/// <summary>
		/// Finaliza un evento por su ID.
		/// </summary>
		/// <param name="id">El ID del evento a finalizar.</param>
		/// <returns>True si el evento se finalizó exitosamente, false en caso contrario.</returns>
		bool FinalizarEvento(int id);
	}
}
```

###### IServicioParticipante.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System.Collections.Generic;
using EduFairOS.Models;

/// <summary>
/// Espacio de nombres para los contratos de la capa de aplicación de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Application.Contracts
{
	/// <summary>
	/// Interfaz que define las operaciones para el servicio de participantes.
	/// Proporciona métodos para registrar, obtener, actualizar, eliminar y buscar participantes, además de generar estadísticas.
	/// </summary>
	public interface IServicioParticipante
	{
		/// <summary>
		/// Registra un nuevo participante.
		/// </summary>
		/// <param name="participante">El participante a registrar.</param>
		/// <returns>True si el participante se registró exitosamente, false en caso contrario.</returns>
		bool RegistrarParticipante(Participante participante);

		/// <summary>
		/// Obtiene un participante por su ID.
		/// </summary>
		/// <param name="id">El ID del participante.</param>
		/// <returns>El participante encontrado, o null si no existe.</returns>
		Participante ObtenerParticipante(int id);

		/// <summary>
		/// Obtiene todos los participantes.
		/// </summary>
		/// <returns>Una lista de todos los participantes.</returns>
		List<Participante> ObtenerTodosParticipantes();

		/// <summary>
		/// Obtiene participantes por institución.
		/// </summary>
		/// <param name="institucion">El nombre de la institución.</param>
		/// <returns>Una lista de participantes de la institución especificada.</returns>
		List<Participante> ObtenerPorInstitucion(string institucion);

		/// <summary>
		/// Obtiene participantes por categoría de edad.
		/// </summary>
		/// <param name="categoria">La categoría de edad.</param>
		/// <returns>Una lista de participantes de la categoría especificada.</returns>
		List<Participante> ObtenerPorCategoriaEdad(string categoria);

		/// <summary>
		/// Actualiza un participante existente.
		/// </summary>
		/// <param name="participante">El participante con los datos actualizados.</param>
		/// <returns>True si el participante se actualizó exitosamente, false en caso contrario.</returns>
		bool ActualizarParticipante(Participante participante);

		/// <summary>
		/// Elimina un participante por su ID.
		/// </summary>
		/// <param name="id">El ID del participante a eliminar.</param>
		/// <returns>True si el participante se eliminó exitosamente, false en caso contrario.</returns>
		bool EliminarParticipante(int id);

		/// <summary>
		/// Busca participantes por nombre.
		/// </summary>
		/// <param name="nombre">El nombre a buscar.</param>
		/// <returns>Una lista de participantes que coinciden con el nombre.</returns>
		List<Participante> BuscarPorNombre(string nombre);

		/// <summary>
		/// Genera estadísticas de los participantes.
		/// </summary>
		/// <returns>Una cadena con las estadísticas generadas.</returns>
		string GenerarEstadisticas();
	}
}
```

###### IServicioStand.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System.Collections.Generic;
using EduFairOS.Models;

/// <summary>
/// Espacio de nombres para los contratos de la capa de aplicación de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Application.Contracts
{
	/// <summary>
	/// Interfaz que define las operaciones para el servicio de stands.
	/// Proporciona métodos para crear, obtener, actualizar y eliminar stands.
	/// </summary>
	public interface IServicioStand
	{
		/// <summary>
		/// Crea un nuevo stand.
		/// </summary>
		/// <param name="stand">El stand a crear.</param>
		/// <returns>True si el stand se creó exitosamente, false en caso contrario.</returns>
		bool CrearStand(Stand stand);

		/// <summary>
		/// Obtiene un stand por su ID.
		/// </summary>
		/// <param name="id">El ID del stand.</param>
		/// <returns>El stand encontrado, o null si no existe.</returns>
		Stand ObtenerStand(int id);

		/// <summary>
		/// Obtiene todos los stands.
		/// </summary>
		/// <returns>Una lista de todos los stands.</returns>
		List<Stand> ObtenerTodosStands();

		/// <summary>
		/// Actualiza un stand existente.
		/// </summary>
		/// <param name="stand">El stand con los datos actualizados.</param>
		/// <returns>True si el stand se actualizó exitosamente, false en caso contrario.</returns>
		bool ActualizarStand(Stand stand);

		/// <summary>
		/// Elimina un stand por su ID.
		/// </summary>
		/// <param name="id">El ID del stand a eliminar.</param>
		/// <returns>True si el stand se eliminó exitosamente, false en caso contrario.</returns>
		bool EliminarStand(int id);
	}
}
```

##### Services
###### ServicioActividad.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;
using EduFairOS.Models;
using EduFairOS.Layers.Infrastructure.Data;

/// <summary>
/// Espacio de nombres para los servicios de la capa de aplicación de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Application.Services
{
	/// <summary>
	/// Servicio que maneja las operaciones relacionadas con actividades.
	/// Implementa la lógica de negocio para crear, obtener, actualizar y eliminar actividades.
	/// </summary>
	public class ServicioActividad
	{
		private RepositorioActividad _repositorio;

		/// <summary>
		/// Constructor que inicializa el repositorio de actividades.
		/// </summary>
		public ServicioActividad()
		{
			_repositorio = new RepositorioActividad();
		}

		/// <summary>
		/// Crea una nueva actividad.
		/// Valida que la actividad no sea nula y que tenga un nombre.
		/// </summary>
		/// <param name="actividad">La actividad a crear.</param>
		/// <returns>True si la actividad se creó exitosamente, false en caso contrario.</returns>
		public bool CrearActividad(Actividad actividad)
		{
			if (actividad == null) throw new ArgumentNullException(nameof(actividad));
			if (string.IsNullOrEmpty(actividad.Nombre))
				throw new Exception("El nombre de la actividad es obligatorio");
			return _repositorio.Agregar(actividad);
		}

		/// <summary>
		/// Obtiene una actividad por su ID.
		/// </summary>
		/// <param name="id">El ID de la actividad.</param>
		/// <returns>La actividad encontrada, o null si no existe.</returns>
		public Actividad ObtenerActividad(int id)
		{
			return _repositorio.ObtenerPorId(id);
		}

		/// <summary>
		/// Obtiene todas las actividades.
		/// </summary>
		/// <returns>Una lista de todas las actividades.</returns>
		public List<Actividad> ObtenerTodasActividades()
		{
			return _repositorio.ObtenerTodos();
		}

		/// <summary>
		/// Actualiza una actividad existente.
		/// Valida que la actividad exista antes de actualizar.
		/// </summary>
		/// <param name="actividad">La actividad con los datos actualizados.</param>
		/// <returns>True si la actividad se actualizó exitosamente, false en caso contrario.</returns>
		public bool ActualizarActividad(Actividad actividad)
		{
			if (actividad == null || actividad.Id <= 0) return false;
			var existente = ObtenerActividad(actividad.Id);
			if (existente == null) return false;
			return _repositorio.Actualizar(actividad);
		}

		/// <summary>
		/// Elimina una actividad por su ID.
		/// Valida que la actividad exista antes de eliminar.
		/// </summary>
		/// <param name="id">El ID de la actividad a eliminar.</param>
		/// <returns>True si la actividad se eliminó exitosamente, false en caso contrario.</returns>
		public bool EliminarActividad(int id)
		{
			var actividad = ObtenerActividad(id);
			if (actividad == null) return false;
			return _repositorio.Eliminar(id);
		}
	}
}
```

#### Infrastructure Layer
##### Interfaces
###### IRepositorio.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Collections.Generic;

/// <summary>
/// Espacio de nombres para las interfaces de la capa de infraestructura de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Infrastructure.Interfaces
{

	/// <summary>
	/// Interfaz genérica para operaciones CRUD.
	/// Define el contrato para las clases de acceso a datos, proporcionando métodos para agregar, obtener, actualizar y eliminar entidades.
	/// </summary>
	/// <typeparam name="T">El tipo de entidad que maneja el repositorio.</typeparam>
	public interface IRepositorio<T> where T : class
	{
		/// <summary>
		/// Agrega una nueva entidad al repositorio.
		/// </summary>
		/// <param name="entidad">La entidad a agregar.</param>
		/// <returns>True si la entidad se agregó exitosamente, false en caso contrario.</returns>
		bool Agregar(T entidad);

		/// <summary>
		/// Obtiene una entidad por su ID.
		/// </summary>
		/// <param name="id">El ID de la entidad.</param>
		/// <returns>La entidad encontrada, o null si no existe.</returns>
		T ObtenerPorId(int id);

		/// <summary>
		/// Obtiene todas las entidades del repositorio.
		/// </summary>
		/// <returns>Una lista de todas las entidades.</returns>
		List<T> ObtenerTodos();

		/// <summary>
		/// Actualiza una entidad existente.
		/// </summary>
		/// <param name="entidad">La entidad con los datos actualizados.</param>
		/// <returns>True si la entidad se actualizó exitosamente, false en caso contrario.</returns>
		bool Actualizar(T entidad);

		/// <summary>
		/// Elimina una entidad por su ID.
		/// </summary>
		/// <param name="id">El ID de la entidad a eliminar.</param>
		/// <returns>True si la entidad se eliminó exitosamente, false en caso contrario.</returns>
		bool Eliminar(int id);

		/// <summary>
		/// Verifica si una entidad existe por su ID.
		/// </summary>
		/// <param name="id">El ID de la entidad.</param>
		/// <returns>True si la entidad existe, false en caso contrario.</returns>
		bool Existe(int id);

		/// <summary>
		/// Agrega múltiples entidades al repositorio.
		/// </summary>
		/// <param name="entidades">La lista de entidades a agregar.</param>
		/// <returns>El número de entidades agregadas exitosamente.</returns>
		int AgregarMultiples(List<T> entidades);

		/// <summary>
		/// Obtiene entidades que cumplen con un predicado.
		/// </summary>
		/// <param name="predicate">El predicado para filtrar las entidades.</param>
		/// <returns>Una lista de entidades que cumplen con el predicado.</returns>
		List<T> ObtenerPor(Func<T, bool> predicate);

		/// <summary>
		/// Obtiene la cantidad total de entidades en el repositorio.
		/// </summary>
		/// <returns>El número total de entidades.</returns>
		int ObtenerCantidad();

		/// <summary>
		/// Obtiene la primera entidad que cumple con un predicado.
		/// </summary>
		/// <param name="predicate">El predicado para buscar la entidad.</param>
		/// <returns>La primera entidad que cumple con el predicado, o null si no se encuentra.</returns>
		T ObtenerPrimero(Func<T, bool> predicate);

		/// <summary>
		/// Valida una entidad según las reglas de negocio.
		/// </summary>
		/// <param name="entidad">La entidad a validar.</param>
		/// <returns>True si la entidad es válida, false en caso contrario.</returns>
		bool ValidarEntidad(T entidad);

		/// <summary>
		/// Elimina múltiples entidades por sus IDs.
		/// </summary>
		/// <param name="ids">La lista de IDs de las entidades a eliminar.</param>
		/// <returns>El número de entidades eliminadas exitosamente.</returns>
		int EliminarMultiples(List<int> ids);
	}
}
```

##### Data
###### ConexionBD.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Data;
using Microsoft.Data.SqlClient;

/// <summary>
/// Espacio de nombres para los datos de la capa de infraestructura de EduFairOS.
/// </summary>
namespace EduFairOS.Layers.Infrastructure.Data
{
	/// <summary>
	/// Clase que maneja la conexión a la base de datos SQL Server.
	/// Implementa el patrón Singleton para asegurar una única instancia de conexión.
	/// </summary>
	public class ConexionBD
	{
		private static ConexionBD _instancia;
		private string _cadenaConexion;
		private SqlConnection _conexion;

		/// <summary>
		/// Obtiene o establece la cadena de conexión a la base de datos.
		/// </summary>
		public string CadenaConexion
		{
			get { return _cadenaConexion; }
			set { _cadenaConexion = value; }
		}

		/// <summary>
		/// Constructor privado para implementar el patrón Singleton.
		/// Inicializa la cadena de conexión y crea la conexión SQL.
		/// </summary>
		private ConexionBD()
		{

			_cadenaConexion = @"Server=ASHLEYFELIZPC\MSSQLSERVER01;Database=EduFairOS;Trusted_Connection=True;TrustServerCertificate=True;";
			_conexion = new SqlConnection(_cadenaConexion);
		}

		/// <summary>
		/// Obtiene la instancia única de la clase ConexionBD.
		/// </summary>
		/// <returns>La instancia única de ConexionBD.</returns>
		public static ConexionBD ObtenerInstancia()
		{
			if (_instancia == null)
			{
				_instancia = new ConexionBD();
			}
			return _instancia;
		}

		/// <summary>
		/// Establece la conexión a la base de datos.
		/// </summary>
		/// <returns>True si la conexión se estableció exitosamente, false en caso contrario.</returns>
		public bool Conectar()
		{
			try
			{

				if (_conexion.State != ConnectionState.Open)
				{
					if (_conexion.State == ConnectionState.Broken)
						_conexion.Close(); 

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

		/// <summary>
		/// Cierra la conexión a la base de datos.
		/// </summary>
		/// <returns>True si la conexión se cerró exitosamente, false en caso contrario.</returns>
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

		/// <summary>
		/// Obtiene la conexión SQL actual.
		/// </summary>
		/// <returns>La instancia de SqlConnection.</returns>
		public SqlConnection ObtenerConexion()
		{
			return _conexion;
		}

		/// <summary>
		/// Verifica si la conexión está abierta.
		/// </summary>
		/// <returns>True si la conexión está abierta, false en caso contrario.</returns>
		public bool EstaConectado()
		{
			return _conexion.State == ConnectionState.Open;
		}

		/// <summary>
		/// Ejecuta un comando SQL sin retorno de datos.
		/// </summary>
		/// <param name="sql">La consulta SQL a ejecutar.</param>
		/// <returns>El número de filas afectadas.</returns>
		public int EjecutarComando(string sql)
		{
			try
			{
				Conectar(); // Para asegurar que conecte siempre
				using SqlCommand command = new SqlCommand(sql, _conexion);
				return command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al ejecutar comando: {ex.Message}");
				return 0;
			}
		}

		/// <summary>
		/// Ejecuta una consulta SQL y devuelve un SqlDataReader.
		/// </summary>
		/// <param name="sql">La consulta SQL a ejecutar.</param>
		/// <returns>Un SqlDataReader con los resultados de la consulta.</returns>
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

		/// <summary>
		/// Ejecuta un comando SQL con parámetros sin retorno de datos.
		/// </summary>
		/// <param name="query">La consulta SQL a ejecutar.</param>
		/// <param name="parameters">Los parámetros para la consulta.</param>
		/// <returns>El número de filas afectadas.</returns>
		public int EjecutarNonQuery(string query, SqlParameter[] parameters)
		{
			Conectar(); // Para forzar a que siempre se abra antes de ejecutar
			using SqlCommand command = new SqlCommand(query, _conexion);
			command.Parameters.AddRange(parameters);
			return command.ExecuteNonQuery();
		}

		/// <summary>
		/// Ejecuta una consulta SQL con parámetros y devuelve un SqlDataReader.
		/// </summary>
		/// <param name="query">La consulta SQL a ejecutar.</param>
		/// <param name="parameters">Los parámetros para la consulta (opcional).</param>
		/// <returns>Un SqlDataReader con los resultados de la consulta.</returns>
		public SqlDataReader EjecutarReader(string query, SqlParameter[] parameters = null)
		{
			Conectar();
			SqlCommand command = new SqlCommand(query, _conexion);
			if (parameters != null) command.Parameters.AddRange(parameters);


			return command.ExecuteReader();
		}
	}
}
```

#### Presentation Layer
##### Controllers
###### Program.cs
```csharp
//Ashley Esmirna Feliz Rodríguez 2025-0903

/// <summary>
/// Punto de entrada principal para la aplicación EduFairOS.
/// Esta clase configura el host de la aplicación web, registra servicios y define el pipeline de middleware.
/// </summary>
using EduFairOS.Models;
using EduFairOS.Layers.Application.Services;
using EduFairOS.Layers.Infrastructure.Data;
using EduFairOS.Layers.Infrastructure.Interfaces;

/// <summary>
/// Crea el constructor de la aplicación web.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Agrega servicios de controladores para manejar solicitudes HTTP.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Agrega servicios para explorar y generar documentación de API con Swagger.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Registra repositorios en el contenedor de dependencias.
/// Cada repositorio implementa IRepositorio para las entidades correspondientes.
/// </summary>
builder.Services.AddScoped<IRepositorio<Evento>, RepositorioEvento>();
builder.Services.AddScoped<IRepositorio<Participante>, RepositorioParticipante>();
builder.Services.AddScoped<IRepositorio<Stand>, RepositorioStand>();
builder.Services.AddScoped<IRepositorio<Actividad>, RepositorioActividad>();

/// <summary>
/// Registra servicios de aplicación en el contenedor de dependencias.
/// Estos servicios contienen la lógica de negocio.
/// </summary>
builder.Services.AddScoped<ServicioEvento>();
builder.Services.AddScoped<ServicioParticipante>();
builder.Services.AddScoped<ServicioStand>();
builder.Services.AddScoped<ServicioActividad>();

/// <summary>
/// Construye la aplicación web con la configuración definida.
/// </summary>
var app = builder.Build();

/// <summary>
/// Configura el pipeline de middleware para el entorno de desarrollo.
/// Incluye Swagger para documentación de API.
/// </summary>
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

/// <summary>
/// Fuerza el uso de HTTPS para todas las solicitudes.
/// </summary>
app.UseHttpsRedirection();

/// <summary>
/// Habilita la autorización para las rutas.
/// </summary>
app.UseAuthorization();

/// <summary>
/// Mapea los controladores a las rutas de la aplicación.
/// </summary>
app.MapControllers();

/// <summary>
/// Ejecuta la aplicación.
/// </summary>
app.Run();
```

## Instalación y Configuración
1. Clona el repositorio.
2. Asegúrate de tener SQL Server instalado y ejecuta el script `CrearBD_EduFairOS.sql` para crear la base de datos.
3. Actualiza la cadena de conexión en `ConexionBD.cs` si es necesario.
4. Restaura los paquetes NuGet: `dotnet restore`.
5. Ejecuta la aplicación: `dotnet run`.

## Uso
La aplicación expone una API REST documentada con Swagger. Accede a `/swagger` para ver la documentación interactiva.

## Contribución
Para contribuir, sigue los estándares de codificación y agrega comentarios XML a cualquier nuevo código.

## Licencia
Este proyecto es propiedad de Ashley Esmirna Feliz Rodríguez.