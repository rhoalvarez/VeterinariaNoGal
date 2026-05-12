namespace VeterinariaNoGal.Models
{
    public class Cliente
    {
        public int Id_Cliente { get; set; }
        public string Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaAlta { get; set; }
        public int Estado { get; set; }
    }
}