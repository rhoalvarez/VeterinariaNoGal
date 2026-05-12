using MailKit.Net.Smtp;
using MimeKit;

namespace VeterinariaNoGal.Models
{
    public static class EmailService
    {
        private static string _gmailFrom = "alvarezrho@gmail.com";
        private static string _gmailPassword = "smvj yusw vybc ebbi";

        public static void EnviarAlertaCuotas(string emailDestino, List<string> cuotasPendientes)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Agroveterinaria El Nogal", _gmailFrom));
            message.To.Add(new MailboxAddress("Veterinario", emailDestino));
            message.Subject = "🔔 Alertas de Cuotas por Vencer - El Nogal";

            var body = "<h2 style='color:#2d5a27;'>🔔 Alertas de Cuotas</h2>";
            body += "<p>Las siguientes cuotas vencen en los próximos 3 días:</p>";
            body += "<table border='1' cellpadding='8' cellspacing='0' style='border-collapse:collapse; width:100%;'>";
            body += "<thead style='background:#2d5a27; color:white;'><tr><th>Detalle</th></tr></thead><tbody>";
            foreach (var cuota in cuotasPendientes)
                body += $"<tr><td>{cuota}</td></tr>";
            body += "</tbody></table>";
            body += "<br/><p style='color:#888; font-size:12px;'>Agroveterinaria El Nogal - Sistema de Gestión</p>";

            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_gmailFrom, _gmailPassword);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}