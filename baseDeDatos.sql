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

-- Volcando datos para la tabla bd_veterinarianogal.clientes: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.cobrocuotas: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.cobros: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.cuentascorrientes: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.cuotas: ~0 rows (aproximadamente)

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

-- Volcando datos para la tabla bd_veterinarianogal.historial: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.mascota: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.productos: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.proveedores: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.tipo: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.turnos: ~1 rows (aproximadamente)
INSERT INTO `turnos` (`IdTurno`, `fecha`, `estadoTurno`, `horaTurno`, `motivo`, `id_mascota`, `observacion`, `estado`) VALUES
	(7, '2026-05-10', 'Pendiente', '22:00:00', 'revision', 1, 'tiene que hacer repsoso', 1);

-- Volcando datos para la tabla bd_veterinarianogal.usuarios: ~1 rows (aproximadamente)
INSERT INTO `usuarios` (`IdUsuario`, `nombre`, `email`, `password`, `rol`, `estado`) VALUES
	(1, 'Administrador', 'admin@elnogal.com', 'admin123', 'admin', 1);

-- Volcando datos para la tabla bd_veterinarianogal.ventadetalles: ~0 rows (aproximadamente)

-- Volcando datos para la tabla bd_veterinarianogal.ventas: ~0 rows (aproximadamente)

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
