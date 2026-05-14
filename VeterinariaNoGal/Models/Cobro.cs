namespace VeterinariaNoGal.Models
{
    public class Cobro
    {
        public int IdCobro { get; set; }
        public int IdCliente { get; set; }
        public string Concepto { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string TipoPrecio { get; set; }
        public string FormaPago { get; set; }
        public string EstadoPago { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public int Estado { get; set; }
        public string NombreCliente { get; set; }
        public string Telefono { get; set; }
    }

    public class CuotaCobro
    {
        public int IdCuota { get; set; }
        public int IdCobro { get; set; }
        public int NumeroCuota { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoCuota { get; set; }
        public decimal Interes { get; set; }
        public decimal MontoConInteres { get; set; }
        public DateTime? FechaPago { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string EstadoCuota { get; set; }
        public int Estado { get; set; }
        // Para mostrar en alertas
        public string Concepto { get; set; }
        public string NombreCliente { get; set; }
        public string Telefono { get; set; }
        public int IdCliente { get; set; }
        public string Alerta { get; set; }
        public string TipoCuota { get; set; }
    }

}

