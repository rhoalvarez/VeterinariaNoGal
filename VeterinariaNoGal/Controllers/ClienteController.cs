using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;

namespace VeterinariaNoGal.Controllers
{
    public class ClienteController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        public IActionResult Index()
        {
            List<Cliente> lista = new List<Cliente>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarClientes", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Cliente
                    {
                        Id_Cliente = Convert.ToInt32(reader["Id_Cliente"]),
                        Nombre = reader["Nombre"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Estado = Convert.ToInt32(reader["Estado"] == DBNull.Value ? 0 : reader["Estado"]),
                        Apellido = reader["mascotas"].ToString()
                    });
                }
            }
            return View(lista);
        }

        public IActionResult Crear()
        {
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
            ViewBag.Especies = especies;
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Cliente c, string NombreMascota, int IdEspecie,
            string SexoMascota, DateTime? FechaNacMascota,
            DateTime? FechaConsulta, string MotivoConsulta,
            string Diagnostico, string Tratamiento)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_InsertarCliente", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_nombre", c.Nombre);
                cmd.Parameters.AddWithValue("p_telefono", c.Telefono);
                cmd.ExecuteNonQuery();
            }

            int idCliente = 0;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT LAST_INSERT_ID() AS id", con);
                idCliente = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (!string.IsNullOrEmpty(NombreMascota) && FechaNacMascota.HasValue)
            {
                using (MySqlConnection con = conexion.ObtenerConexion())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_InsertarMascota", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_nombre", NombreMascota);
                    cmd.Parameters.AddWithValue("p_id_cliente", idCliente);
                    cmd.Parameters.AddWithValue("p_id_especie", IdEspecie);
                    cmd.Parameters.AddWithValue("p_sexo", SexoMascota);
                    cmd.Parameters.AddWithValue("p_fecha_nac", FechaNacMascota.Value);
                    cmd.ExecuteNonQuery();
                }

                int idMascota = 0;
                using (MySqlConnection con = conexion.ObtenerConexion())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT LAST_INSERT_ID() AS id", con);
                    idMascota = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (!string.IsNullOrEmpty(MotivoConsulta) && FechaConsulta.HasValue)
                {
                    using (MySqlConnection con = conexion.ObtenerConexion())
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("sp_InsertarHistorial", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_id_mascota", idMascota);
                        cmd.Parameters.AddWithValue("p_id_turno", 0);
                        cmd.Parameters.AddWithValue("p_motivo", MotivoConsulta);
                        cmd.Parameters.AddWithValue("p_diagnostico", Diagnostico ?? "");
                        cmd.Parameters.AddWithValue("p_tratamiento", Tratamiento ?? "");
                        cmd.Parameters.AddWithValue("p_indicaciones", "");
                        cmd.Parameters.AddWithValue("p_observaciones", "");
                        cmd.Parameters.AddWithValue("p_fecha", FechaConsulta.Value);
                        cmd.Parameters.AddWithValue("p_proximo", FechaConsulta.Value.AddMonths(6));
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            return RedirectToAction("Ficha", new { id = idCliente });
        }

        public IActionResult Editar(int id)
        {
            Cliente cliente = null;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ObtenerCliente", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cliente = new Cliente
                    {
                        Id_Cliente = Convert.ToInt32(reader["Id_Cliente"]),
                        Nombre = reader["Nombre"].ToString(),
                        Telefono = reader["Telefono"].ToString()
                    };
                }
            }
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Editar(Cliente c)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ActualizarCliente", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", c.Id_Cliente);
                cmd.Parameters.AddWithValue("p_nombre", c.Nombre);
                cmd.Parameters.AddWithValue("p_telefono", c.Telefono);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EliminarCliente", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Ficha(int id)
        {
            Cliente cliente = null;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_FichaCliente", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cliente = new Cliente
                    {
                        Id_Cliente = Convert.ToInt32(reader["Id_Cliente"]),
                        Nombre = reader["Nombre"].ToString(),
                        Telefono = reader["Telefono"].ToString()
                    };
                }
            }

            List<Mascota> mascotas = new List<Mascota>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_MascotasPorCliente", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id_cliente", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mascotas.Add(new Mascota
                    {
                        Id_Mascota = Convert.ToInt32(reader["Id_Mascota"]),
                        Nombre = reader["Nombre"].ToString(),
                        Sexo = reader["Sexo"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                        Especie = reader["especie"].ToString()
                    });
                }
            }

            ViewBag.Mascotas = mascotas;
            ViewBag.IdCliente = id;

            Dictionary<int, List<Historial>> historiales = new Dictionary<int, List<Historial>>();
            Dictionary<int, List<Turno>> turnos = new Dictionary<int, List<Turno>>();

            foreach (var mascota in mascotas)
            {
                // Historial
                List<Historial> hist = new List<Historial>();
                using (MySqlConnection con = conexion.ObtenerConexion())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_HistorialPorMascota", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_id_mascota", mascota.Id_Mascota);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        hist.Add(new Historial
                        {
                            IdHistorial = Convert.ToInt32(reader["IdHistorial"]),
                            MotivoConsulta = reader["motivoConsulta"].ToString(),
                            Diagnostico = reader["diagnostico"].ToString(),
                            Tratamiento = reader["tartamiento"].ToString(),
                            Observaciones = reader["observaciones"].ToString(),
                            FechaConsulta = reader["fechaConsulta"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["fechaConsulta"]),
                            ProximoControl = reader["proximoControl"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["proximoControl"])
                        });
                    }
                }
                historiales[mascota.Id_Mascota] = hist;

                // Turnos por mascota
                List<Turno> turnsList = new List<Turno>();
                using (MySqlConnection con = conexion.ObtenerConexion())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_TurnosPorMascota", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_id_mascota", mascota.Id_Mascota);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        turnsList.Add(new Turno
                        {
                            IdTurno = Convert.ToInt32(reader["IdTurno"]),
                            Fecha = Convert.ToDateTime(reader["fecha"]),
                            HoraTurno = (TimeSpan)reader["horaTurno"],
                            Motivo = reader["motivo"].ToString(),
                            EstadoTurno = reader["estadoTurno"].ToString(),
                            Observacion = reader["observacion"].ToString()
                        });
                    }
                }
                turnos[mascota.Id_Mascota] = turnsList;
            }

            ViewBag.Historiales = historiales;
            ViewBag.Turnos = turnos;

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
            ViewBag.Especies = especies;

            return View(cliente);
        }

        [HttpPost]
        public IActionResult AgregarMascota(Mascota m)
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
            return RedirectToAction("Ficha", new { id = m.Id_Cliente });
        }

        [HttpPost]
        public IActionResult AgregarVisita(Historial h, int idCliente)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_InsertarHistorial", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id_mascota", h.IdMascota);
                cmd.Parameters.AddWithValue("p_id_turno", 0);
                cmd.Parameters.AddWithValue("p_motivo", h.MotivoConsulta);
                cmd.Parameters.AddWithValue("p_diagnostico", h.Diagnostico);
                cmd.Parameters.AddWithValue("p_tratamiento", h.Tratamiento ?? "");
                cmd.Parameters.AddWithValue("p_indicaciones", h.Indicaciones ?? "");
                cmd.Parameters.AddWithValue("p_observaciones", h.Observaciones ?? "");
                cmd.Parameters.AddWithValue("p_fecha", h.FechaConsulta);
                cmd.Parameters.AddWithValue("p_proximo", h.ProximoControl);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Ficha", new { id = idCliente });
        }

        [HttpPost]
        public IActionResult AgregarTurno(int idMascota, int idCliente, DateTime fecha, TimeSpan horaTurno, string motivo, string observacion)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_InsertarTurno", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_fecha", fecha);
                cmd.Parameters.AddWithValue("p_hora", horaTurno);
                cmd.Parameters.AddWithValue("p_motivo", motivo ?? "");
                cmd.Parameters.AddWithValue("p_id_mascota", idMascota);
                cmd.Parameters.AddWithValue("p_observacion", observacion ?? "");
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Ficha", new { id = idCliente });
        }

        [HttpPost]
        public IActionResult EliminarTurno(int idTurno, int idCliente)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EliminarTurno", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", idTurno);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Ficha", new { id = idCliente });
        }

        public IActionResult Buscar(string buscar)
        {
            List<Cliente> lista = new List<Cliente>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_BuscarCliente", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_buscar", buscar ?? "");
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Cliente
                    {
                        Id_Cliente = Convert.ToInt32(reader["Id_Cliente"]),
                        Nombre = reader["Nombre"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Estado = Convert.ToInt32(reader["Estado"] == DBNull.Value ? 0 : reader["Estado"]),
                        Apellido = reader["mascotas"].ToString()
                    });
                }
            }
            ViewBag.Buscar = buscar;
            return View("Index", lista);
        }

        [HttpPost]
        public IActionResult EditarMascota(Mascota m, int idCliente)
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
            return RedirectToAction("Ficha", new { id = idCliente });
        }
    }
}