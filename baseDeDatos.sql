-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         10.4.32-MariaDB - mariadb.org binary distribution
-- SO del servidor:              Win64
-- HeidiSQL Versión:             12.11.0.7065
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para bd_veterinarianogal
CREATE DATABASE IF NOT EXISTS `bd_veterinarianogal` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `bd_veterinarianogal`;

-- Volcando estructura para tabla bd_veterinarianogal.clientes
CREATE TABLE IF NOT EXISTS `clientes` (
  `Id_Cliente` int(11) NOT NULL AUTO_INCREMENT,
  `Dni` varchar(20) NOT NULL DEFAULT '0',
  `Apellido` varchar(50) NOT NULL DEFAULT '0',
  `Nombre` varchar(50) NOT NULL DEFAULT '0',
  `Direccion` varchar(50) NOT NULL DEFAULT '0',
  `Telefono` varchar(50) NOT NULL DEFAULT '0',
  `Email` varchar(50) NOT NULL DEFAULT '0',
  `FechaAlta` date NOT NULL,
  `Estado` tinyint(4) DEFAULT 0,
  PRIMARY KEY (`Id_Cliente`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.clientes: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.cobrocuotas
CREATE TABLE IF NOT EXISTS `cobrocuotas` (
  `IdCuota` int(11) NOT NULL AUTO_INCREMENT,
  `IdCobro` int(11) NOT NULL,
  `numeroCuota` int(11) NOT NULL,
  `fechaVencimiento` date NOT NULL,
  `montoCuota` decimal(20,2) NOT NULL,
  `interes` decimal(5,2) DEFAULT 0.00,
  `montoConInteres` decimal(20,2) NOT NULL,
  `fechaPago` date DEFAULT NULL,
  `montoPagado` decimal(20,2) DEFAULT 0.00,
  `saldoPendiente` decimal(20,2) NOT NULL,
  `estadoCuota` varchar(50) DEFAULT 'Pendiente',
  `estado` tinyint(4) DEFAULT 1,
  PRIMARY KEY (`IdCuota`),
  KEY `IdCobro` (`IdCobro`),
  CONSTRAINT `cobrocuotas_ibfk_1` FOREIGN KEY (`IdCobro`) REFERENCES `cobros` (`IdCobro`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.cobrocuotas: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.cobros
CREATE TABLE IF NOT EXISTS `cobros` (
  `IdCobro` int(11) NOT NULL AUTO_INCREMENT,
  `IdCliente` int(11) NOT NULL,
  `concepto` varchar(250) NOT NULL,
  `descripcion` varchar(250) DEFAULT NULL,
  `monto` decimal(20,2) NOT NULL,
  `tipoPrecio` varchar(20) DEFAULT 'minorista',
  `formaPago` varchar(50) NOT NULL,
  `estadoPago` varchar(50) DEFAULT 'Pendiente',
  `fecha` date NOT NULL,
  `observacion` varchar(250) DEFAULT NULL,
  `estado` tinyint(4) DEFAULT 1,
  PRIMARY KEY (`IdCobro`),
  KEY `IdCliente` (`IdCliente`),
  CONSTRAINT `cobros_ibfk_1` FOREIGN KEY (`IdCliente`) REFERENCES `clientes` (`Id_Cliente`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.cobros: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.cuentascorrientes
CREATE TABLE IF NOT EXISTS `cuentascorrientes` (
  `IdCuentasC` int(11) NOT NULL AUTO_INCREMENT,
  `IdClientes` int(11) NOT NULL,
  `IdVenta` int(11) NOT NULL,
  `fechaMovimiento` date NOT NULL,
  `tipoMovimiento` varchar(50) NOT NULL,
  `importe` decimal(20,6) NOT NULL,
  `concepto` varchar(100) NOT NULL,
  `saldoAnterior` decimal(20,6) NOT NULL,
  `saldoNuevo` decimal(20,6) NOT NULL,
  `fechaVencimiento` date NOT NULL,
  `estadoCuenta` varchar(50) NOT NULL,
  `comprobante` varchar(50) NOT NULL,
  `observacion` varchar(250) NOT NULL,
  `estado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`IdCuentasC`),
  KEY `IdClientes` (`IdClientes`),
  KEY `IdVenta` (`IdVenta`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.cuentascorrientes: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.cuotas
CREATE TABLE IF NOT EXISTS `cuotas` (
  `IdCuotas` int(11) NOT NULL AUTO_INCREMENT,
  `numeroCuota` int(11) NOT NULL,
  `fechaVencimiento` date NOT NULL,
  `fechaPago` date NOT NULL,
  `montoCuota` decimal(20,6) NOT NULL DEFAULT 0.000000,
  `montoPagodo` decimal(20,6) NOT NULL DEFAULT 0.000000,
  `saldoPendiente` decimal(20,6) NOT NULL DEFAULT 0.000000,
  `IdVenta` int(11) NOT NULL,
  `interesHora` decimal(20,6) NOT NULL DEFAULT 0.000000,
  `estadoCuota` varchar(50) NOT NULL DEFAULT '',
  `estado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`IdCuotas`),
  KEY `IdVenta` (`IdVenta`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.cuotas: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.especies
CREATE TABLE IF NOT EXISTS `especies` (
  `IdEspecies` int(11) NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(250) NOT NULL,
  `estado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`IdEspecies`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.especies: ~8 rows (aproximadamente)
INSERT INTO `especies` (`IdEspecies`, `descripcion`, `estado`) VALUES
	(1, 'Perro', 1),
	(2, 'Gato', 1),
	(3, 'Vaca', 1),
	(4, 'Caballo', 1),
	(5, 'Oveja', 1),
	(6, 'Cerdo', 1),
	(7, 'Conejo', 1),
	(8, 'Ave', 1);

-- Volcando estructura para tabla bd_veterinarianogal.historial
CREATE TABLE IF NOT EXISTS `historial` (
  `IdHistorial` int(11) NOT NULL AUTO_INCREMENT,
  `IdMascota` int(11) NOT NULL,
  `IdTurno` int(11) NOT NULL,
  `motivoConsulta` varchar(50) NOT NULL,
  `diagnostico` varchar(100) NOT NULL,
  `tartamiento` varchar(250) NOT NULL,
  `indicaciones` varchar(250) NOT NULL,
  `observaciones` varchar(250) NOT NULL,
  `fechaConsulta` date DEFAULT NULL,
  `proximoControl` date DEFAULT NULL,
  `estado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`IdHistorial`),
  KEY `IdTurno` (`IdTurno`),
  KEY `IdMascota` (`IdMascota`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.historial: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.mascota
CREATE TABLE IF NOT EXISTS `mascota` (
  `Id_Mascota` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) NOT NULL,
  `Id_Cliente` int(11) NOT NULL,
  `Id_Especie` int(11) NOT NULL,
  `Sexo` varchar(1) NOT NULL,
  `FechaNacimiento` date NOT NULL,
  `Estado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`Id_Mascota`),
  KEY `Id_Cliente` (`Id_Cliente`),
  KEY `Id_Especie` (`Id_Especie`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.mascota: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.productos
CREATE TABLE IF NOT EXISTS `productos` (
  `IdProductos` int(11) NOT NULL AUTO_INCREMENT,
  `codigo` varchar(250) DEFAULT NULL,
  `nombre` varchar(250) DEFAULT NULL,
  `stock` int(11) DEFAULT NULL,
  `p_minorista` decimal(20,6) DEFAULT NULL,
  `p_mayorista` decimal(20,6) DEFAULT NULL,
  `descripcion` varchar(250) DEFAULT NULL,
  `IdTipo` int(11) DEFAULT NULL,
  `IdProveedor` int(11) DEFAULT NULL,
  `estado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`IdProductos`),
  KEY `IdTipo` (`IdTipo`),
  KEY `IdProveedor` (`IdProveedor`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.productos: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.proveedores
CREATE TABLE IF NOT EXISTS `proveedores` (
  `IdProveedores` int(11) NOT NULL AUTO_INCREMENT,
  `razonSocial` varchar(50) DEFAULT NULL,
  `dni` varchar(50) DEFAULT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `fechaAlta` date DEFAULT NULL,
  `estado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`IdProveedores`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.proveedores: ~0 rows (aproximadamente)

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ActualizarCliente
DELIMITER //
CREATE PROCEDURE `sp_ActualizarCliente`(
	IN `p_id` INT,
	IN `p_nombre` VARCHAR(100),
	IN `p_telefono` VARCHAR(20)
)
BEGIN
    UPDATE clientes SET
        Nombre = p_nombre,
        Telefono = p_telefono
    WHERE Id_Cliente = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ActualizarMascota
DELIMITER //
CREATE PROCEDURE `sp_ActualizarMascota`(
	IN `p_id` INT,
	IN `p_nombre` VARCHAR(50),
	IN `p_id_cliente` INT,
	IN `p_id_especie` INT,
	IN `p_sexo` VARCHAR(1),
	IN `p_fecha_nac` DATE
)
BEGIN
    UPDATE mascota SET
        Nombre = p_nombre,
        Id_Cliente = p_id_cliente,
        Id_Especie = p_id_especie,
        Sexo = p_sexo,
        FechaNacimiento = p_fecha_nac
    WHERE Id_Mascota = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ActualizarTurno
DELIMITER //
CREATE PROCEDURE `sp_ActualizarTurno`(
	IN `p_id` INT,
	IN `p_fecha` DATE,
	IN `p_hora` TIME,
	IN `p_motivo` VARCHAR(50),
	IN `p_estado` VARCHAR(50)
)
BEGIN
    UPDATE turnos SET
        fecha = p_fecha,
        horaTurno = p_hora,
        motivo = p_motivo,
        estadoTurno = p_estado
    WHERE IdTurno = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ActualizarVenta
DELIMITER //
CREATE PROCEDURE `sp_ActualizarVenta`(
	IN `p_id` INT,
	IN `p_forma_pago` VARCHAR(50),
	IN `p_estado_pago` VARCHAR(50),
	IN `p_observacion` VARCHAR(50)
)
BEGIN
    UPDATE ventas SET
        formaPago = p_forma_pago,
        estadoPago = p_estado_pago,
        observacion = p_observacion
    WHERE IdVentas = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_AgregarDetalleVenta
DELIMITER //
CREATE PROCEDURE `sp_AgregarDetalleVenta`(
	IN `p_id_venta` INT,
	IN `p_id_producto` INT,
	IN `p_cantidad` DECIMAL(20,6),
	IN `p_precio` DECIMAL(20,6),
	IN `p_descuento` DECIMAL(20,6),
	IN `p_subtotal` DECIMAL(20,6)
)
BEGIN
    INSERT INTO ventadetalles (IdVentas, IdProductos, cantidad, precioUnitario, descuentoItem, subtotalItem)
    VALUES (p_id_venta, p_id_producto, p_cantidad, p_precio, p_descuento, p_subtotal);
    UPDATE productos SET stock = stock - p_cantidad WHERE IdProductos = p_id_producto;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_AlertasDeuda
DELIMITER //
CREATE PROCEDURE `sp_AlertasDeuda`()
BEGIN
    SELECT 
        c.Id_Cliente,
        c.Nombre,
        c.Apellido,
        c.Telefono,
        cc.IdCuentasC,
        cc.importe,
        cc.saldoNuevo,
        cc.fechaVencimiento AS vencimiento_cuenta,
        cu.IdCuotas,
        cu.numeroCuota,
        cu.montoCuota,
        cu.saldoPendiente,
        cu.fechaVencimiento AS vencimiento_cuota,
        cu.estadoCuota,
        CASE 
            WHEN cu.fechaVencimiento < CURDATE() THEN 'VENCIDA'
            WHEN cu.fechaVencimiento = CURDATE() THEN 'VENCE HOY'
            WHEN cu.fechaVencimiento <= DATE_ADD(CURDATE(), INTERVAL 7 DAY) THEN 'PROXIMA A VENCER'
            ELSE 'AL DIA'
        END AS alerta
    FROM cuentascorrientes cc
    JOIN clientes c ON cc.IdClientes = c.Id_Cliente
    JOIN cuotas cu ON cc.IdVenta = cu.IdVenta
    WHERE cu.estadoCuota != 'Pagada'
    ORDER BY cu.fechaVencimiento ASC;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_BuscarCliente
DELIMITER //
CREATE PROCEDURE `sp_BuscarCliente`(
	IN `p_buscar` VARCHAR(100)
)
BEGIN
    SELECT DISTINCT c.Id_Cliente, c.Nombre, c.Telefono, c.Estado,
           GROUP_CONCAT(m.Nombre SEPARATOR ', ') AS mascotas
    FROM clientes c
    LEFT JOIN mascota m ON c.Id_Cliente = m.Id_Cliente
    WHERE c.Nombre = p_buscar
    OR c.Telefono = p_buscar
    OR m.Nombre = p_buscar
    GROUP BY c.Id_Cliente, c.Nombre, c.Telefono, c.Estado;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_CambiarPassword
DELIMITER //
CREATE PROCEDURE `sp_CambiarPassword`(
	IN `p_id` INT,
	IN `p_password` VARCHAR(50)
)
BEGIN
    UPDATE usuarios SET password = p_password WHERE IdUsuario = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_CrearVenta
DELIMITER //
CREATE PROCEDURE `sp_CrearVenta`(
	IN `p_id_cliente` INT,
	IN `p_total` DECIMAL(20,6),
	IN `p_forma_pago` VARCHAR(50),
	IN `p_observacion` VARCHAR(250)
)
BEGIN
    INSERT INTO ventas (IdCliente, fecha, total, formaPago, estadoPago, observacion, estado)
    VALUES (p_id_cliente, CURDATE(), p_total, p_forma_pago, 'Pendiente', p_observacion, 1);
    SELECT LAST_INSERT_ID() AS IdVenta;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_EliminarCliente
DELIMITER //
CREATE PROCEDURE `sp_EliminarCliente`(
	IN `p_id` INT
)
BEGIN
    DELETE FROM clientes WHERE Id_Cliente = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_EliminarHistorial
DELIMITER //
CREATE PROCEDURE `sp_EliminarHistorial`(
	IN `p_id` INT
)
BEGIN
    DELETE FROM historial WHERE IdHistorial = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_EliminarMascota
DELIMITER //
CREATE PROCEDURE `sp_EliminarMascota`(
	IN `p_id` INT
)
BEGIN
    DELETE FROM mascota WHERE Id_Mascota = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_EliminarProducto
DELIMITER //
CREATE PROCEDURE `sp_EliminarProducto`(
	IN `p_id` INT
)
BEGIN
    UPDATE productos SET estado = 0 WHERE IdProductos = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_EliminarProveedor
DELIMITER //
CREATE PROCEDURE `sp_EliminarProveedor`(
	IN `p_id` INT
)
BEGIN
    UPDATE proveedores SET estado = 0 WHERE IdProveedores = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_EliminarTurno
DELIMITER //
CREATE PROCEDURE `sp_EliminarTurno`(
	IN `p_id` INT
)
BEGIN
    DELETE FROM turnos WHERE IdTurno = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_FichaCliente
DELIMITER //
CREATE PROCEDURE `sp_FichaCliente`(
	IN `p_id` INT
)
BEGIN
    SELECT * FROM clientes WHERE Id_Cliente = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_HistorialPorMascota
DELIMITER //
CREATE PROCEDURE `sp_HistorialPorMascota`(
	IN `p_id_mascota` INT
)
BEGIN
    SELECT * FROM historial
    WHERE IdMascota = p_id_mascota
    ORDER BY fechaConsulta DESC;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_InsertarCliente
DELIMITER //
CREATE PROCEDURE `sp_InsertarCliente`(
	IN `p_nombre` VARCHAR(100),
	IN `p_telefono` VARCHAR(20)
)
BEGIN
    INSERT INTO clientes (Nombre, Telefono, FechaAlta, Estado)
    VALUES (p_nombre, p_telefono, CURDATE(), 1);
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_InsertarCobro
DELIMITER //
CREATE PROCEDURE `sp_InsertarCobro`(
	IN `p_id_cliente` INT,
	IN `p_concepto` VARCHAR(250),
	IN `p_descripcion` VARCHAR(250),
	IN `p_monto` DECIMAL(20,6),
	IN `p_tipo_precio` VARCHAR(20),
	IN `p_forma_pago` VARCHAR(50),
	IN `p_observacion` INT
)
BEGIN
    INSERT INTO cobros (IdCliente, concepto, descripcion, monto,
        tipoPrecio, formaPago, estadoPago, fecha, observacion, estado)
    VALUES (p_id_cliente, p_concepto, p_descripcion, p_monto,
        p_tipo_precio, p_forma_pago, 'Pendiente', CURDATE(), 
        IFNULL(p_observacion,''), 1);
    SELECT LAST_INSERT_ID() AS IdCobro;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_InsertarCuotaCobro
DELIMITER //
CREATE PROCEDURE `sp_InsertarCuotaCobro`(
	IN `p_id_cobro` INT,
	IN `p_numero` INT,
	IN `p_fecha_venc` DATE,
	IN `p_monto` DECIMAL(20,6),
	IN `p_interes` DECIMAL(20,6)
)
BEGIN
    DECLARE v_monto_interes DECIMAL(20,2);
    SET v_monto_interes = p_monto + (p_monto * p_interes / 100);
    INSERT INTO cobrocuotas (IdCobro, numeroCuota, fechaVencimiento,
        montoCuota, interes, montoConInteres, saldoPendiente, estadoCuota, estado)
    VALUES (p_id_cobro, p_numero, p_fecha_venc,
        p_monto, p_interes, v_monto_interes, v_monto_interes, 'Pendiente', 1);
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_InsertarHistorial
DELIMITER //
CREATE PROCEDURE `sp_InsertarHistorial`(
	IN `p_id_mascota` INT,
	IN `p_id_turno` INT,
	IN `p_motivo` VARCHAR(50),
	IN `p_diagnostico` VARCHAR(100),
	IN `p_tratamiento` VARCHAR(250),
	IN `p_indicaciones` VARCHAR(250),
	IN `p_observaciones` VARCHAR(250),
	IN `p_fecha` DATE,
	IN `p_proximo` DATE
)
BEGIN
    INSERT INTO historial (IdMascota, IdTurno, motivoConsulta, diagnostico, tartamiento, indicaciones, observaciones, fechaConsulta, proximoControl, estado)
    VALUES (p_id_mascota, p_id_turno, p_motivo, p_diagnostico, IFNULL(p_tratamiento, ''), IFNULL(p_indicaciones, ''), IFNULL(p_observaciones, ''), p_fecha, p_proximo, 1);
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_InsertarMascota
DELIMITER //
CREATE PROCEDURE `sp_InsertarMascota`(
	IN `p_nombre` VARCHAR(50),
	IN `p_id_cliente` INT,
	IN `p_id_especie` INT,
	IN `p_sexo` VARCHAR(1),
	IN `p_fecha_nac` DATE
)
BEGIN
    INSERT INTO mascota (Nombre, Id_Cliente, Id_Especie, Sexo, FechaNacimiento)
    VALUES (p_nombre, p_id_cliente, p_id_especie, p_sexo, p_fecha_nac);
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_InsertarProducto
DELIMITER //
CREATE PROCEDURE `sp_InsertarProducto`(
	IN `p_codigo` VARCHAR(250),
	IN `p_nombre` VARCHAR(250),
	IN `p_stock` INT,
	IN `p_precio_min` DECIMAL(20,6),
	IN `p_precio_may` DECIMAL(20,6),
	IN `p_descripcion` VARCHAR(250),
	IN `p_id_proveedor` INT
)
BEGIN
    INSERT INTO productos (codigo, nombre, stock, p_minorista, p_mayorista, descripcion, IdProveedor, estado)
    VALUES (p_codigo, p_nombre, p_stock, p_precio_min, p_precio_may, p_descripcion, p_id_proveedor, 1);
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_InsertarProveedor
DELIMITER //
CREATE PROCEDURE `sp_InsertarProveedor`(
	IN `p_razon` VARCHAR(50),
	IN `p_dni` VARCHAR(50),
	IN `p_telefono` VARCHAR(50),
	IN `p_email` VARCHAR(50)
)
BEGIN
    INSERT INTO proveedores (razonSocial, dni, telefono, email, fechaAlta, estado)
    VALUES (p_razon, p_dni, p_telefono, p_email, CURDATE(), 1);
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_InsertarTurno
DELIMITER //
CREATE PROCEDURE `sp_InsertarTurno`(
	IN `p_fecha` DATE,
	IN `p_hora` TIME,
	IN `p_motivo` VARCHAR(50),
	IN `p_id_mascota` INT,
	IN `p_observacion` VARCHAR(50)
)
BEGIN
    INSERT INTO turnos (fecha, horaTurno, motivo, id_mascota, observacion, estadoTurno, estado)
    VALUES (p_fecha, p_hora, p_motivo, p_id_mascota, IFNULL(p_observacion, ''), 'Pendiente', 1);
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarAlertasCobros
DELIMITER //
CREATE PROCEDURE `sp_ListarAlertasCobros`()
BEGIN
    SELECT cc.IdCuota, cc.numeroCuota, cc.fechaVencimiento,
           cc.montoCuota, cc.interes, cc.montoConInteres,
           cc.saldoPendiente, cc.estadoCuota,
           co.IdCobro, co.concepto, co.descripcion, co.formaPago,
           c.Id_Cliente, c.Nombre AS nombre_cliente, c.Telefono,
           CASE
               WHEN cc.estadoCuota = 'Pagada' THEN 'PAGADA'
               WHEN cc.fechaVencimiento < CURDATE() THEN 'VENCIDA'
               WHEN cc.fechaVencimiento = CURDATE() THEN 'VENCE HOY'
               WHEN cc.fechaVencimiento <= DATE_ADD(CURDATE(), INTERVAL 7 DAY)
                   THEN 'PROXIMA A VENCER'
               ELSE 'AL DIA'
           END AS alerta
    FROM cobrocuotas cc
    JOIN cobros co ON cc.IdCobro = co.IdCobro
    JOIN clientes c ON co.IdCliente = c.Id_Cliente
    WHERE co.estado = 1

    UNION ALL

    SELECT 
        cu.IdCuotas AS IdCuota,
        cu.numeroCuota,
        cu.fechaVencimiento,
        cu.montoCuota,
        cu.interesHora AS interes,
        cu.montoCuota AS montoConInteres,
        cu.saldoPendiente,
        cu.estadoCuota,
        v.IdVentas AS IdCobro,
        'Venta' AS concepto,
        '' AS descripcion,
        v.formaPago,
        c.Id_Cliente,
        c.Nombre AS nombre_cliente,
        c.Telefono,
        CASE
            WHEN cu.estadoCuota = 'Pagada' THEN 'PAGADA'
            WHEN cu.fechaVencimiento < CURDATE() THEN 'VENCIDA'
            WHEN cu.fechaVencimiento = CURDATE() THEN 'VENCE HOY'
            WHEN cu.fechaVencimiento <= DATE_ADD(CURDATE(), INTERVAL 7 DAY)
                THEN 'PROXIMA A VENCER'
            ELSE 'AL DIA'
        END AS alerta
    FROM cuotas cu
    JOIN ventas v ON cu.IdVenta = v.IdVentas
    JOIN clientes c ON v.IdCliente = c.Id_Cliente
    WHERE cu.estado = 1

    ORDER BY fechaVencimiento ASC;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarClientes
DELIMITER //
CREATE PROCEDURE `sp_ListarClientes`()
BEGIN
    SELECT c.Id_Cliente, c.Nombre, c.Telefono, c.Estado,
           GROUP_CONCAT(m.Nombre SEPARATOR ', ') AS mascotas
    FROM clientes c
    LEFT JOIN mascota m ON c.Id_Cliente = m.Id_Cliente
    GROUP BY c.Id_Cliente, c.Nombre, c.Telefono, c.Estado;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarCobros
DELIMITER //
CREATE PROCEDURE `sp_ListarCobros`()
BEGIN
    SELECT co.IdCobro, co.concepto, co.descripcion, co.monto,
           co.tipoPrecio, co.formaPago, co.estadoPago, co.fecha,
           co.observacion,
           c.Nombre AS nombre_cliente, c.Telefono
    FROM cobros co
    JOIN clientes c ON co.IdCliente = c.Id_Cliente
    WHERE co.estado = 1
    ORDER BY co.IdCobro DESC;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarCuentasCorrientes
DELIMITER //
CREATE PROCEDURE `sp_ListarCuentasCorrientes`()
BEGIN
    SELECT 
        'Venta' AS tipo,
        v.IdVentas AS id,
        DATE(v.fecha) AS fecha,
        c.Nombre AS cliente,
        (SELECT GROUP_CONCAT(p.nombre, ' x', CAST(CAST(vd.cantidad AS UNSIGNED) AS CHAR) SEPARATOR ' | ')
         FROM ventadetalles vd
         JOIN productos p ON vd.IdProductos = p.IdProductos
         WHERE vd.IdVentas = v.IdVentas) AS concepto,
        v.total AS monto,
        v.formaPago,
        v.estadoPago,
        COUNT(cu.IdCuotas) AS totalCuotas,
        SUM(CASE WHEN cu.estadoCuota != 'Pagada' THEN cu.saldoPendiente ELSE 0 END) AS saldoTotal
    FROM ventas v
    JOIN clientes c ON v.IdCliente = c.Id_Cliente
    LEFT JOIN cuotas cu ON v.IdVentas = cu.IdVenta AND cu.estado = 1
    WHERE v.formaPago = 'Cuotas'
    GROUP BY v.IdVentas, v.fecha, c.Nombre, v.total, v.formaPago, v.estadoPago

    UNION ALL

    SELECT 
        'Cobro' AS tipo,
        co.IdCobro AS id,
        DATE(co.fecha) AS fecha,
        c.Nombre AS cliente,
        co.concepto,
        co.monto,
        co.formaPago,
        co.estadoPago,
        COUNT(cc.IdCuota) AS totalCuotas,
        SUM(CASE WHEN cc.estadoCuota != 'Pagada' THEN cc.saldoPendiente ELSE 0 END) AS saldoTotal
    FROM cobros co
    JOIN clientes c ON co.IdCliente = c.Id_Cliente
    LEFT JOIN cobrocuotas cc ON co.IdCobro = cc.IdCobro AND cc.estado = 1
    WHERE co.formaPago = 'Cuotas'
    GROUP BY co.IdCobro, co.fecha, c.Nombre, co.monto, co.formaPago, co.estadoPago, co.concepto

    ORDER BY fecha DESC;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarHistorial
DELIMITER //
CREATE PROCEDURE `sp_ListarHistorial`()
BEGIN
    SELECT h.*, m.Nombre AS nombre_mascota
    FROM historial h
    LEFT JOIN mascota m ON h.IdMascota = m.Id_Mascota;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarMascotas
DELIMITER //
CREATE PROCEDURE `sp_ListarMascotas`()
BEGIN
    SELECT m.Id_Mascota, m.Nombre, m.Sexo, m.FechaNacimiento, m.Estado,
           c.Nombre AS nombre_cliente, c.Apellido AS apellido_cliente,
           e.descripcion AS especie
    FROM mascota m
    LEFT JOIN clientes c ON m.Id_Cliente = c.Id_Cliente
    LEFT JOIN especies e ON m.Id_Especie = e.IdEspecies;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarProductos
DELIMITER //
CREATE PROCEDURE `sp_ListarProductos`()
BEGIN
    SELECT p.*, pr.razonSocial AS proveedor
    FROM productos p
    LEFT JOIN proveedores pr ON p.IdProveedor = pr.IdProveedores
    WHERE p.estado = 1 OR p.estado IS NULL;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarProveedores
DELIMITER //
CREATE PROCEDURE `sp_ListarProveedores`()
BEGIN
    SELECT * FROM proveedores WHERE estado = 1 OR estado IS NULL;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarTurnos
DELIMITER //
CREATE PROCEDURE `sp_ListarTurnos`()
BEGIN
    SELECT t.IdTurno, t.fecha, t.horaTurno, t.estadoTurno, 
           t.motivo, t.observacion, t.estado,
           m.Nombre AS nombre_mascota,
           c.Nombre AS nombre_cliente, c.Apellido AS apellido_cliente
    FROM turnos t
    LEFT JOIN mascota m ON t.id_mascota = m.Id_Mascota
    LEFT JOIN clientes c ON m.Id_Cliente = c.Id_Cliente;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ListarVentas
DELIMITER //
CREATE PROCEDURE `sp_ListarVentas`()
BEGIN
    SELECT v.IdVentas, v.fecha, v.total, v.formaPago, v.estadoPago, v.observacion,
           c.Nombre AS nombre_cliente, c.Apellido AS apellido_cliente
    FROM ventas v
    LEFT JOIN clientes c ON v.IdCliente = c.Id_Cliente
    WHERE v.estado = 1
    ORDER BY v.IdVentas DESC;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_MascotasPorCliente
DELIMITER //
CREATE PROCEDURE `sp_MascotasPorCliente`(
	IN `p_id_cliente` INT
)
BEGIN
    SELECT m.*, e.descripcion AS especie
    FROM mascota m
    LEFT JOIN especies e ON m.Id_Especie = e.IdEspecies
    WHERE m.Id_Cliente = p_id_cliente;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ObtenerCliente
DELIMITER //
CREATE PROCEDURE `sp_ObtenerCliente`(
	IN `p_id` INT
)
BEGIN
    SELECT * FROM clientes WHERE Id_Cliente = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ObtenerCobro
DELIMITER //
CREATE PROCEDURE `sp_ObtenerCobro`(IN p_id INT)
BEGIN
    SELECT co.IdCobro, co.concepto, co.descripcion, co.monto,
           co.tipoPrecio, co.formaPago, co.estadoPago, co.fecha,
           co.observacion,
           c.Nombre AS nombre_cliente, c.Telefono
    FROM cobros co
    JOIN clientes c ON co.IdCliente = c.Id_Cliente
    WHERE co.IdCobro = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ObtenerDetalleVenta
DELIMITER //
CREATE PROCEDURE `sp_ObtenerDetalleVenta`(
	IN `p_id` INT
)
BEGIN
    SELECT vd.IdVentasDetalles, vd.cantidad, vd.precioUnitario, 
           vd.descuentoItem, vd.subtotalItem,
           p.nombre AS nombre_producto, p.codigo
    FROM ventadetalles vd
    JOIN productos p ON vd.IdProductos = p.IdProductos
    WHERE vd.IdVentas = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ObtenerMascota
DELIMITER //
CREATE PROCEDURE `sp_ObtenerMascota`(
	IN `p_id` INT
)
BEGIN
    SELECT m.Id_Mascota, m.Nombre, m.Sexo, m.FechaNacimiento, m.Estado,
           m.Id_Cliente, m.Id_Especie,
           c.Nombre AS nombre_cliente, c.Apellido AS apellido_cliente,
           e.descripcion AS especie
    FROM mascota m
    LEFT JOIN clientes c ON m.Id_Cliente = c.Id_Cliente
    LEFT JOIN especies e ON m.Id_Especie = e.IdEspecies
    WHERE m.Id_Mascota = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ObtenerTurno
DELIMITER //
CREATE PROCEDURE `sp_ObtenerTurno`(
	IN `p_id` INT
)
BEGIN
    SELECT t.IdTurno, t.fecha, t.horaTurno, t.estadoTurno, 
           t.motivo, t.observacion, t.estado, t.id_mascota,
           m.Nombre AS nombre_mascota,
           c.Nombre AS nombre_cliente, c.Apellido AS apellido_cliente
    FROM turnos t
    LEFT JOIN mascota m ON t.id_mascota = m.Id_Mascota
    LEFT JOIN clientes c ON m.Id_Cliente = c.Id_Cliente
    WHERE t.IdTurno = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ObtenerUsuario
DELIMITER //
CREATE PROCEDURE `sp_ObtenerUsuario`(
	IN `p_id` INT
)
BEGIN
    SELECT * FROM usuarios WHERE IdUsuario = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_ObtenerVenta
DELIMITER //
CREATE PROCEDURE `sp_ObtenerVenta`(
	IN `p_id` INT
)
BEGIN
    SELECT v.IdVentas, v.IdCliente, v.total, v.formaPago, v.estadoPago, v.observacion,
           c.Nombre AS nombre_cliente, c.Apellido AS apellido_cliente
    FROM ventas v
    LEFT JOIN clientes c ON v.IdCliente = c.Id_Cliente
    WHERE v.IdVentas = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_PagarCuota
DELIMITER //
CREATE PROCEDURE `sp_PagarCuota`(
	IN `p_id` INT
)
BEGIN
    UPDATE cuotas SET 
        estadoCuota = 'Pagada',
        fechaPago = CURDATE(),
        montoPagodo = montoCuota,
        saldoPendiente = 0
    WHERE IdCuotas = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_PagarCuotaCobro
DELIMITER //
CREATE PROCEDURE `sp_PagarCuotaCobro`(
	IN `p_id_cuota` INT,
	IN `p_monto_pago` DECIMAL(20,6)
)
BEGIN
    DECLARE v_saldo DECIMAL(20,2);
    DECLARE v_id_cobro INT;
    DECLARE v_cuotas_pendientes INT;

    -- Obtener saldo y cobro padre
    SELECT saldoPendiente, IdCobro INTO v_saldo, v_id_cobro
    FROM cobrocuotas WHERE IdCuota = p_id_cuota;

    IF p_monto_pago >= v_saldo THEN
        UPDATE cobrocuotas SET
            estadoCuota = 'Pagada',
            fechaPago = CURDATE(),
            montoPagado = v_saldo,
            saldoPendiente = 0
        WHERE IdCuota = p_id_cuota;
    ELSE
        UPDATE cobrocuotas SET
            montoPagado = montoPagado + p_monto_pago,
            saldoPendiente = saldoPendiente - p_monto_pago
        WHERE IdCuota = p_id_cuota;
    END IF;

    -- Verificar si todas las cuotas del cobro están pagadas
    SELECT COUNT(*) INTO v_cuotas_pendientes
    FROM cobrocuotas
    WHERE IdCobro = v_id_cobro AND estadoCuota != 'Pagada' AND estado = 1;

    -- Si no quedan pendientes, marcar el cobro como Pagado
    IF v_cuotas_pendientes = 0 THEN
        UPDATE cobros SET estadoPago = 'Pagado' WHERE IdCobro = v_id_cobro;
    END IF;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_PagarCuotaVenta
DELIMITER //
CREATE PROCEDURE `sp_PagarCuotaVenta`(
    IN p_id_cuota INT,
    IN p_monto_pago DECIMAL(20,2)
)
BEGIN
    DECLARE v_saldo DECIMAL(20,2);
    DECLARE v_id_venta INT;
    DECLARE v_cuotas_pendientes INT;

    SELECT saldoPendiente, IdVenta INTO v_saldo, v_id_venta
    FROM cuotas WHERE IdCuotas = p_id_cuota;

    IF p_monto_pago >= v_saldo THEN
        UPDATE cuotas SET
            estadoCuota = 'Pagada',
            fechaPago = CURDATE(),
            montoPagodo = v_saldo,
            saldoPendiente = 0
        WHERE IdCuotas = p_id_cuota;
    ELSE
        UPDATE cuotas SET
            montoPagodo = montoPagodo + p_monto_pago,
            saldoPendiente = saldoPendiente - p_monto_pago
        WHERE IdCuotas = p_id_cuota;
    END IF;

    SELECT COUNT(*) INTO v_cuotas_pendientes
    FROM cuotas
    WHERE IdVenta = v_id_venta AND estadoCuota != 'Pagada' AND estado = 1;

    IF v_cuotas_pendientes = 0 THEN
        UPDATE ventas SET estadoPago = 'Pagado' WHERE IdVentas = v_id_venta;
    END IF;
END//
DELIMITER ;

-- Volcando estructura para procedimiento bd_veterinarianogal.sp_TurnosPorMascota
DELIMITER //
CREATE PROCEDURE `sp_TurnosPorMascota`(IN p_id_mascota INT)
BEGIN
    SELECT IdTurno, fecha, horaTurno, motivo, estadoTurno, observacion
    FROM turnos
    WHERE id_mascota = p_id_mascota AND estado = 1
    ORDER BY fecha ASC, horaTurno ASC;
END//
DELIMITER ;

-- Volcando estructura para tabla bd_veterinarianogal.tipo
CREATE TABLE IF NOT EXISTS `tipo` (
  `IdTipo` int(11) NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(50) NOT NULL DEFAULT '0',
  `estado` tinyint(4) DEFAULT 0,
  PRIMARY KEY (`IdTipo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.tipo: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.turnos
CREATE TABLE IF NOT EXISTS `turnos` (
  `IdTurno` int(11) NOT NULL AUTO_INCREMENT,
  `fecha` date NOT NULL,
  `estadoTurno` varchar(50) NOT NULL DEFAULT '',
  `horaTurno` time NOT NULL,
  `motivo` varchar(50) NOT NULL DEFAULT '',
  `id_mascota` int(11) NOT NULL DEFAULT 0,
  `observacion` varchar(50) NOT NULL DEFAULT '0',
  `estado` tinyint(4) DEFAULT 0,
  PRIMARY KEY (`IdTurno`),
  KEY `id_mascota` (`id_mascota`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.turnos: ~0 rows (aproximadamente)
INSERT INTO `turnos` (`IdTurno`, `fecha`, `estadoTurno`, `horaTurno`, `motivo`, `id_mascota`, `observacion`, `estado`) VALUES
	(7, '2026-05-10', 'Pendiente', '22:00:00', 'revision', 1, 'tiene que hacer repsoso', 1);

-- Volcando estructura para tabla bd_veterinarianogal.usuarios
CREATE TABLE IF NOT EXISTS `usuarios` (
  `IdUsuario` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL,
  `rol` varchar(50) NOT NULL DEFAULT 'admin',
  `estado` tinyint(4) DEFAULT 1,
  PRIMARY KEY (`IdUsuario`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.usuarios: ~1 rows (aproximadamente)
INSERT INTO `usuarios` (`IdUsuario`, `nombre`, `email`, `password`, `rol`, `estado`) VALUES
	(1, 'Administrador', 'admin@elnogal.com', 'admin123', 'admin', 1);

-- Volcando estructura para tabla bd_veterinarianogal.ventadetalles
CREATE TABLE IF NOT EXISTS `ventadetalles` (
  `IdVentasDetalles` int(11) NOT NULL AUTO_INCREMENT,
  `IdVentas` int(11) NOT NULL,
  `IdProductos` int(11) NOT NULL,
  `cantidad` decimal(20,6) NOT NULL,
  `precioUnitario` decimal(20,6) NOT NULL,
  `descuentoItem` decimal(20,6) NOT NULL,
  `subtotalItem` decimal(20,6) NOT NULL,
  PRIMARY KEY (`IdVentasDetalles`),
  KEY `IdVentas` (`IdVentas`),
  KEY `IdProductos` (`IdProductos`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.ventadetalles: ~0 rows (aproximadamente)

-- Volcando estructura para tabla bd_veterinarianogal.ventas
CREATE TABLE IF NOT EXISTS `ventas` (
  `IdVentas` int(11) NOT NULL AUTO_INCREMENT,
  `numeroVenta` varchar(50) DEFAULT NULL,
  `fecha` decimal(20,6) DEFAULT NULL,
  `IdCliente` decimal(20,6) DEFAULT NULL,
  `subtotal` decimal(20,6) DEFAULT NULL,
  `descuento` decimal(20,6) DEFAULT NULL,
  `recargo` decimal(20,6) DEFAULT NULL,
  `total` decimal(20,6) DEFAULT NULL,
  `formaPago` varchar(50) NOT NULL,
  `estadoPago` varchar(50) NOT NULL,
  `observacion` varchar(250) NOT NULL,
  `estado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`IdVentas`),
  KEY `IdCliente` (`IdCliente`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla bd_veterinarianogal.ventas: ~0 rows (aproximadamente)

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
