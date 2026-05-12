namespace VeterinariaNoGal.Models
{
    public class Proveedor
    {
        public int IdProveedores { get; set; }
        public string RazonSocial { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaAlta { get; set; }
        public int Estado { get; set; }
    }
}
