using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;

namespace VeterinariaNoGal.Controllers
{
    public class AlertaController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        public IActionResult Index()
        {
            List<Alerta> lista = new List<Alerta>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_AlertasDeuda", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Alerta
                    {
                        Id_Cliente = Convert.ToInt32(reader["Id_Cliente"]),
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        IdCuentasC = Convert.ToInt32(reader["IdCuentasC"]),
                        Importe = Convert.ToDecimal(reader["importe"]),
                        SaldoNuevo = Convert.ToDecimal(reader["saldoNuevo"]),
                        VencimientoCuenta = Convert.ToDateTime(reader["vencimiento_cuenta"]),
                        IdCuotas = Convert.ToInt32(reader["IdCuotas"]),
                        NumeroCuota = Convert.ToInt32(reader["numeroCuota"]),
                        MontoCuota = Convert.ToDecimal(reader["montoCuota"]),
                        SaldoPendiente = Convert.ToDecimal(reader["saldoPendiente"]),
                        VencimientoCuota = Convert.ToDateTime(reader["vencimiento_cuota"]),
                        EstadoCuota = reader["estadoCuota"].ToString(),
                        Alerta_ = reader["alerta"].ToString()
                    });
                }
            }
            return View(lista);
        }
        public IActionResult PagarCuota(int id)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_PagarCuota", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}