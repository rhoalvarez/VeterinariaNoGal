namespace VeterinariaNoGal.Models
{
    public class Producto
    {
        public int IdProductos { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public decimal PMinorista { get; set; }
        public decimal PMayorista { get; set; }
        public string Descripcion { get; set; }
        public int IdProveedor { get; set; }
        public int Estado { get; set; }

        // Para mostrar
        public string Proveedor { get; set; }
    }
}
