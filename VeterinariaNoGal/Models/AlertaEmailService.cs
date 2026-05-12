namespace VeterinariaNoGal.Models
{
    public class AlertaEmailService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private string _emailDestino = "alvarezrho@gmail.com";

        public AlertaEmailService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var ahora = DateTime.Now;
                    var proximaEjecucion = DateTime.Today.AddHours(8);
                    if (ahora > proximaEjecucion)
                        proximaEjecucion = proximaEjecucion.AddDays(1);

                    var espera = proximaEjecucion - ahora;
                    await Task.Delay(espera, stoppingToken);

                    EnviarAlertas();
                }
                catch { }
            }
        }

        private void EnviarAlertas()
        {
            var conexion = new ConexionDB();
            var cuotasPendientes = new List<string>();

            using (var con = conexion.ObtenerConexion())
            {
                con.Open();
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(@"
                    SELECT c.Nombre, cc.numeroCuota, cc.fechaVencimiento, cc.saldoPendiente, co.concepto
                    FROM cobrocuotas cc
                    JOIN cobros co ON cc.IdCobro = co.IdCobro
                    JOIN clientes c ON co.IdCliente = c.Id_Cliente
                    WHERE cc.estadoCuota != 'Pagada' AND cc.estado = 1
                    AND cc.fechaVencimiento BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL 3 DAY)
                    UNION ALL
                    SELECT c.Nombre, cu.numeroCuota, cu.fechaVencimiento, cu.saldoPendiente, 'Venta de productos'
                    FROM cuotas cu
                    JOIN ventas v ON cu.IdVenta = v.IdVentas
                    JOIN clientes c ON v.IdCliente = c.Id_Cliente
                    WHERE cu.estadoCuota != 'Pagada' AND cu.estado = 1
                    AND cu.fechaVencimiento BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL 3 DAY)", con);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cuotasPendientes.Add($"<b>{reader["Nombre"]}</b> — {reader["concepto"]} — Cuota {reader["numeroCuota"]} — Vence: {Convert.ToDateTime(reader["fechaVencimiento"]).ToString("dd/MM/yyyy")} — Saldo: ${Convert.ToDecimal(reader["saldoPendiente"]).ToString("N2")}");
                }
            }

            if (cuotasPendientes.Any())
                EmailService.EnviarAlertaCuotas(_emailDestino, cuotasPendientes);
        }
    }
}
