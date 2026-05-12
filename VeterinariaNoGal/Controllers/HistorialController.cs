using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;

namespace VeterinariaNoGal.Controllers
{
    public class HistorialController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        public IActionResult Index()
        {
            List<Historial> lista = new List<Historial>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarHistorial", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Historial
                    {
                        IdHistorial = Convert.ToInt32(reader["IdHistorial"]),
                        IdMascota = Convert.ToInt32(reader["IdMascota"]),
                        IdTurno = Convert.ToInt32(reader["IdTurno"]),
                        MotivoConsulta = reader["motivoConsulta"].ToString(),
                        Diagnostico = reader["diagnostico"].ToString(),
                        Tratamiento = reader["tartamiento"].ToString(),
                        Indicaciones = reader["indicaciones"].ToString(),
                        Observaciones = reader["observaciones"].ToString(),
                        FechaConsulta = reader["fechaConsulta"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["fechaConsulta"]),
                        ProximoControl = reader["proximoControl"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["proximoControl"]),
                        NombreMascota = reader["nombre_mascota"].ToString()
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
                        Nombre = reader["Nombre"].ToString()
                    });
                }
            }
            ViewBag.Mascotas = mascotas;
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Historial h)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_InsertarHistorial", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id_mascota", h.IdMascota);
                cmd.Parameters.AddWithValue("p_id_turno", h.IdTurno);
                cmd.Parameters.AddWithValue("p_motivo", h.MotivoConsulta);
                cmd.Parameters.AddWithValue("p_diagnostico", h.Diagnostico);
                cmd.Parameters.AddWithValue("p_tratamiento", h.Tratamiento);
                cmd.Parameters.AddWithValue("p_indicaciones", h.Indicaciones);
                cmd.Parameters.AddWithValue("p_observaciones", h.Observaciones);
                cmd.Parameters.AddWithValue("p_fecha", h.FechaConsulta);
                cmd.Parameters.AddWithValue("p_proximo", h.ProximoControl);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EliminarHistorial", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
