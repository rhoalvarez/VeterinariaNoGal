using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;

namespace VeterinariaNoGal.Controllers
{
    public class CuentasCorrientesController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        public IActionResult Index()
        {
            List<CuentaCorriente> lista = new List<CuentaCorriente>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarCuentasCorrientes", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new CuentaCorriente
                    {
                        Tipo = reader["tipo"].ToString(),
                        Id = Convert.ToInt32(reader["id"]),
                        Fecha = reader["fecha"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["fecha"].ToString()),
                        Cliente = reader["cliente"].ToString(),
                        Concepto = reader["concepto"].ToString(),
                        Monto = Convert.ToDecimal(reader["monto"]),
                        FormaPago = reader["formaPago"].ToString(),
                        EstadoPago = reader["estadoPago"].ToString(),
                        TotalCuotas = Convert.ToInt32(reader["totalCuotas"]),
                        SaldoTotal = Convert.ToDecimal(reader["saldoTotal"])
                    });
                }
            }
            return View(lista);
        }
    }
}
