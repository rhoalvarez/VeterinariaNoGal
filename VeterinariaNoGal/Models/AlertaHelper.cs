namespace VeterinariaNoGal.Models
{
    public static class AlertaHelper
    {
        public static int ContarAlertasPendientes()
        {
            int count = 0;
            ConexionDB conexion = new ConexionDB();
            using (MySql.Data.MySqlClient.MySqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(@"
                    SELECT COUNT(*) FROM (
                        SELECT IdCuota FROM cobrocuotas 
                        WHERE estadoCuota != 'Pagada' AND estado = 1
                        AND fechaVencimiento <= DATE_ADD(CURDATE(), INTERVAL 7 DAY)
                        UNION ALL
                        SELECT IdCuotas FROM cuotas 
                        WHERE estadoCuota != 'Pagada' AND estado = 1
                        AND fechaVencimiento <= DATE_ADD(CURDATE(), INTERVAL 7 DAY)
                    ) t", con);
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return count;
        }
    }
}