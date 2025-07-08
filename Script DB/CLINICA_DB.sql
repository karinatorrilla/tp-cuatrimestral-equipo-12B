USE [master]
GO
/****** Object:  Database [CLINICA_DB]    Script Date: 8/7/2025 00:27:33 ******/
CREATE DATABASE [CLINICA_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CLINICA_DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CLINICA_DB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CLINICA_DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CLINICA_DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CLINICA_DB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CLINICA_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CLINICA_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CLINICA_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CLINICA_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CLINICA_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CLINICA_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CLINICA_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CLINICA_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CLINICA_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CLINICA_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CLINICA_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CLINICA_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CLINICA_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CLINICA_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CLINICA_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CLINICA_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CLINICA_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CLINICA_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CLINICA_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CLINICA_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CLINICA_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CLINICA_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CLINICA_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CLINICA_DB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CLINICA_DB] SET  MULTI_USER 
GO
ALTER DATABASE [CLINICA_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CLINICA_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CLINICA_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CLINICA_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CLINICA_DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CLINICA_DB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CLINICA_DB] SET QUERY_STORE = ON
GO
ALTER DATABASE [CLINICA_DB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CLINICA_DB]
GO
/****** Object:  Table [dbo].[ESPECIALIDADES]    Script Date: 8/7/2025 00:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ESPECIALIDADES](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[Habilitado] [bit] NOT NULL,
 CONSTRAINT [PK_ESPECIALIDADES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MEDICOS]    Script Date: 8/7/2025 00:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MEDICOS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Matricula] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Apellido] [varchar](100) NOT NULL,
	[Documento] [int] NULL,
	[Email] [varchar](100) NULL,
	[Telefono] [varchar](50) NULL,
	[Nacionalidad] [varchar](50) NULL,
	[Provincia] [varchar](30) NULL,
	[Localidad] [varchar](30) NULL,
	[Calle] [varchar](150) NULL,
	[Altura] [int] NULL,
	[CodPostal] [varchar](10) NULL,
	[Depto] [varchar](50) NULL,
	[FechaNacimiento] [date] NULL,
	[EspecialidadesIDs] [varchar](max) NULL,
	[IDTurnoTrabajo] [int] NULL,
	[DiasDisponiblesIDs] [varchar](max) NULL,
	[HoraInicioBloque] [time](0) NULL,
	[HoraFinBloque] [time](0) NULL,
	[Habilitado] [bit] NULL,
 CONSTRAINT [PK_MEDICOS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MEDICOxDISPONIBILIDADHORARIA]    Script Date: 8/7/2025 00:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MEDICOxDISPONIBILIDADHORARIA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MedicoId] [int] NOT NULL,
	[DiaDeLaSemana] [int] NOT NULL,
	[HoraInicioBloque] [time](0) NOT NULL,
	[HoraFinBloque] [time](0) NOT NULL,
	[Habilitado] [bit] NOT NULL,
 CONSTRAINT [PK_MEDICOxDISPONIBILIDADHORARIA] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MEDICOxESPECIALIDAD]    Script Date: 8/7/2025 00:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MEDICOxESPECIALIDAD](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MedicoId] [int] NOT NULL,
	[EspecialidadId] [int] NOT NULL,
	[Habilitado] [bit] NOT NULL,
 CONSTRAINT [PK_MEDICOxESPECIALIDAD_New] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OBRASOCIAL]    Script Date: 8/7/2025 00:27:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OBRASOCIAL](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NULL,
	[Habilitado] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PACIENTES]    Script Date: 8/7/2025 00:27:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PACIENTES](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NULL,
	[Apellido] [varchar](100) NULL,
	[Documento] [int] NULL,
	[Email] [varchar](100) NULL,
	[Telefono] [varchar](50) NULL,
	[Nacionalidad] [varchar](50) NULL,
	[ProvinciaId] [varchar](30) NULL,
	[LocalidadId] [varchar](30) NULL,
	[Calle] [varchar](150) NULL,
	[Altura] [int] NULL,
	[CodPostal] [varchar](10) NULL,
	[Depto] [varchar](50) NULL,
	[FechaNacimiento] [date] NULL,
	[ObraSocialId] [int] NULL,
	[Observaciones] [varchar](max) NULL,
	[Habilitado] [bit] NULL,
 CONSTRAINT [PK_PACIENTES] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TURNOS]    Script Date: 8/7/2025 00:27:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TURNOS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdMedico] [int] NOT NULL,
	[IdPaciente] [int] NOT NULL,
	[IdEspecialidad] [nchar](10) NOT NULL,
	[Fecha] [date] NOT NULL,
	[Hora] [smallint] NOT NULL,
	[Observaciones] [varchar](max) NULL,
	[Estado] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USUARIOS]    Script Date: 8/7/2025 00:27:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIOS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](50) NULL,
	[Pass] [varchar](50) NULL,
	[TipoUser] [int] NULL,
	[IDMedico] [int] NULL,
 CONSTRAINT [PK_USUARIOS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ESPECIALIDADES] ON 
GO
INSERT [dbo].[ESPECIALIDADES] ([ID], [Descripcion], [Habilitado]) VALUES (1, N'Dentista', 1)
GO
INSERT [dbo].[ESPECIALIDADES] ([ID], [Descripcion], [Habilitado]) VALUES (3, N'Pediatra', 1)
GO
INSERT [dbo].[ESPECIALIDADES] ([ID], [Descripcion], [Habilitado]) VALUES (4, N'Oftalmologo', 1)
GO
INSERT [dbo].[ESPECIALIDADES] ([ID], [Descripcion], [Habilitado]) VALUES (5, N'Cardiología', 1)
GO
INSERT [dbo].[ESPECIALIDADES] ([ID], [Descripcion], [Habilitado]) VALUES (6, N'Cirujano', 1)
GO
INSERT [dbo].[ESPECIALIDADES] ([ID], [Descripcion], [Habilitado]) VALUES (7, N'NuevaEspecialidad', 1)
GO
INSERT [dbo].[ESPECIALIDADES] ([ID], [Descripcion], [Habilitado]) VALUES (8, N'NuevaEspecialidad', 0)
GO
INSERT [dbo].[ESPECIALIDADES] ([ID], [Descripcion], [Habilitado]) VALUES (9, N'ssdf', 0)
GO
SET IDENTITY_INSERT [dbo].[ESPECIALIDADES] OFF
GO
SET IDENTITY_INSERT [dbo].[MEDICOS] ON 
GO
INSERT [dbo].[MEDICOS] ([Id], [Matricula], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [Provincia], [Localidad], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [EspecialidadesIDs], [IDTurnoTrabajo], [DiasDisponiblesIDs], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (10, 1234, N'Lucía', N'González', 30555666, N'lucia.gonzalez@clinica.com', N'1122334455', N'Argentina', N'Buenos Aires', N'La Plata', N'Calle 12', 456, N'1900', N'3B', CAST(N'1980-06-15' AS Date), NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[MEDICOS] ([Id], [Matricula], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [Provincia], [Localidad], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [EspecialidadesIDs], [IDTurnoTrabajo], [DiasDisponiblesIDs], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (11, 5678, N'Martín', N'Pérez', 28999111, N'martin.perez@clinica.com', N'1133445566', N'Argentina', N'Córdoba', N'Córdoba', N'Av. Colón', 1023, N'5000', NULL, CAST(N'1975-09-22' AS Date), NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[MEDICOS] ([Id], [Matricula], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [Provincia], [Localidad], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [EspecialidadesIDs], [IDTurnoTrabajo], [DiasDisponiblesIDs], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (12, 9876, N'Ana', N'Martínez', 31111222, N'ana.martinez@clinica.com', N'1144556677', N'Argentina', N'Mendoza', N'Godoy Cruz', N'San Martín', 850, N'5501', N'1A', CAST(N'1988-03-10' AS Date), NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[MEDICOS] ([Id], [Matricula], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [Provincia], [Localidad], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [EspecialidadesIDs], [IDTurnoTrabajo], [DiasDisponiblesIDs], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (13, 4321, N'Carlos', N'Suárez', 27777888, N'carlos.suarez@clinica.com', N'1177889900', N'Argentina', N'Santa Fe', N'Rosario', N'Mitre', 321, N'2000', NULL, CAST(N'1982-11-30' AS Date), NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[MEDICOS] ([Id], [Matricula], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [Provincia], [Localidad], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [EspecialidadesIDs], [IDTurnoTrabajo], [DiasDisponiblesIDs], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (14, 6543, N'Soledad', N'Ramírez', 32222333, N'soledad.ramirez@clinica.com', N'1199001122', N'Argentina', N'Salta', N'Salta', N'Av. Belgrano', 777, N'4400', N'2C', CAST(N'1990-01-25' AS Date), NULL, NULL, NULL, NULL, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[MEDICOS] OFF
GO
SET IDENTITY_INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ON 
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (6, 10, 1, CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (7, 10, 3, CAST(N'14:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (8, 10, 5, CAST(N'08:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (9, 11, 2, CAST(N'10:00:00' AS Time), CAST(N'15:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (10, 11, 4, CAST(N'10:00:00' AS Time), CAST(N'15:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (11, 12, 1, CAST(N'08:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (12, 12, 2, CAST(N'08:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (13, 12, 3, CAST(N'08:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (14, 12, 4, CAST(N'08:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (15, 13, 5, CAST(N'08:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (16, 12, 5, CAST(N'08:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (17, 14, 3, CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (18, 14, 4, CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] ([Id], [MedicoId], [DiaDeLaSemana], [HoraInicioBloque], [HoraFinBloque], [Habilitado]) VALUES (19, 14, 5, CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
GO
SET IDENTITY_INSERT [dbo].[MEDICOxDISPONIBILIDADHORARIA] OFF
GO
SET IDENTITY_INSERT [dbo].[MEDICOxESPECIALIDAD] ON 
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (61, 10, 1, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (62, 11, 1, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (63, 11, 3, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (64, 12, 5, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (65, 13, 4, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (66, 13, 6, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (67, 14, 1, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (68, 14, 3, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (69, 14, 4, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (70, 14, 5, 1)
GO
INSERT [dbo].[MEDICOxESPECIALIDAD] ([Id], [MedicoId], [EspecialidadId], [Habilitado]) VALUES (71, 14, 6, 1)
GO
SET IDENTITY_INSERT [dbo].[MEDICOxESPECIALIDAD] OFF
GO
SET IDENTITY_INSERT [dbo].[OBRASOCIAL] ON 
GO
INSERT [dbo].[OBRASOCIAL] ([ID], [Descripcion], [Habilitado]) VALUES (1, N'Particular', 1)
GO
INSERT [dbo].[OBRASOCIAL] ([ID], [Descripcion], [Habilitado]) VALUES (3, N'OSDE', 1)
GO
INSERT [dbo].[OBRASOCIAL] ([ID], [Descripcion], [Habilitado]) VALUES (4, N'Galeno', 1)
GO
INSERT [dbo].[OBRASOCIAL] ([ID], [Descripcion], [Habilitado]) VALUES (5, N'Swiss', 1)
GO
INSERT [dbo].[OBRASOCIAL] ([ID], [Descripcion], [Habilitado]) VALUES (6, N'Swiss', 1)
GO
SET IDENTITY_INSERT [dbo].[OBRASOCIAL] OFF
GO
SET IDENTITY_INSERT [dbo].[PACIENTES] ON 
GO
INSERT [dbo].[PACIENTES] ([Id], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [ProvinciaId], [LocalidadId], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [ObraSocialId], [Observaciones], [Habilitado]) VALUES (3, N'Lucas', N'Alegre', 38765432, N'lucas.alegre@example.com', N'1167894321', N'Argentina', N'02', N'0021', N'Av. Siempre Viva', 742, N'1407', N'3B', CAST(N'1990-08-15' AS Date), 3, N'Alergia al ibuprofeno', 1)
GO
INSERT [dbo].[PACIENTES] ([Id], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [ProvinciaId], [LocalidadId], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [ObraSocialId], [Observaciones], [Habilitado]) VALUES (4, N'Sabrina', N'Gómez', 40567891, N'sabrina.gomez@example.com', N'1134567890', N'Argentina', N'06', N'0602', N'Calle Falsa', 1234, N'5000', NULL, CAST(N'1995-02-10' AS Date), 1, N'Sin observaciones', 1)
GO
INSERT [dbo].[PACIENTES] ([Id], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [ProvinciaId], [LocalidadId], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [ObraSocialId], [Observaciones], [Habilitado]) VALUES (5, N'Joaquín', N'Pérez', 39654321, N'joaquin.perez@example.com', N'1156784321', N'Argelina', N'50', N'50098020', N'Las Heras', 455, N'8300', N'1A', CAST(N'1988-11-30' AS Date), 4, N'Hipertenso controlado', 1)
GO
INSERT [dbo].[PACIENTES] ([Id], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [ProvinciaId], [LocalidadId], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [ObraSocialId], [Observaciones], [Habilitado]) VALUES (6, N'Camila', N'Rodríguez', 42345678, N'camila.rodriguez@example.com', N'1145678901', N'Argentina', N'01', N'0104', N'San Martín', 999, N'1000', NULL, CAST(N'2001-06-22' AS Date), 4, NULL, 1)
GO
INSERT [dbo].[PACIENTES] ([Id], [Nombre], [Apellido], [Documento], [Email], [Telefono], [Nacionalidad], [ProvinciaId], [LocalidadId], [Calle], [Altura], [CodPostal], [Depto], [FechaNacimiento], [ObraSocialId], [Observaciones], [Habilitado]) VALUES (7, N'Bruno', N'Fernández', 38456789, N'bruno.fernandez@example.com', N'1122334455', N'Saudí', N'30', N'30098110', N'Mitre', 654, N'7600', N'2C', CAST(N'1992-04-03' AS Date), 3, N'Celíaco', 1)
GO
SET IDENTITY_INSERT [dbo].[PACIENTES] OFF
GO
SET IDENTITY_INSERT [dbo].[TURNOS] ON 
GO
INSERT [dbo].[TURNOS] ([Id], [IdMedico], [IdPaciente], [IdEspecialidad], [Fecha], [Hora], [Observaciones], [Estado]) VALUES (1, 10, 3, N'1         ', CAST(N'2025-10-10' AS Date), 15, N'Observacion numero uno del medico', 1)
GO
SET IDENTITY_INSERT [dbo].[TURNOS] OFF
GO
SET IDENTITY_INSERT [dbo].[USUARIOS] ON 
GO
INSERT [dbo].[USUARIOS] ([Id], [Usuario], [Pass], [TipoUser], [IDMedico]) VALUES (1, N'admin', N'admin', 1, NULL)
GO
INSERT [dbo].[USUARIOS] ([Id], [Usuario], [Pass], [TipoUser], [IDMedico]) VALUES (2, N'recep', N'recep', 2, NULL)
GO
INSERT [dbo].[USUARIOS] ([Id], [Usuario], [Pass], [TipoUser], [IDMedico]) VALUES (3, N'med', N'med', 3, NULL)
GO
INSERT [dbo].[USUARIOS] ([Id], [Usuario], [Pass], [TipoUser], [IDMedico]) VALUES (4, N'med10', N'med10', 3, 10)
GO
INSERT [dbo].[USUARIOS] ([Id], [Usuario], [Pass], [TipoUser], [IDMedico]) VALUES (5, N'med11', N'med11', 3, 11)
GO
INSERT [dbo].[USUARIOS] ([Id], [Usuario], [Pass], [TipoUser], [IDMedico]) VALUES (6, N'med12', N'med12', 3, 12)
GO
INSERT [dbo].[USUARIOS] ([Id], [Usuario], [Pass], [TipoUser], [IDMedico]) VALUES (7, N'med13', N'med13', 3, 13)
GO
INSERT [dbo].[USUARIOS] ([Id], [Usuario], [Pass], [TipoUser], [IDMedico]) VALUES (8, N'med14', N'med14', 3, 14)
GO
SET IDENTITY_INSERT [dbo].[USUARIOS] OFF
GO
/****** Object:  Index [UQ__MEDICOS__0FB9FB4FACB9FFCB]    Script Date: 8/7/2025 00:27:34 ******/
ALTER TABLE [dbo].[MEDICOS] ADD UNIQUE NONCLUSTERED 
(
	[Matricula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__MEDICOS__0FB9FB4FD60FF0AE]    Script Date: 8/7/2025 00:27:34 ******/
ALTER TABLE [dbo].[MEDICOS] ADD UNIQUE NONCLUSTERED 
(
	[Matricula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_MEDICOS_Documento]    Script Date: 8/7/2025 00:27:34 ******/
ALTER TABLE [dbo].[MEDICOS] ADD  CONSTRAINT [UQ_MEDICOS_Documento] UNIQUE NONCLUSTERED 
(
	[Documento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ESPECIALIDADES] ADD  CONSTRAINT [DF_ESPECIALIDADES_Habilitado]  DEFAULT ((1)) FOR [Habilitado]
GO
ALTER TABLE [dbo].[MEDICOS] ADD  DEFAULT ((1)) FOR [Habilitado]
GO
ALTER TABLE [dbo].[MEDICOxDISPONIBILIDADHORARIA] ADD  CONSTRAINT [DF_MEDICOxDISPONIBILIDADHORARIA_Habilitado]  DEFAULT ((1)) FOR [Habilitado]
GO
ALTER TABLE [dbo].[MEDICOxESPECIALIDAD] ADD  DEFAULT ((1)) FOR [Habilitado]
GO
ALTER TABLE [dbo].[OBRASOCIAL] ADD  CONSTRAINT [DF_OBRASOCIAL_Habilitado]  DEFAULT ((1)) FOR [Habilitado]
GO
ALTER TABLE [dbo].[PACIENTES] ADD  DEFAULT ((1)) FOR [Habilitado]
GO
ALTER TABLE [dbo].[TURNOS] ADD  CONSTRAINT [DF_TURNOS_Observaciones]  DEFAULT ('No hay observaciones') FOR [Observaciones]
GO
ALTER TABLE [dbo].[TURNOS] ADD  CONSTRAINT [DF_TURNOS_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[MEDICOxDISPONIBILIDADHORARIA]  WITH CHECK ADD  CONSTRAINT [FK_MEDICOxDISPONIBILIDADHORARIA_Medicos] FOREIGN KEY([MedicoId])
REFERENCES [dbo].[MEDICOS] ([Id])
GO
ALTER TABLE [dbo].[MEDICOxDISPONIBILIDADHORARIA] CHECK CONSTRAINT [FK_MEDICOxDISPONIBILIDADHORARIA_Medicos]
GO
USE [master]
GO
ALTER DATABASE [CLINICA_DB] SET  READ_WRITE 
GO
