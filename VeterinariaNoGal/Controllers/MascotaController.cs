using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;

namespace VeterinariaNoGal.Controllers
{
    public class MascotaController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        public IActionResult Index()
        {
            List<Mascota> lista = new List<Mascota>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarMascotas", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Mascota
                    {
                        Id_Mascota = Convert.ToInt32(reader["Id_Mascota"]),
                        Nombre = reader["Nombre"].ToString(),
                        Sexo = reader["Sexo"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                        Estado = Convert.ToInt32(reader["Estado"] == DBNull.Value ? 0 : reader["Estado"]),
                        NombreCliente = reader["nombre_cliente"].ToString(),
                        ApellidoCliente = reader["apellido_cliente"].ToString(),
                        Especie = reader["especie"].ToString()
                    });
                }
            }
            return View(lista);
        }

        public IActionResult Crear()
        {
            // Cargar clientes para el desplegable
            List<Cliente> clientes = new List<Cliente>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarClientes", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id_Cliente = Convert.ToInt32(reader["Id_Cliente"]),
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString()
                    });
                }
            }

            // Cargar especies para el desplegable
            List<Especie> especies = new List<Especie>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT IdEspecies, descripcion FROM especies WHERE estado = 1", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    especies.Add(new Especie
                    {
                        IdEspecies = Convert.ToInt32(reader["IdEspecies"]),
                        Descripcion = reader["descripcion"].ToString()
                    });
                }
            }

            ViewBag.Clientes = clientes;
            ViewBag.Especies = especies;
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Mascota m)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_InsertarMascota", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_nombre", m.Nombre);
                cmd.Parameters.AddWithValue("p_id_cliente", m.Id_Cliente);
                cmd.Parameters.AddWithValue("p_id_especie", m.Id_Especie);
                cmd.Parameters.AddWithValue("p_sexo", m.Sexo);
                cmd.Parameters.AddWithValue("p_fecha_nac", m.FechaNacimiento);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            Mascota mascota = null;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ObtenerMascota", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    mascota = new Mascota
                    {
                        Id_Mascota = Convert.ToInt32(reader["Id_Mascota"]),
                        Nombre = reader["Nombre"].ToString(),
                        Id_Cliente = Convert.ToInt32(reader["Id_Cliente"]),
                        Id_Especie = Convert.ToInt32(reader["Id_Especie"]),
                        Sexo = reader["Sexo"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"])
                    };
                }
            }
            return View(mascota);
        }

        [HttpPost]
        public IActionResult Editar(Mascota m)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ActualizarMascota", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", m.Id_Mascota);
                cmd.Parameters.AddWithValue("p_nombre", m.Nombre);
                cmd.Parameters.AddWithValue("p_id_cliente", m.Id_Cliente);
                cmd.Parameters.AddWithValue("p_id_especie", m.Id_Especie);
                cmd.Parameters.AddWithValue("p_sexo", m.Sexo);
                cmd.Parameters.AddWithValue("p_fecha_nac", m.FechaNacimiento);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EliminarMascota", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
