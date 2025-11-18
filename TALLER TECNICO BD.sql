-- =============================================
-- SISTEMA DE TALLER DE SERVICIO TÉCNICO
-- SCRIPT COMPLETO DE BASE DE DATOS (CORREGIDO)
-- =============================================

USE master;
GO

-- Eliminar base de datos si existe
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'TallerTecnicoDB')
BEGIN
    ALTER DATABASE TallerTecnicoDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TallerTecnicoDB;
END
GO

-- Crear base de datos
CREATE DATABASE TallerTecnicoDB;
GO

USE TallerTecnicoDB;
GO

-- =============================================
-- TABLA: Usuarios
-- =============================================
CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario NVARCHAR(50) UNIQUE NOT NULL,
    Contrasena NVARCHAR(255) NOT NULL,
    NombreCompleto NVARCHAR(100) NOT NULL,
    Rol NVARCHAR(20) NOT NULL CHECK (Rol IN ('Administrador', 'Tecnico')),
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

-- =============================================
-- TABLA: Clientes
-- =============================================
CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY IDENTITY(1,1),
    DNI NVARCHAR(20) UNIQUE NOT NULL,
    NombreCompleto NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100),
    Direccion NVARCHAR(200),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

-- =============================================
-- TABLA: Equipos
-- =============================================
CREATE TABLE Equipos (
    EquipoID INT PRIMARY KEY IDENTITY(1,1),
    ClienteID INT NOT NULL,
    TipoEquipo NVARCHAR(50) NOT NULL,
    Marca NVARCHAR(50) NOT NULL,
    Modelo NVARCHAR(50) NOT NULL,
    NumeroSerie NVARCHAR(50),
    Descripcion NVARCHAR(500),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Equipos_Clientes FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);
GO

-- =============================================
-- TABLA: Citas
-- =============================================
CREATE TABLE Citas (
    CitaID INT PRIMARY KEY IDENTITY(1,1),
    ClienteID INT NOT NULL,
    EquipoID INT NOT NULL,
    FechaCita DATETIME NOT NULL,
    HoraCita TIME NOT NULL,
    MotivoConsulta NVARCHAR(500) NOT NULL,
    Estado NVARCHAR(20) DEFAULT 'Pendiente' 
        CHECK (Estado IN ('Pendiente', 'En Proceso', 'Atendida', 'Cancelada')),
    TecnicoAsignado INT,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Citas_Clientes FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID),
    CONSTRAINT FK_Citas_Equipos FOREIGN KEY (EquipoID) REFERENCES Equipos(EquipoID),
    CONSTRAINT FK_Citas_Usuarios FOREIGN KEY (TecnicoAsignado) REFERENCES Usuarios(UsuarioID)
);
GO

-- =============================================
-- TABLA: Servicios
-- =============================================
CREATE TABLE Servicios (
    ServicioID INT PRIMARY KEY IDENTITY(1,1),
    ClienteID INT NOT NULL,
    EquipoID INT NOT NULL,
    CitaID INT NULL,
    TecnicoAsignado INT NOT NULL,
    FechaIngreso DATETIME DEFAULT GETDATE(),
    FechaEntregaEstimada DATETIME NULL,
    FechaEntregaReal DATETIME NULL,
    Diagnostico NVARCHAR(1000) NULL,
    SolucionAplicada NVARCHAR(1000) NULL,
    Estado NVARCHAR(20) DEFAULT 'Recibido' 
        CHECK (Estado IN ('Recibido', 'En Diagnostico', 'En Reparacion', 'Reparado', 'Entregado', 'Cancelado')),
    CostoManoObra DECIMAL(10,2) DEFAULT 0,
    CostoTotal DECIMAL(10,2) DEFAULT 0,
    Observaciones NVARCHAR(500) NULL,
    CONSTRAINT FK_Servicios_Clientes FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID),
    CONSTRAINT FK_Servicios_Equipos FOREIGN KEY (EquipoID) REFERENCES Equipos(EquipoID),
    CONSTRAINT FK_Servicios_Citas FOREIGN KEY (CitaID) REFERENCES Citas(CitaID),
    CONSTRAINT FK_Servicios_Usuarios FOREIGN KEY (TecnicoAsignado) REFERENCES Usuarios(UsuarioID)
);
GO

-- =============================================
-- TABLA: Repuestos
-- =============================================
CREATE TABLE Repuestos (
    RepuestoID INT PRIMARY KEY IDENTITY(1,1),
    Codigo NVARCHAR(50) UNIQUE NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    Categoria NVARCHAR(50) NULL,
    StockActual INT DEFAULT 0,
    StockMinimo INT DEFAULT 5,
    PrecioCompra DECIMAL(10,2) DEFAULT 0,
    PrecioVenta DECIMAL(10,2) DEFAULT 0,
    Proveedor NVARCHAR(100) NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

-- =============================================
-- TABLA: ServicioRepuestos
-- =============================================
CREATE TABLE ServicioRepuestos (
    ServicioRepuestoID INT PRIMARY KEY IDENTITY(1,1),
    ServicioID INT NOT NULL,
    RepuestoID INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_ServicioRepuestos_Servicios FOREIGN KEY (ServicioID) REFERENCES Servicios(ServicioID),
    CONSTRAINT FK_ServicioRepuestos_Repuestos FOREIGN KEY (RepuestoID) REFERENCES Repuestos(RepuestoID)
);
GO

-- =============================================
-- TABLA: Facturas
-- =============================================
CREATE TABLE Facturas (
    FacturaID INT PRIMARY KEY IDENTITY(1,1),
    NumeroFactura NVARCHAR(20) UNIQUE NOT NULL,
    ServicioID INT NOT NULL,
    ClienteID INT NOT NULL,
    FechaEmision DATETIME DEFAULT GETDATE(),
    Subtotal DECIMAL(10,2) NOT NULL,
    IGV DECIMAL(10,2) NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    FormaPago NVARCHAR(20) DEFAULT 'Efectivo'
        CHECK (FormaPago IN ('Efectivo', 'Tarjeta', 'Transferencia', 'Yape', 'Plin')),
    Estado NVARCHAR(20) DEFAULT 'Pendiente'
        CHECK (Estado IN ('Pendiente', 'Pagada', 'Anulada')),
    UsuarioRegistro INT NOT NULL,
    CONSTRAINT FK_Facturas_Servicios FOREIGN KEY (ServicioID) REFERENCES Servicios(ServicioID),
    CONSTRAINT FK_Facturas_Clientes FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID),
    CONSTRAINT FK_Facturas_Usuarios FOREIGN KEY (UsuarioRegistro) REFERENCES Usuarios(UsuarioID)
);
GO

-- =============================================
-- INSERTAR DATOS INICIALES
-- =============================================

INSERT INTO Usuarios (NombreUsuario, Contrasena, NombreCompleto, Rol, Email, Telefono)
VALUES 
('admin', 'admin123', 'Administrador del Sistema', 'Administrador', 'admin@taller.com', '999888777'),
('tecnico1', 'tecnico123', 'Juan Carlos Pérez', 'Tecnico', 'jperez@taller.com', '987654321');
GO

-- Clientes
INSERT INTO Clientes (DNI, NombreCompleto, Telefono, Email, Direccion)
VALUES 
('72345678', 'María González', '987456321', 'maria@email.com', 'Av. Principal 123'),
('71234567', 'Carlos Méndez', '965874123', 'carlos@email.com', 'Jr. Pinos 456'),
('70123456', 'Ana Vargas', '954789632', 'ana@email.com', 'Calle Lima 789');
GO

-- Equipos
INSERT INTO Equipos (ClienteID, TipoEquipo, Marca, Modelo, NumeroSerie, Descripcion)
VALUES 
(1, 'Laptop', 'HP', 'Pavilion 15', 'HP123456', 'Laptop 8GB RAM'),
(2, 'PC', 'Dell', 'Optiplex 7090', 'DELL9876', 'PC Intel i5'),
(3, 'Impresora', 'Canon', 'G3110', 'CAN4567', 'Impresora multifuncional');
GO

-- Repuestos
INSERT INTO Repuestos (Codigo, Nombre, Categoria, StockActual, StockMinimo, PrecioCompra, PrecioVenta, Proveedor)
VALUES 
('MEM001', 'Memoria RAM 8GB', 'Hardware', 15, 5, 120, 180, 'TechPeru'),
('HDD001', 'Disco Duro 1TB', 'Hardware', 10, 3, 150, 220, 'CompuParts'),
('TEC001', 'Teclado USB', 'Accesorios', 25, 10, 25, 45, 'OfficeMax');
GO

DECLARE @ServicioID INT;

-- Servicio (FECHAS en formato ISO seguro)
INSERT INTO Servicios (
    ClienteID, EquipoID, CitaID, TecnicoAsignado,
    FechaIngreso, FechaEntregaEstimada,
    Diagnostico, Estado, CostoManoObra, CostoTotal
)
VALUES 
(3, 3, 3, 2, '2025-11-17 09:30:00', '2025-11-18 18:00:00',
 'Cabezal obstruido', 'En Reparacion', 50.00, 95.00);

-- Obtener el ID generado
SET @ServicioID = SCOPE_IDENTITY();


-- Repuestos usados
INSERT INTO ServicioRepuestos (ServicioID, RepuestoID, Cantidad, PrecioUnitario, Subtotal)
VALUES (@ServicioID, 3, 1, 45.00, 45.00);
GO


-- Si necesitas actualizar costos después, ya lo puedes hacer con UPDATE:
-- UPDATE Servicios SET CostoTotal = CostoManoObra + <otra columna si aplica> WHERE ServicioID = 1;
-- (ya hemos insertado CostoTotal en el INSERT anterior)
-- GO

-- =============================================
-- VISTAS
-- =============================================
CREATE VIEW vw_ServiciosCompletos AS
SELECT 
    s.ServicioID, 
    s.FechaIngreso, 
    s.FechaEntregaEstimada, 
    s.FechaEntregaReal,
    s.Estado,
    c.NombreCompleto AS Cliente, 
    c.Telefono AS TelefonoCliente,
    e.TipoEquipo, 
    e.Marca, 
    e.Modelo,
    u.NombreCompleto AS Tecnico, 
    s.Diagnostico, 
    s.SolucionAplicada,
    s.CostoManoObra,
    s.CostoTotal,
    s.Observaciones
FROM Servicios s
INNER JOIN Clientes c ON s.ClienteID = c.ClienteID
INNER JOIN Equipos e ON s.EquipoID = e.EquipoID
INNER JOIN Usuarios u ON s.TecnicoAsignado = u.UsuarioID;
GO

-- =============================================
-- INDICES RECOMENDADOS
-- (opcionales, mejoran rendimiento en búsquedas)
-- =============================================
CREATE INDEX IX_Clientes_DNI ON Clientes(DNI);
CREATE INDEX IX_Equipos_ClienteID ON Equipos(ClienteID);
CREATE INDEX IX_Servicios_Estado ON Servicios(Estado);
CREATE INDEX IX_Servicios_TecnicoAsignado ON Servicios(TecnicoAsignado);
CREATE INDEX IX_Citas_FechaCita ON Citas(FechaCita);
CREATE INDEX IX_Repuestos_Codigo ON Repuestos(Codigo);
GO

-- =============================================
-- FIN DEL SCRIPT
-- =============================================
PRINT 'Base de datos TallerTecnicoDB creada correctamente y sin errores';
PRINT 'Usuario Admin: admin / admin123';
PRINT 'Usuario Técnico: tecnico1 / tecnico123';
GO
