using System;
using System.Windows.Forms;

namespace TallerTecnico
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Verificar conexión a la base de datos antes de iniciar
            if (!DatabaseConnection.TestConnection())
            {
                MessageBox.Show(
                    "No se pudo conectar a la base de datos.\n\n" +
                    "Por favor verifique:\n" +
                    "1. SQL Server está en ejecución\n" +
                    "2. La base de datos 'TallerTecnicoDB' existe\n" +
                    "3. La cadena de conexión en DatabaseConnection.cs es correcta\n\n" +
                    "La aplicación se cerrará.",
                    "Error de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Iniciar con el formulario de Login
            Application.Run(new FrmLogin());
        }
    }
}