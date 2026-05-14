using System.ComponentModel.DataAnnotations;

namespace VeterinariaNoGal.Models
{
    public class Proveedor
    {
        public int IdProveedores { get; set; }
        public string RazonSocial { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "El DNI/CUIT solo puede contener números.")]
        [StringLength(11, ErrorMessage = "El CUIT no puede superar los 11 dígitos.")]
        public string Dni { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "El teléfono solo puede contener números.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }

        public DateTime FechaAlta { get; set; }
        public int Estado { get; set; }
    }
}