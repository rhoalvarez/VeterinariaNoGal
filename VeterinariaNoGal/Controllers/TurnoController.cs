using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;

namespace VeterinariaNoGal.Controllers
{
    public class TurnoController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        public IActionResult Index()
        {
            List<Turno> lista = new List<Turno>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarTurnos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Turno
                    {
                        IdTurno = Convert.ToInt32(reader["IdTurno"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        HoraTurno = (TimeSpan)reader["horaTurno"],
                        EstadoTurno = reader["estadoTurno"].ToString(),
                        Motivo = reader["motivo"].ToString(),
                        Observacion = reader["observacion"].ToString(),
                        Estado = Convert.ToInt32(reader["estado"] == DBNull.Value ? 0 : reader["estado"]),
                        NombreMascota = reader["nombre_mascota"].ToString(),
                        NombreCliente = reader["nombre_cliente"].ToString(),
                        ApellidoCliente = reader["apellido_cliente"].ToString()
                    });
                }
            }
            return View(lista);
        }

        public IActionResult Crear()
        {
            List<Mascota> mascotas = new List<Mascota>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarMascotas", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mascotas.Add(new Mascota
                    {
                        Id_Mascota = Convert.ToInt32(reader["Id_Mascota"]),
                        Nombre = reader["Nombre"].ToString(),
                        NombreCliente = reader["nombre_cliente"].ToString(),
                        ApellidoCliente = reader["apellido_cliente"].ToString()
                    });
                }
            }
            ViewBag.Mascotas = mascotas;
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Turno t)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_InsertarTurno", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_fecha", t.Fecha);
                cmd.Parameters.AddWithValue("p_hora", t.HoraTurno);
                cmd.Parameters.AddWithValue("p_motivo", t.Motivo);
                cmd.Parameters.AddWithValue("p_id_mascota", t.Id_Mascota);
                cmd.Parameters.AddWithValue("p_observacion", t.Observacion);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            Turno turno = null;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ObtenerTurno", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    turno = new Turno
                    {
                        IdTurno = Convert.ToInt32(reader["IdTurno"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        HoraTurno = (TimeSpan)reader["horaTurno"],
                        EstadoTurno = reader["estadoTurno"].ToString(),
                        Motivo = reader["motivo"].ToString(),
                        Observacion = reader["observacion"].ToString(),
                        Id_Mascota = Convert.ToInt32(reader["id_mascota"])
                    };
                }
            }
            return View(turno);
        }

        [HttpPost]
        public IActionResult Editar(Turno t)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ActualizarTurno", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", t.IdTurno);
                cmd.Parameters.AddWithValue("p_fecha", t.Fecha);
                cmd.Parameters.AddWithValue("p_hora", t.HoraTurno);
                cmd.Parameters.AddWithValue("p_motivo", t.Motivo);
                cmd.Parameters.AddWithValue("p_estado", t.EstadoTurno);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CambiarEstado(int idTurno, string nuevoEstado, int idCliente)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE turnos SET estadoTurno = @estado WHERE IdTurno = @id", con);
                cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@id", idTurno);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Ficha", "Cliente", new { id = idCliente });
        }

        public IActionResult Eliminar(int id)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EliminarTurno", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
