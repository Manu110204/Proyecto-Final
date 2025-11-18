using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TallerTecnico
{
    public partial class FrmLogin : Form
    {
        private Timer animationTimer;
        private int animationProgress = 0;
        private bool isLoading = false;

        public FrmLogin()
        {
            InitializeComponent();
            InitializeAnimations();
            ConfigureControls();
        }

        private void InitializeAnimations()
        {
            // Timer para animaciones
            animationTimer = new Timer();
            animationTimer.Interval = 15;
            animationTimer.Tick += AnimationTimer_Tick;

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

            // Efecto de entrada para los controles
            panelLogin.Top += 50;
            Timer slideTimer = new Timer();
            int targetTop = (this.Height - panelLogin.Height) / 2;
            slideTimer.Interval = 10;
            slideTimer.Tick += (s, e) =>
            {
                if (panelLogin.Top > targetTop)
                {
                    panelLogin.Top -= 3;
                }
                else
                {
                    panelLogin.Top = targetTop;
                    slideTimer.Stop();
                }
            };
            slideTimer.Start();
        }

        private void ConfigureControls()
        {
            // Configurar placeholders
            txtUsuario.Text = "Usuario";
            txtUsuario.ForeColor = Color.Gray;
            txtContrasena.Text = "Contraseña";
            txtContrasena.ForeColor = Color.Gray;
            txtContrasena.UseSystemPasswordChar = false;

            // Redondear botones
            RoundButton(btnLogin, 25);
            RoundButton(btnSalir, 25);

            // Redondear panel de login
            RoundPanel(panelLogin, 20);
            RoundPanel(panelLeft, 20);

            // Efectos hover en botones
            btnLogin.MouseEnter += (s, e) => BtnLogin_MouseEnter();
            btnLogin.MouseLeave += (s, e) => BtnLogin_MouseLeave();
            btnSalir.MouseEnter += (s, e) => BtnSalir_MouseEnter();
            btnSalir.MouseLeave += (s, e) => BtnSalir_MouseLeave();
        }

        private void RoundButton(Button btn, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            btn.Region = new Region(path);
        }

        private void RoundPanel(Panel panel, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);
            panel.Region = new Region(path);
        }

        private void BtnLogin_MouseEnter()
        {
            btnLogin.BackColor = Color.FromArgb(30, 136, 229);
        }

        private void BtnLogin_MouseLeave()
        {
            btnLogin.BackColor = Color.FromArgb(33, 150, 243);
        }

        private void BtnSalir_MouseEnter()
        {
            btnSalir.BackColor = Color.FromArgb(200, 200, 200);
        }

        private void BtnSalir_MouseLeave()
        {
            btnSalir.BackColor = Color.FromArgb(224, 224, 224);
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            animationProgress += 5;
            progressBar.Value = animationProgress;

            if (animationProgress >= 100)
            {
                animationTimer.Stop();
                animationProgress = 0;
                PerformLogin();
            }
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "Usuario")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.Black;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Text = "Usuario";
                txtUsuario.ForeColor = Color.Gray;
            }
        }

        private void txtContrasena_Enter(object sender, EventArgs e)
        {
            if (txtContrasena.Text == "Contraseña")
            {
                txtContrasena.Text = "";
                txtContrasena.ForeColor = Color.Black;
                txtContrasena.UseSystemPasswordChar = true;
            }
        }

        private void txtContrasena_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                txtContrasena.Text = "Contraseña";
                txtContrasena.ForeColor = Color.Gray;
                txtContrasena.UseSystemPasswordChar = false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "Usuario" || string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Por favor ingrese su usuario", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            if (txtContrasena.Text == "Contraseña" || string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Por favor ingrese su contraseña", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasena.Focus();
                return;
            }

            // Iniciar animación de carga
            isLoading = true;
            btnLogin.Enabled = false;
            btnSalir.Enabled = false;
            txtUsuario.Enabled = false;
            txtContrasena.Enabled = false;
            progressBar.Visible = true;
            lblLoading.Visible = true;
            animationTimer.Start();
        }

        private void PerformLogin()
        {
            string usuario = txtUsuario.Text;
            string contrasena = txtContrasena.Text;

            string query = "SELECT UsuarioID, NombreCompleto, Rol, Email FROM Usuarios " +
                          "WHERE NombreUsuario = @Usuario AND Contrasena = @Contrasena AND Activo = 1";

            SqlParameter[] parameters = {
                new SqlParameter("@Usuario", usuario),
                new SqlParameter("@Contrasena", contrasena)
            };

            var dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                // Guardar datos de sesión
                SessionData.UsuarioID = Convert.ToInt32(dt.Rows[0]["UsuarioID"]);
                SessionData.NombreUsuario = usuario;
                SessionData.NombreCompleto = dt.Rows[0]["NombreCompleto"].ToString();
                SessionData.Rol = dt.Rows[0]["Rol"].ToString();
                SessionData.Email = dt.Rows[0]["Email"].ToString();

                // Efecto fade out
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 20;
                fadeOutTimer.Tick += (s, e) =>
                {
                    if (this.Opacity > 0)
                        this.Opacity -= 0.1;
                    else
                    {
                        fadeOutTimer.Stop();
                        // Abrir dashboard
                        FrmDashboard dashboard = new FrmDashboard();
                        dashboard.Show();
                        this.Hide();
                    }
                };
                fadeOutTimer.Start();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos", "Error de autenticación",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Restaurar controles
                btnLogin.Enabled = true;
                btnSalir.Enabled = true;
                txtUsuario.Enabled = true;
                txtContrasena.Enabled = true;
                progressBar.Visible = false;
                lblLoading.Visible = false;
                progressBar.Value = 0;
                txtContrasena.Clear();
                txtContrasena.Focus();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtContrasena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}