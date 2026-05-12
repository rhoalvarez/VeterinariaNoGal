namespace VeterinariaNoGal.Models
{
    public class Alerta
    {
        public int Id_Cliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public int IdCuentasC { get; set; }
        public decimal Importe { get; set; }
        public decimal SaldoNuevo { get; set; }
        public DateTime VencimientoCuenta { get; set; }
        public int IdCuotas { get; set; }
        public int NumeroCuota { get; set; }
        public decimal MontoCuota { get; set; }
        public decimal SaldoPendiente { get; set; }
        public DateTime VencimientoCuota { get; set; }
        public string EstadoCuota { get; set; }
        public string Alerta_ { get; set; }
    }
}