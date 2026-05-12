namespace VeterinariaNoGal.Models
{
    public class Mascota
    {
        public int Id_Mascota { get; set; }
        public string Nombre { get; set; }
        public int Id_Cliente { get; set; }
        public int Id_Especie { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Estado { get; set; }

        // Para mostrar en la vista
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string Especie { get; set; }
    }
}