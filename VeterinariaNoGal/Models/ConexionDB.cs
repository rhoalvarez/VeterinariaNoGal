using MySql.Data.MySqlClient;

namespace VeterinariaNoGal.Models
{
    public class ConexionDB
    {
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=bd_veterinarianogal;Uid=root;Pwd=;Connect Timeout=30;";

        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(connectionString);
        }
    }
}