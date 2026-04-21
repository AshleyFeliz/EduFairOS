-- =====================================================
-- SCRIPT DE CREACIÓN DE BASE DE DATOS
-- Sistema de Organización de Ferias Escolares (EduFair OS)
-- =====================================================

-- Crear base de datos
CREATE DATABASE EduFairOS;
GO

-- Usar la base de datos
USE EduFairOS;
GO

-- =====================================================
-- TABLA: Eventos
-- =====================================================
CREATE TABLE Eventos (
    IdEvento INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(200) NOT NULL,
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NOT NULL,
    Lugar NVARCHAR(300) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Planificación',
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);

-- =====================================================
-- TABLA: Stands
-- =====================================================
CREATE TABLE Stands (
    IdStand INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(200) NOT NULL,
    Ubicacion NVARCHAR(300) NOT NULL,
    Categoria NVARCHAR(100) NOT NULL,
    EncargadoId INT,
    IdEvento INT NOT NULL,
    Descripcion NVARCHAR(MAX),
    Ocupado BIT DEFAULT 0,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    CONSTRAINT FK_Stand_Evento FOREIGN KEY (IdEvento) REFERENCES Eventos(IdEvento)
);

-- =====================================================
-- TABLA: Participantes
-- =====================================================
CREATE TABLE Participantes (
    IdParticipante INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(200) NOT NULL,
    Edad INT NOT NULL,
    Grado NVARCHAR(50) NOT NULL,
    Institucion NVARCHAR(300) NOT NULL,
    Telefono NVARCHAR(20),
    Correo NVARCHAR(100),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);

-- =====================================================
-- TABLA: Actividades
-- =====================================================
CREATE TABLE Actividades (
    IdActividad INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    HoraInicio DATETIME NOT NULL,
    HoraFin DATETIME NOT NULL,
    IdStand INT NOT NULL,
    MonitorId INT,
    CapacidadMaxima INT NOT NULL,
    ParticipantesActuales INT DEFAULT 0,
    Nivel NVARCHAR(50) DEFAULT 'Básico',
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    CONSTRAINT FK_Actividad_Stand FOREIGN KEY (IdStand) REFERENCES Stands(IdStand)
);

-- =====================================================
-- TABLA: Asignaciones
-- =====================================================
CREATE TABLE Asignaciones (
    IdAsignacion INT PRIMARY KEY IDENTITY(1,1),
    IdParticipante INT NOT NULL,
    IdActividad INT NOT NULL,
    FechaAsignacion DATETIME DEFAULT GETDATE(),
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Pendiente',
    Puntuacion DECIMAL(5,2) DEFAULT 0,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    CONSTRAINT FK_Asignacion_Participante FOREIGN KEY (IdParticipante) REFERENCES Participantes(IdParticipante),
    CONSTRAINT FK_Asignacion_Actividad FOREIGN KEY (IdActividad) REFERENCES Actividades(IdActividad)
);