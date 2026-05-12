namespace VeterinariaNoGal.Models
{
    public class Historial
    {
        public int IdHistorial { get; set; }
        public int IdMascota { get; set; }
        public int IdTurno { get; set; }
        public string MotivoConsulta { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public string Indicaciones { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaConsulta { get; set; }
        public DateTime ProximoControl { get; set; }
        public int Estado { get; set; }

        // Para mostrar
        public string NombreMascota { get; set; }
    }
}