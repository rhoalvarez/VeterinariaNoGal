using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;

namespace VeterinariaNoGal.Controllers
{
    public class CobrosController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        // GET /Cobros/Index
        public IActionResult Index()
        {
            List<Cobro> lista = new List<Cobro>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarCobros", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Cobro
                    {
                        IdCobro = Convert.ToInt32(reader["IdCobro"]),
                        Concepto = reader["concepto"].ToString(),
                        Descripcion = reader["descripcion"].ToString(),
                        Monto = Convert.ToDecimal(reader["monto"]),
                        TipoPrecio = reader["tipoPrecio"].ToString(),
                        FormaPago = reader["formaPago"].ToString(),
                        EstadoPago = reader["estadoPago"].ToString(),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        NombreCliente = reader["nombre_cliente"].ToString(),
                        Telefono = reader["Telefono"].ToString()
                    });
                }
            }
            return View(lista);
        }

        // GET /Cobros/Crear
        public IActionResult Crear()
        {
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
                        Telefono = reader["Telefono"].ToString()
                    });
                }
            }
            ViewBag.Clientes = clientes;
            return View();
        }

        // POST /Cobros/Crear
        [HttpPost]
        public IActionResult Crear(int idCliente, string concepto, string descripcion,
            decimal monto, string tipoPrecio, string formaPago, string observacion,
            List<DateTime> fechasVenc, List<decimal> montosCuota, List<decimal> intereses)
        {
            int idCobro = 0;

            // 1. Guarda el cobro
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_InsertarCobro", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id_cliente", idCliente);
                cmd.Parameters.AddWithValue("p_concepto", concepto);
                cmd.Parameters.AddWithValue("p_descripcion", descripcion ?? "");
                cmd.Parameters.AddWithValue("p_monto", monto);
                cmd.Parameters.AddWithValue("p_tipo_precio", tipoPrecio);
                cmd.Parameters.AddWithValue("p_forma_pago", formaPago);
                cmd.Parameters.AddWithValue("p_observacion", observacion ?? "");
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    idCobro = Convert.ToInt32(reader["IdCobro"]);
            }

            // 2. Si es en cuotas, guarda cada cuota
            if (formaPago == "Cuotas" && fechasVenc != null && fechasVenc.Count > 0)
            {
                for (int i = 0; i < fechasVenc.Count; i++)
                {
                    using (MySqlConnection con = conexion.ObtenerConexion())
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("sp_InsertarCuotaCobro", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_id_cobro", idCobro);
                        cmd.Parameters.AddWithValue("p_numero", i + 1);
                        cmd.Parameters.AddWithValue("p_fecha_venc", fechasVenc[i]);
                        cmd.Parameters.AddWithValue("p_monto", montosCuota[i]);
                        cmd.Parameters.AddWithValue("p_interes", intereses[i]);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        // GET /Cobros/Alertas
        public IActionResult Alertas()
        {
            List<CuotaCobro> lista = new List<CuotaCobro>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarAlertasCobros", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new CuotaCobro
                    {
                        IdCuota = Convert.ToInt32(reader["IdCuota"]),
                        NumeroCuota = Convert.ToInt32(reader["numeroCuota"]),
                        FechaVencimiento = Convert.ToDateTime(reader["fechaVencimiento"]),
                        MontoCuota = Convert.ToDecimal(reader["montoCuota"]),
                        Interes = Convert.ToDecimal(reader["interes"]),
                        MontoConInteres = Convert.ToDecimal(reader["montoConInteres"]),
                        SaldoPendiente = Convert.ToDecimal(reader["saldoPendiente"]),
                        EstadoCuota = reader["estadoCuota"].ToString(),
                        Concepto = reader["concepto"].ToString(),
                        NombreCliente = reader["nombre_cliente"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        IdCliente = Convert.ToInt32(reader["Id_Cliente"]),
                        Alerta = reader["alerta"].ToString()
                    });
                }
            }
            return View(lista);
        }

        public IActionResult Detalle(int id)
        {
            Cobro cobro = null;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ObtenerCobro", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cobro = new Cobro
                    {
                        IdCobro = Convert.ToInt32(reader["IdCobro"]),
                        Concepto = reader["concepto"].ToString(),
                        Descripcion = reader["descripcion"].ToString(),
                        Monto = Convert.ToDecimal(reader["monto"]),
                        FormaPago = reader["formaPago"].ToString(),
                        EstadoPago = reader["estadoPago"].ToString(),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Observacion = reader["observacion"].ToString(),
                        NombreCliente = reader["nombre_cliente"].ToString(),
                        Telefono = reader["Telefono"].ToString()
                    };
                }
            }

            List<CuotaCobro> cuotas = new List<CuotaCobro>();
            if (cobro != null && cobro.FormaPago == "Cuotas")
            {
                using (MySqlConnection con = conexion.ObtenerConexion())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT * FROM cobrocuotas WHERE IdCobro = @id AND estado = 1 ORDER BY numeroCuota", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cuotas.Add(new CuotaCobro
                        {
                            IdCuota = Convert.ToInt32(reader["IdCuota"]),
                            NumeroCuota = Convert.ToInt32(reader["numeroCuota"]),
                            FechaVencimiento = Convert.ToDateTime(reader["fechaVencimiento"]),
                            MontoCuota = Convert.ToDecimal(reader["montoCuota"]),
                            Interes = Convert.ToDecimal(reader["interes"]),
                            MontoConInteres = Convert.ToDecimal(reader["montoConInteres"]),
                            SaldoPendiente = Convert.ToDecimal(reader["saldoPendiente"]),
                            EstadoCuota = reader["estadoCuota"].ToString()
                        });
                    }
                }
            }

            ViewBag.Cuotas = cuotas;
            return View(cobro);
        }

        // POST /Cobros/PagarCuota
        [HttpPost]
        public IActionResult PagarCuota(int idCuota, decimal montoPago)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_PagarCuotaCobro", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id_cuota", idCuota);
                cmd.Parameters.AddWithValue("p_monto_pago", montoPago);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Alertas");
        }

        public static int ContarAlertasPendientes(ConexionDB conexion)
        {
            int count = 0;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"
            SELECT COUNT(*) FROM (
                SELECT IdCuota FROM cobrocuotas 
                WHERE estadoCuota != 'Pagada' AND estado = 1
                AND fechaVencimiento <= DATE_ADD(CURDATE(), INTERVAL 7 DAY)
                UNION ALL
                SELECT IdCuotas FROM cuotas 
                WHERE estadoCuota != 'Pagada' AND estado = 1
                AND fechaVencimiento <= DATE_ADD(CURDATE(), INTERVAL 7 DAY)
            ) t", con);
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return count;
        }
    }
}