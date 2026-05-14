using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VeterinariaNoGal.Models;

namespace VeterinariaNoGal.Controllers
{
    public class ProductoController : Controller
    {
        private ConexionDB conexion = new ConexionDB();

        public IActionResult Index()
        {
            List<Producto> lista = new List<Producto>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarProductos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Producto
                    {
                        IdProductos = Convert.ToInt32(reader["IdProductos"]),
                        Codigo = reader["codigo"].ToString(),
                        Nombre = reader["nombre"].ToString(),
                        Stock = Convert.ToInt32(reader["stock"] == DBNull.Value ? 0 : reader["stock"]),
                        PMinorista = Convert.ToDecimal(reader["p_minorista"] == DBNull.Value ? 0 : reader["p_minorista"]),
                        PMayorista = Convert.ToDecimal(reader["p_mayorista"] == DBNull.Value ? 0 : reader["p_mayorista"]),
                        Descripcion = reader["descripcion"].ToString(),
                        Proveedor = reader["proveedor"].ToString(),
                        Imagen = reader["imagen"].ToString() // ← línea agregada
                    });
                }
            }
            return View(lista);
        }

        public IActionResult Crear()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_ListarProveedores", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    proveedores.Add(new Proveedor
                    {
                        IdProveedores = Convert.ToInt32(reader["IdProveedores"]),
                        RazonSocial = reader["razonSocial"].ToString()
                    });
                }
            }
            ViewBag.Proveedores = proveedores;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Producto p, IFormFile imagenFile)
        {
            string nombreImagen = "sin-imagen.png";

            if (imagenFile != null && imagenFile.Length > 0)
            {
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "productos");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string extension = Path.GetExtension(imagenFile.FileName);
                nombreImagen = Guid.NewGuid().ToString() + extension;
                string rutaCompleta = Path.Combine(carpeta, nombreImagen);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await imagenFile.CopyToAsync(stream);
                }
            }

            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_InsertarProducto", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_codigo", p.Codigo);
                cmd.Parameters.AddWithValue("p_nombre", p.Nombre);
                cmd.Parameters.AddWithValue("p_stock", p.Stock);
                cmd.Parameters.AddWithValue("p_precio_min", p.PMinorista);
                cmd.Parameters.AddWithValue("p_precio_may", p.PMayorista);
                cmd.Parameters.AddWithValue("p_descripcion", p.Descripcion);
                cmd.Parameters.AddWithValue("p_id_proveedor", p.IdProveedor);
                cmd.Parameters.AddWithValue("p_imagen", nombreImagen);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            using (MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EliminarProducto", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}