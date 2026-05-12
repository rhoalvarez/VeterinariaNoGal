using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;
using System.Text.Json;

namespace VeterinariaNoGal.Controllers
{
    public class VentaController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        public IActionResult Index()
        {
            List<Venta> lista = new List<Venta>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarVentas", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Venta
                    {
                        IdVentas = Convert.ToInt32(reader["IdVentas"]),
                        Fecha = DateTime.Now,
                        Total = Convert.ToDecimal(reader["total"] == DBNull.Value ? 0 : reader["total"]),
                        FormaPago = reader["formaPago"].ToString(),
                        EstadoPago = reader["estadoPago"].ToString(),
                        Observacion = reader["observacion"].ToString(),
                        NombreCliente = reader["nombre_cliente"].ToString()
                    });
                }
            }
            return View(lista);
        }

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
                        Nombre = reader["Nombre"].ToString()
                    });
                }
            }

            List<Producto> productos = new List<Producto>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarProductos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    productos.Add(new Producto
                    {
                        IdProductos = Convert.ToInt32(reader["IdProductos"]),
                        Nombre = reader["nombre"].ToString(),
                        PMinorista = Convert.ToDecimal(reader["p_minorista"] == DBNull.Value ? 0 : reader["p_minorista"]),
                        Stock = Convert.ToInt32(reader["stock"] == DBNull.Value ? 0 : reader["stock"])
                    });
                }
            }

            ViewBag.Clientes = clientes;
            ViewBag.Productos = productos;
            return View();
        }

        [HttpPost]
        public IActionResult GuardarVenta([FromBody] JsonElement data)
        {
            try
            {
                int idCliente = data.GetProperty("idCliente").GetInt32();
                string formaPago = data.GetProperty("formaPago").GetString();
                string observacion = data.GetProperty("observacion").GetString();
                int cantCuotas = data.GetProperty("cantCuotas").GetInt32();
                decimal interes = 0;
                DateTime? fechaPrimeraCuota = null;

                if (data.TryGetProperty("interes", out JsonElement interesEl))
                    interes = interesEl.GetDecimal();

                if (data.TryGetProperty("fechaPrimeraCuota", out JsonElement fechaEl) &&
                    !string.IsNullOrEmpty(fechaEl.GetString()))
                    fechaPrimeraCuota = DateTime.Parse(fechaEl.GetString());

                var items = data.GetProperty("items");

                decimal total = 0;
                foreach (var item in items.EnumerateArray())
                    total += item.GetProperty("subtotal").GetDecimal();

                int idVenta = 0;
                using (MySqlConnection con = conexion.ObtenerConexion())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_CrearVenta", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_id_cliente", idCliente);
                    cmd.Parameters.AddWithValue("p_total", total);
                    cmd.Parameters.AddWithValue("p_forma_pago", formaPago);
                    cmd.Parameters.AddWithValue("p_observacion", observacion ?? "");
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                        idVenta = Convert.ToInt32(reader["IdVenta"]);
                }

                foreach (var item in items.EnumerateArray())
                {
                    using (MySqlConnection con = conexion.ObtenerConexion())
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("sp_AgregarDetalleVenta", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_id_venta", idVenta);
                        cmd.Parameters.AddWithValue("p_id_producto", item.GetProperty("idProducto").GetInt32());
                        cmd.Parameters.AddWithValue("p_cantidad", item.GetProperty("cantidad").GetDecimal());
                        cmd.Parameters.AddWithValue("p_precio", item.GetProperty("precio").GetDecimal());
                        cmd.Parameters.AddWithValue("p_descuento", 0);
                        cmd.Parameters.AddWithValue("p_subtotal", item.GetProperty("subtotal").GetDecimal());
                        cmd.ExecuteNonQuery();
                    }
                }

                if (formaPago == "Cuotas" && cantCuotas > 0)
                {
                    decimal montoCuota = Math.Round(total / cantCuotas, 2);
                    decimal montoConInteres = Math.Round(montoCuota * (1 + interes / 100), 2);
                    DateTime fechaBase = fechaPrimeraCuota ?? DateTime.Today.AddMonths(1);

                    using (MySqlConnection con = conexion.ObtenerConexion())
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand(@"
                            INSERT INTO cuentascorrientes 
                            (IdClientes, IdVenta, fechaMovimiento, tipoMovimiento, importe, concepto, saldoAnterior, saldoNuevo, fechaVencimiento, estadoCuenta, estado)
                            VALUES (@idCliente, @idVenta, CURDATE(), 'Crédito', @total, 'Venta en cuotas', 0, @total, DATE_ADD(CURDATE(), INTERVAL @meses MONTH), 'Pendiente', 1);", con);
                        cmd.Parameters.AddWithValue("@idCliente", idCliente);
                        cmd.Parameters.AddWithValue("@idVenta", idVenta);
                        cmd.Parameters.AddWithValue("@total", total);
                        cmd.Parameters.AddWithValue("@meses", cantCuotas);
                        cmd.ExecuteNonQuery();
                    }

                    for (int i = 0; i < cantCuotas; i++)
                    {
                        DateTime fechaVenc = fechaBase.AddMonths(i);
                        using (MySqlConnection con = conexion.ObtenerConexion())
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand(@"
                                INSERT INTO cuotas 
                                (numeroCuota, fechaVencimiento, montoCuota, montoPagodo, saldoPendiente, IdVenta, estadoCuota, interesHora, estado)
                                VALUES (@numero, @fechaVenc, @monto, 0, @saldo, @idVenta, 'Pendiente', @interes, 1)", con);
                            cmd.Parameters.AddWithValue("@numero", i + 1);
                            cmd.Parameters.AddWithValue("@fechaVenc", fechaVenc);
                            cmd.Parameters.AddWithValue("@monto", montoConInteres);
                            cmd.Parameters.AddWithValue("@saldo", montoConInteres);
                            cmd.Parameters.AddWithValue("@idVenta", idVenta);
                            cmd.Parameters.AddWithValue("@interes", interes);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                return Json(new { success = true, idVenta });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public IActionResult Editar(int id)
        {
            Venta venta = null;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ObtenerVenta", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    venta = new Venta
                    {
                        IdVentas = Convert.ToInt32(reader["IdVentas"]),
                        IdCliente = Convert.ToInt32(reader["IdCliente"]),
                        FormaPago = reader["formaPago"].ToString(),
                        EstadoPago = reader["estadoPago"].ToString(),
                        Observacion = reader["observacion"].ToString(),
                        NombreCliente = reader["nombre_cliente"].ToString(),
                        Total = Convert.ToDecimal(reader["total"])
                    };
                }
            }
            return View(venta);
        }

        [HttpPost]
        public IActionResult Editar(Venta v)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ActualizarVenta", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", v.IdVentas);
                cmd.Parameters.AddWithValue("p_forma_pago", v.FormaPago);
                cmd.Parameters.AddWithValue("p_estado_pago", v.EstadoPago);
                cmd.Parameters.AddWithValue("p_observacion", v.Observacion ?? "");
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Detalle(int id)
        {
            Venta venta = null;
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ObtenerVenta", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    venta = new Venta
                    {
                        IdVentas = Convert.ToInt32(reader["IdVentas"]),
                        FormaPago = reader["formaPago"].ToString(),
                        EstadoPago = reader["estadoPago"].ToString(),
                        Observacion = reader["observacion"].ToString(),
                        NombreCliente = reader["nombre_cliente"].ToString(),
                        Total = Convert.ToDecimal(reader["total"])
                    };
                }
            }

            List<DetalleVenta> detalles = new List<DetalleVenta>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ObtenerDetalleVenta", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    detalles.Add(new DetalleVenta
                    {
                        NombreProducto = reader["nombre_producto"].ToString(),
                        Cantidad = Convert.ToDecimal(reader["cantidad"]),
                        PrecioUnitario = Convert.ToDecimal(reader["precioUnitario"]),
                        DescuentoItem = Convert.ToDecimal(reader["descuentoItem"]),
                        SubtotalItem = Convert.ToDecimal(reader["subtotalItem"])
                    });
                }
            }

            List<Cuota> cuotas = new List<Cuota>();
            if (venta != null && venta.FormaPago == "Cuotas")
            {
                using (MySqlConnection con = conexion.ObtenerConexion())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT * FROM cuotas WHERE IdVenta = @id AND estado = 1 ORDER BY numeroCuota", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cuotas.Add(new Cuota
                        {
                            NumeroCuota = Convert.ToInt32(reader["numeroCuota"]),
                            FechaVencimiento = Convert.ToDateTime(reader["fechaVencimiento"]),
                            MontoCuota = Convert.ToDecimal(reader["montoCuota"]),
                            SaldoPendiente = Convert.ToDecimal(reader["saldoPendiente"]),
                            EstadoCuota = reader["estadoCuota"].ToString(),
                            Interes = Convert.ToDecimal(reader["interesHora"]),
                            IdCuota = Convert.ToInt32(reader["IdCuotas"])
                        });
                    }
                }
            }

            ViewBag.Detalles = detalles;
            ViewBag.Cuotas = cuotas;
            return View(venta);
        }


        [HttpPost]
        public IActionResult PagarCuotaVenta(int idCuota, decimal montoPago)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_PagarCuotaVenta", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id_cuota", idCuota);
                cmd.Parameters.AddWithValue("p_monto_pago", montoPago);
                cmd.ExecuteNonQuery();
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}