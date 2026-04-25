# EduFairOS - Documento de Entrega

## 1. Documento de requerimientos base

### Requisitos de negocio
- Gestionar ferias escolares.
- Registrar y administrar eventos, stands, actividades y participantes.
- Mantener datos persistentes en SQL Server.
- Ofrecer una API REST para la capa de presentación.
- Separar responsabilidades en capas (Presentación, Aplicación, Dominio, Infraestructura).
- Validar la información en la capa de negocio.
- Generar documentación y explicación de diseño.

### Inspiración
El proyecto se inspira en sistemas reales de gestión de ferias escolares donde se necesitan:
- eventos con fechas y lugares,
- stands con responsables y categorías,
- actividades asociadas a eventos,
- participantes provenientes de instituciones educativas.

## 2. Idea del Proyecto
EduFairOS es una aplicación para administrar una feria escolar. Permite:
- crear, leer, actualizar y eliminar eventos,
- gestionar stands dentro de cada evento,
- registrar participantes y asignarlos a actividades,
- mantener la información en una base de datos relacional.

## 3. Diseño Preliminar
El diseño sigue una arquitectura en capas:
- `Layers/Presentacion`: controladores API REST.
- `Layers/Application`: servicios de lógica de negocio y contratos.
- `Layers/Domain`: entidades de dominio y clases abstractas.
- `Layers/Infrastructure`: acceso a datos y repositorios.

### Diagrama de clases
Ver `docs/DIAGRAMA_CLASES.md` para el diagrama de entidades.

## 4. Implementación Inicial
La implementación inicial incluyó:
- la creación de las entidades principales en `Layers/Domain/Entities`,
- la definición de los contratos de repositorio en `Layers/Infrastructure/Interfaces/IRepositorio.cs`,
- el desarrollo de repositorios con acceso a SQL Server en `Layers/Infrastructure/Data`,
- la construcción de servicios de negocio en `Layers/Application/Services`,
- la exposición de endpoints en `Layers/Presentacion/Controllers`.

## 5. Clases Normales
Se utilizan clases normales para representar entidades del dominio:
- `Stand`
- `Evento`
- `Actividad`
- `Participante`
- `Asignacion`
- `EntidadBase`

Estas clases contienen atributos y métodos que representan la información y las reglas del sistema.

## 6. Constructores
Se implementaron constructores normales y sobrecargados en las entidades:
- `Persona(string nombre, int edad, string telefono, string correo)`
- `Persona()` como constructor por defecto

También los servicios reciben repositorios mediante constructores inyectados desde DI.

## 7. Clases Abstractas
Se utilizan clases abstractas para modelar el dominio:
- `EntidadBase`: base para todas las entidades con Id, fechas y estado.
- `Persona`: clase abstracta que hereda de `EntidadBase` y define propiedades comunes.

## 8. Sobrecargas
Ejemplos de sobrecarga en el proyecto:
- `Persona()` y `Persona(string nombre, int edad, string telefono, string correo)`
- `ServicioStand.ObtenerStand(int id)` y `ServicioStand.ObtenerStand(string nombre)`
- `ServicioEvento.ObtenerEvento(int id)` y `ServicioEvento.ObtenerEvento(string nombre)`

## 9. Arquitectura Distribuida
El proyecto está diseñado con capas separadas y un API REST que puede ser consumido desde clientes remotos.

### ¿Por qué es distribuido?
- La capa de presentación (API) está separada de la capa de infraestructura (datos).
- La aplicación utiliza inyección de dependencias para desacoplar servicios y repositorios.
- El sistema puede desplegarse como un servicio web accesible por otros clientes o un frontend.

## 10. Presentación
### Enlaces a entregar
- Documento de requerimientos base: `docs/ENTREGA_EDUFAIROS.md`
- Presentación de la necesidad y carta de venta: (agregar enlace aquí)
- Repositorio público: (agregar enlace aquí)
- Video explicativo: (agregar enlace aquí)

> Nota: completa los enlaces reales antes de entregar para cumplir con todos los requisitos de entrega.
