namespace VeterinariaNoGal.Models
{
    public class Venta
    {
        public int IdVentas { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string FormaPago { get; set; }
        public string EstadoPago { get; set; }
        public string Observacion { get; set; }
        public int Estado { get; set; }

        // Para mostrar
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
    }

    public class DetalleVenta
    {
        public int IdVentasDetalles { get; set; }
        public int IdVentas { get; set; }
        public int IdProductos { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal DescuentoItem { get; set; }
        public decimal SubtotalItem { get; set; }

        // Para mostrar
        public string NombreProducto { get; set; }
    }

    public class CarritoItem
    {
        public int IdProductos { get; set; }
        public string NombreProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class Cuota
    {
        public int IdCuota { get; set; }
        public int NumeroCuota { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoCuota { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string EstadoCuota { get; set; }
        public decimal Interes { get; set; }
    }
}