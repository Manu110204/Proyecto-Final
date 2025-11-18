using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TallerTecnico
{
    public class DatabaseConnection
    {
        // CADENA DE CONEXIÓN – ADAPTADA A TU PC Y SQL
        private static string connectionString =
            "Data Source=MANUEL;Initial Catalog=TallerTecnicoDB;Integrated Security=True";

        // Obtener conexión
        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Ejecutar INSERT / UPDATE / DELETE
        public static bool ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    if (conn == null) return false;

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar consulta: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Ejecutar SELECT → retorna DataTable
        public static DataTable ExecuteQueryDataTable(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    if (conn == null) return null;

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar datos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Ejecutar consulta escalar (COUNT, SUM, etc.)
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    if (conn == null) return null;

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar consulta escalar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Probar conexión
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    return conn != null;
                }
            }
            catch
            {
                return false;
            }
        }
    }

    // DATOS DE LA SESIÓN DEL USUARIO
    public static class SessionData
    {
        public static int UsuarioID { get; set; }
        public static string NombreUsuario { get; set; }
        public static string NombreCompleto { get; set; }
        public static string Rol { get; set; }
        public static string Email { get; set; }

        public static bool IsAdmin()
        {
            return Rol == "Administrador";
        }

        public static bool IsTecnico()
        {
            return Rol == "Tecnico";
        }

        public static void Clear()
        {
            UsuarioID = 0;
            NombreUsuario = string.Empty;
            NombreCompleto = string.Empty;
            Rol = string.Empty;
            Email = string.Empty;
        }
    }
}
