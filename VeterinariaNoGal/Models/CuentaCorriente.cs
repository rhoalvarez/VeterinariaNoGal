namespace VeterinariaNoGal.Models
{
    public class CuentaCorriente
    {
        public string Tipo { get; set; }
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public string FormaPago { get; set; }
        public string EstadoPago { get; set; }
        public int TotalCuotas { get; set; }
        public decimal SaldoTotal { get; set; }
    }
}