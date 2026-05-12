namespace VeterinariaNoGal.Models
{
    public class Turno
    {
        public int IdTurno { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraTurno { get; set; }
        public string EstadoTurno { get; set; }
        public string Motivo { get; set; }
        public string Observacion { get; set; }
        public int Id_Mascota { get; set; }
        public int Estado { get; set; }

        // Para mostrar en la vista
        public string NombreMascota { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
    }
}
