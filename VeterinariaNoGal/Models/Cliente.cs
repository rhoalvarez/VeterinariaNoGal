using System.ComponentModel.DataAnnotations;

namespace VeterinariaNoGal.Models
{
    public class Cliente
    {
        public int Id_Cliente { get; set; }

        public string Dni { get; set; }

        public string Apellido { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "El teléfono solo puede contener números.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }

        public DateTime FechaAlta { get; set; }

        public int Estado { get; set; }
    }
}