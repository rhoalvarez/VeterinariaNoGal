using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;

namespace VeterinariaNoGal.Controllers
{
    public class LoginController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("usuario") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM usuarios WHERE nombre = @nombre AND password = @password AND estado = 1", con);
                cmd.Parameters.AddWithValue("@nombre", email);
                cmd.Parameters.AddWithValue("@password", password);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    HttpContext.Session.SetString("usuario", reader["nombre"].ToString());
                    HttpContext.Session.SetString("rol", reader["rol"].ToString());
                    HttpContext.Session.SetString("idUsuario", reader["IdUsuario"].ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Perfil()
        {
            if (HttpContext.Session.GetString("usuario") == null)
                return RedirectToAction("Index", "Login");

            int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("idUsuario"));
            Usuario usuario = null;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ObtenerUsuario", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", idUsuario);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                        Nombre = reader["nombre"].ToString(),
                        Email = reader["email"].ToString(),
                        Rol = reader["rol"].ToString()
                    };
                }
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult CambiarPassword(int idUsuario, string passwordActual, string passwordNueva, string passwordConfirmar)
        {
            if (passwordNueva != passwordConfirmar)
            {
                TempData["Error"] = "Las contraseñas nuevas no coinciden";
                return RedirectToAction("Perfil");
            }

            bool passwordCorrecta = false;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT COUNT(*) FROM usuarios WHERE IdUsuario = @id AND password = @pass", con);
                cmd.Parameters.AddWithValue("@id", idUsuario);
                cmd.Parameters.AddWithValue("@pass", passwordActual);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                passwordCorrecta = count > 0;
            }

            if (!passwordCorrecta)
            {
                TempData["Error"] = "La contraseña actual es incorrecta";
                return RedirectToAction("Perfil");
            }

            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_CambiarPassword", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", idUsuario);
                cmd.Parameters.AddWithValue("p_password", passwordNueva);
                cmd.ExecuteNonQuery();
            }

            TempData["Exito"] = "Contraseña cambiada correctamente";
            return RedirectToAction("Perfil");
        }

        [HttpPost]
        public IActionResult ActualizarPerfil(int idUsuario, string nombre)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE usuarios SET nombre = @nombre WHERE IdUsuario = @id", con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@id", idUsuario);
                cmd.ExecuteNonQuery();
            }
            HttpContext.Session.SetString("usuario", nombre);
            TempData["Exito"] = "Perfil actualizado correctamente";
            return RedirectToAction("Perfil");
        }

        public IActionResult Usuarios()
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Index", "Home");

            List<Usuario> lista = new List<Usuario>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT IdUsuario, nombre, rol, estado FROM usuarios", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Usuario
                    {
                        IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                        Nombre = reader["nombre"].ToString(),
                        Rol = reader["rol"].ToString(),
                        Estado = Convert.ToInt32(reader["estado"])
                    });
                }
            }
            return View(lista);
        }

        [HttpPost]
        public IActionResult CrearUsuario(string nombre, string password, string rol)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO usuarios (nombre, password, rol, estado) VALUES (@nombre, @password, @rol, 1)", con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.ExecuteNonQuery();
            }
            TempData["Exito"] = "Usuario creado correctamente";
            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        public IActionResult ToggleUsuario(int idUsuario)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE usuarios SET estado = CASE WHEN estado = 1 THEN 0 ELSE 1 END WHERE IdUsuario = @id", con);
                cmd.Parameters.AddWithValue("@id", idUsuario);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        public IActionResult EliminarUsuario(int idUsuario)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "DELETE FROM usuarios WHERE IdUsuario = @id", con);
                cmd.Parameters.AddWithValue("@id", idUsuario);
                cmd.ExecuteNonQuery();
            }
            TempData["Exito"] = "Usuario eliminado correctamente";
            return RedirectToAction("Usuarios");
        }
    }
}