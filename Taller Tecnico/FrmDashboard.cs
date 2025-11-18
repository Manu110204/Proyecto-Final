using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;

namespace TallerTecnico
{
    public partial class FrmDashboard : Form
    {
        private Form activeForm = null;

        public FrmDashboard()
        {
            InitializeComponent();
            ConfigureByRole();
            LoadDashboardData();
            InitializeAnimations();
        }

        private void InitializeAnimations()
        {
            // Animación de entrada
            this.Opacity = 0;
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 20;
            fadeInTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    fadeInTimer.Stop();
            };
            fadeInTimer.Start();
        }

        private void ConfigureByRole()
        {
            lblBienvenida.Text = $"Bienvenido, {SessionData.NombreCompleto}";
            lblRol.Text = $"Rol: {SessionData.Rol}";

            // Configurar menú según rol
            if (SessionData.IsTecnico())
            {
                // Técnicos NO tienen acceso a:
                btnClientes.Enabled = false;
                btnClientes.BackColor = Color.FromArgb(180, 180, 180);

                btnFacturacion.Enabled = false;
                btnFacturacion.BackColor = Color.FromArgb(180, 180, 180);

                btnUsuarios.Visible = false;

                // Solo lectura en inventario
                btnInventario.Text = "📦 Inventario\n(Solo Lectura)";
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                // Total de clientes
                string queryClientes = "SELECT COUNT(*) FROM Clientes WHERE Activo = 1";
                object resultClientes = DatabaseConnection.ExecuteScalar(queryClientes);
                lblTotalClientes.Text = resultClientes != null ? resultClientes.ToString() : "0";

                // Servicios activos
                string queryServicios = "SELECT COUNT(*) FROM Servicios WHERE Estado IN ('Recibido', 'En Diagnostico', 'En Reparacion')";
                object resultServicios = DatabaseConnection.ExecuteScalar(queryServicios);
                lblServiciosActivos.Text = resultServicios != null ? resultServicios.ToString() : "0";

                // Citas pendientes
                string queryCitas = "SELECT COUNT(*) FROM Citas WHERE Estado = 'Pendiente' AND FechaCita >= CAST(GETDATE() AS DATE)";
                object resultCitas = DatabaseConnection.ExecuteScalar(queryCitas);
                lblCitasPendientes.Text = resultCitas != null ? resultCitas.ToString() : "0";

                // Repuestos con stock bajo
                string queryRepuestos = "SELECT COUNT(*) FROM Repuestos WHERE StockActual <= StockMinimo AND Activo = 1";
                object resultRepuestos = DatabaseConnection.ExecuteScalar(queryRepuestos);
                lblStockBajo.Text = resultRepuestos != null ? resultRepuestos.ToString() : "0";

                // Cargar citas del día
                LoadCitasHoy();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void LoadCitasHoy()
        {
            string query = @"SELECT 
                                ci.FechaCita,
                                ci.HoraCita,
                                c.NombreCompleto AS Cliente,
                                e.TipoEquipo + ' ' + e.Marca + ' ' + e.Modelo AS Equipo,
                                ci.Estado
                            FROM Citas ci
                            INNER JOIN Clientes c ON ci.ClienteID = c.ClienteID
                            INNER JOIN Equipos e ON ci.EquipoID = e.EquipoID
                            WHERE CAST(ci.FechaCita AS DATE) = CAST(GETDATE() AS DATE)
                            ORDER BY ci.HoraCita";

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);
            if (dt != null)
            {
                dgvCitasHoy.DataSource = dt;
                dgvCitasHoy.Columns["FechaCita"].HeaderText = "Fecha";
                dgvCitasHoy.Columns["HoraCita"].HeaderText = "Hora";
                dgvCitasHoy.Columns["Cliente"].HeaderText = "Cliente";
                dgvCitasHoy.Columns["Equipo"].HeaderText = "Equipo";
                dgvCitasHoy.Columns["Estado"].HeaderText = "Estado";
            }
        }

        private void OpenFormInPanel(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelContenedor.Controls.Add(childForm);
            panelContenedor.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new FrmClientes());
        }

        private void btnEquipos_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new FrmEquipos());
        }

        private void btnCitas_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new FrmCitas());
        }

        private void btnServicios_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new FrmServicios());
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new FrmInventario());
        }

        private void btnFacturacion_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new FrmFacturacion());
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new FrmUsuarios());
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }
            LoadDashboardData();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SessionData.Clear();

                // Efecto fade out
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 20;
                fadeOutTimer.Tick += (s, ev) =>
                {
                    if (this.Opacity > 0)
                        this.Opacity -= 0.1;
                    else
                    {
                        fadeOutTimer.Stop();
                        FrmLogin login = new FrmLogin();
                        login.Show();
                        this.Close();
                    }
                };
                fadeOutTimer.Start();
            }
        }

        private void FrmDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Efectos hover para botones del menú
        private void MenuButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Enabled)
            {
                btn.BackColor = Color.FromArgb(30, 136, 229);
            }
        }

        private void MenuButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Enabled)
            {
                btn.BackColor = Color.FromArgb(33, 150, 243);
            }
        }
    }
}
