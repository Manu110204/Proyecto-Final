namespace TallerTecnico
{
    partial class FrmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.btnUsuarios = new System.Windows.Forms.Button();
            this.btnFacturacion = new System.Windows.Forms.Button();
            this.btnInventario = new System.Windows.Forms.Button();
            this.btnServicios = new System.Windows.Forms.Button();
            this.btnCitas = new System.Windows.Forms.Button();
            this.btnEquipos = new System.Windows.Forms.Button();
            this.btnClientes = new System.Windows.Forms.Button();
            this.btnInicio = new System.Windows.Forms.Button();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.lblRol = new System.Windows.Forms.Label();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelContenedor = new System.Windows.Forms.Panel();
            this.panelDashboard = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvCitasHoy = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelCard4 = new System.Windows.Forms.Panel();
            this.lblStockBajo = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panelCard3 = new System.Windows.Forms.Panel();
            this.lblCitasPendientes = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panelCard2 = new System.Windows.Forms.Panel();
            this.lblServiciosActivos = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelCard1 = new System.Windows.Forms.Panel();
            this.lblTotalClientes = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelMenu.SuspendLayout();
            this.panelLogo.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelContenedor.SuspendLayout();
            this.panelDashboard.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitasHoy)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panelCard4.SuspendLayout();
            this.panelCard3.SuspendLayout();
            this.panelCard2.SuspendLayout();
            this.panelCard1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(51)))), ((int)(((byte)(64)))));
            this.panelMenu.Controls.Add(this.btnCerrarSesion);
            this.panelMenu.Controls.Add(this.btnUsuarios);
            this.panelMenu.Controls.Add(this.btnFacturacion);
            this.panelMenu.Controls.Add(this.btnInventario);
            this.panelMenu.Controls.Add(this.btnServicios);
            this.panelMenu.Controls.Add(this.btnCitas);
            this.panelMenu.Controls.Add(this.btnEquipos);
            this.panelMenu.Controls.Add(this.btnClientes);
            this.panelMenu.Controls.Add(this.btnInicio);
            this.panelMenu.Controls.Add(this.panelLogo);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(250, 788);
            this.panelMenu.TabIndex = 0;
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarSesion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCerrarSesion.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.White;
            this.btnCerrarSesion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesion.Location = new System.Drawing.Point(0, 728);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnCerrarSesion.Size = new System.Drawing.Size(250, 60);
            this.btnCerrarSesion.TabIndex = 9;
            this.btnCerrarSesion.Text = "  Cerrar Sesión";
            this.btnCerrarSesion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // btnUsuarios
            // 
            this.btnUsuarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUsuarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUsuarios.FlatAppearance.BorderSize = 0;
            this.btnUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsuarios.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUsuarios.ForeColor = System.Drawing.Color.White;
            this.btnUsuarios.Location = new System.Drawing.Point(0, 580);
            this.btnUsuarios.Name = "btnUsuarios";
            this.btnUsuarios.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnUsuarios.Size = new System.Drawing.Size(250, 60);
            this.btnUsuarios.TabIndex = 8;
            this.btnUsuarios.Text = "👥 Usuarios";
            this.btnUsuarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsuarios.UseVisualStyleBackColor = false;
            this.btnUsuarios.Click += new System.EventHandler(this.btnUsuarios_Click);
            this.btnUsuarios.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnUsuarios.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnFacturacion
            // 
            this.btnFacturacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnFacturacion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFacturacion.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnFacturacion.FlatAppearance.BorderSize = 0;
            this.btnFacturacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFacturacion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFacturacion.ForeColor = System.Drawing.Color.White;
            this.btnFacturacion.Location = new System.Drawing.Point(0, 520);
            this.btnFacturacion.Name = "btnFacturacion";
            this.btnFacturacion.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnFacturacion.Size = new System.Drawing.Size(250, 60);
            this.btnFacturacion.TabIndex = 7;
            this.btnFacturacion.Text = "💰 Facturación";
            this.btnFacturacion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFacturacion.UseVisualStyleBackColor = false;
            this.btnFacturacion.Click += new System.EventHandler(this.btnFacturacion_Click);
            this.btnFacturacion.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnFacturacion.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnInventario
            // 
            this.btnInventario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnInventario.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInventario.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnInventario.FlatAppearance.BorderSize = 0;
            this.btnInventario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInventario.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnInventario.ForeColor = System.Drawing.Color.White;
            this.btnInventario.Location = new System.Drawing.Point(0, 460);
            this.btnInventario.Name = "btnInventario";
            this.btnInventario.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnInventario.Size = new System.Drawing.Size(250, 60);
            this.btnInventario.TabIndex = 6;
            this.btnInventario.Text = "📦 Inventario";
            this.btnInventario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInventario.UseVisualStyleBackColor = false;
            this.btnInventario.Click += new System.EventHandler(this.btnInventario_Click);
            this.btnInventario.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnInventario.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnServicios
            // 
            this.btnServicios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnServicios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnServicios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnServicios.FlatAppearance.BorderSize = 0;
            this.btnServicios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServicios.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnServicios.ForeColor = System.Drawing.Color.White;
            this.btnServicios.Location = new System.Drawing.Point(0, 400);
            this.btnServicios.Name = "btnServicios";
            this.btnServicios.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnServicios.Size = new System.Drawing.Size(250, 60);
            this.btnServicios.TabIndex = 5;
            this.btnServicios.Text = "🔧 Servicios";
            this.btnServicios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnServicios.UseVisualStyleBackColor = false;
            this.btnServicios.Click += new System.EventHandler(this.btnServicios_Click);
            this.btnServicios.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnServicios.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnCitas
            // 
            this.btnCitas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnCitas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCitas.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCitas.FlatAppearance.BorderSize = 0;
            this.btnCitas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCitas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCitas.ForeColor = System.Drawing.Color.White;
            this.btnCitas.Location = new System.Drawing.Point(0, 340);
            this.btnCitas.Name = "btnCitas";
            this.btnCitas.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnCitas.Size = new System.Drawing.Size(250, 60);
            this.btnCitas.TabIndex = 4;
            this.btnCitas.Text = "📅 Citas";
            this.btnCitas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCitas.UseVisualStyleBackColor = false;
            this.btnCitas.Click += new System.EventHandler(this.btnCitas_Click);
            this.btnCitas.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnCitas.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnEquipos
            // 
            this.btnEquipos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnEquipos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEquipos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEquipos.FlatAppearance.BorderSize = 0;
            this.btnEquipos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEquipos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEquipos.ForeColor = System.Drawing.Color.White;
            this.btnEquipos.Location = new System.Drawing.Point(0, 280);
            this.btnEquipos.Name = "btnEquipos";
            this.btnEquipos.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnEquipos.Size = new System.Drawing.Size(250, 60);
            this.btnEquipos.TabIndex = 3;
            this.btnEquipos.Text = "💻 Equipos";
            this.btnEquipos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEquipos.UseVisualStyleBackColor = false;
            this.btnEquipos.Click += new System.EventHandler(this.btnEquipos_Click);
            this.btnEquipos.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnEquipos.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnClientes
            // 
            this.btnClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnClientes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClientes.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnClientes.FlatAppearance.BorderSize = 0;
            this.btnClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClientes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClientes.ForeColor = System.Drawing.Color.White;
            this.btnClientes.Location = new System.Drawing.Point(0, 220);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnClientes.Size = new System.Drawing.Size(250, 60);
            this.btnClientes.TabIndex = 2;
            this.btnClientes.Text = "👤 Clientes";
            this.btnClientes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClientes.UseVisualStyleBackColor = false;
            this.btnClientes.Click += new System.EventHandler(this.btnClientes_Click);
            this.btnClientes.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnClientes.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnInicio
            // 
            this.btnInicio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnInicio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInicio.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnInicio.FlatAppearance.BorderSize = 0;
            this.btnInicio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInicio.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnInicio.ForeColor = System.Drawing.Color.White;
            this.btnInicio.Location = new System.Drawing.Point(0, 160);
            this.btnInicio.Name = "btnInicio";
            this.btnInicio.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnInicio.Size = new System.Drawing.Size(250, 60);
            this.btnInicio.TabIndex = 1;
            this.btnInicio.Text = "🏠 Inicio";
            this.btnInicio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInicio.UseVisualStyleBackColor = false;
            this.btnInicio.Click += new System.EventHandler(this.btnInicio_Click);
            this.btnInicio.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnInicio.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // panelLogo
            // 
            this.panelLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.panelLogo.Controls.Add(this.lblRol);
            this.panelLogo.Controls.Add(this.lblBienvenida);
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogo.Location = new System.Drawing.Point(0, 0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(250, 160);
            this.panelLogo.TabIndex = 0;
            // 
            // lblRol
            // 
            this.lblRol.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRol.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRol.ForeColor = System.Drawing.Color.White;
            this.lblRol.Location = new System.Drawing.Point(0, 130);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(250, 30);
            this.lblRol.TabIndex = 1;
            this.lblRol.Text = "Rol: Administrador";
            this.lblRol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.ForeColor = System.Drawing.Color.White;
            this.lblBienvenida.Location = new System.Drawing.Point(0, 0);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Padding = new System.Windows.Forms.Padding(10);
            this.lblBienvenida.Size = new System.Drawing.Size(250, 120);
            this.lblBienvenida.TabIndex = 0;
            this.lblBienvenida.Text = "Bienvenido";
            this.lblBienvenida.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(250, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1136, 80);
            this.panelTop.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.label1.Location = new System.Drawing.Point(30, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(561, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "SISTEMA DE TALLER DE SERVICIO TÉCNICO";
            // 
            // panelContenedor
            // 
            this.panelContenedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelContenedor.Controls.Add(this.panelDashboard);
            this.panelContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenedor.Location = new System.Drawing.Point(250, 80);
            this.panelContenedor.Name = "panelContenedor";
            this.panelContenedor.Padding = new System.Windows.Forms.Padding(20);
            this.panelContenedor.Size = new System.Drawing.Size(1136, 708);
            this.panelContenedor.TabIndex = 2;
            // 
            // panelDashboard
            // 
            this.panelDashboard.BackColor = System.Drawing.Color.White;
            this.panelDashboard.Controls.Add(this.groupBox2);
            this.panelDashboard.Controls.Add(this.groupBox1);
            this.panelDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDashboard.Location = new System.Drawing.Point(20, 20);
            this.panelDashboard.Name = "panelDashboard";
            this.panelDashboard.Padding = new System.Windows.Forms.Padding(20);
            this.panelDashboard.Size = new System.Drawing.Size(1096, 668);
            this.panelDashboard.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvCitasHoy);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(20, 200);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(1056, 448);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "📅 Citas de Hoy";
            // 
            // dgvCitasHoy
            // 
            this.dgvCitasHoy.AllowUserToAddRows = false;
            this.dgvCitasHoy.AllowUserToDeleteRows = false;
            this.dgvCitasHoy.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCitasHoy.BackgroundColor = System.Drawing.Color.White;
            this.dgvCitasHoy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCitasHoy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCitasHoy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCitasHoy.Location = new System.Drawing.Point(10, 30);
            this.dgvCitasHoy.Name = "dgvCitasHoy";
            this.dgvCitasHoy.ReadOnly = true;
            this.dgvCitasHoy.RowHeadersVisible = false;
            this.dgvCitasHoy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCitasHoy.Size = new System.Drawing.Size(1036, 408);
            this.dgvCitasHoy.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelCard4);
            this.groupBox1.Controls.Add(this.panelCard3);
            this.groupBox1.Controls.Add(this.panelCard2);
            this.groupBox1.Controls.Add(this.panelCard1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(1056, 180);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "📊 Resumen General";
            // 
            // panelCard4
            // 
            this.panelCard4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.panelCard4.Controls.Add(this.lblStockBajo);
            this.panelCard4.Controls.Add(this.label9);
            this.panelCard4.Location = new System.Drawing.Point(810, 40);
            this.panelCard4.Name = "panelCard4";
            this.panelCard4.Size = new System.Drawing.Size(240, 120);
            this.panelCard4.TabIndex = 3;
            // 
            // lblStockBajo
            // 
            this.lblStockBajo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStockBajo.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            this.lblStockBajo.ForeColor = System.Drawing.Color.White;
            this.lblStockBajo.Location = new System.Drawing.Point(0, 40);
            this.lblStockBajo.Name = "lblStockBajo";
            this.lblStockBajo.Size = new System.Drawing.Size(240, 80);
            this.lblStockBajo.TabIndex = 1;
            this.lblStockBajo.Text = "0";
            this.lblStockBajo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(240, 40);
            this.label9.TabIndex = 0;
            this.label9.Text = "Repuestos Stock Bajo";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCard3
            // 
            this.panelCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.panelCard3.Controls.Add(this.lblCitasPendientes);
            this.panelCard3.Controls.Add(this.label7);
            this.panelCard3.Location = new System.Drawing.Point(545, 40);
            this.panelCard3.Name = "panelCard3";
            this.panelCard3.Size = new System.Drawing.Size(240, 120);
            this.panelCard3.TabIndex = 2;
            // 
            // lblCitasPendientes
            // 
            this.lblCitasPendientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCitasPendientes.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            this.lblCitasPendientes.ForeColor = System.Drawing.Color.White;
            this.lblCitasPendientes.Location = new System.Drawing.Point(0, 40);
            this.lblCitasPendientes.Name = "lblCitasPendientes";
            this.lblCitasPendientes.Size = new System.Drawing.Size(240, 80);
            this.lblCitasPendientes.TabIndex = 1;
            this.lblCitasPendientes.Text = "0";
            this.lblCitasPendientes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(240, 40);
            this.label7.TabIndex = 0;
            this.label7.Text = "Citas Pendientes";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCard2
            // 
            this.panelCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(39)))), ((int)(((byte)(176)))));
            this.panelCard2.Controls.Add(this.lblServiciosActivos);
            this.panelCard2.Controls.Add(this.label5);
            this.panelCard2.Location = new System.Drawing.Point(280, 40);
            this.panelCard2.Name = "panelCard2";
            this.panelCard2.Size = new System.Drawing.Size(240, 120);
            this.panelCard2.TabIndex = 1;
            // 
            // lblServiciosActivos
            // 
            this.lblServiciosActivos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServiciosActivos.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            this.lblServiciosActivos.ForeColor = System.Drawing.Color.White;
            this.lblServiciosActivos.Location = new System.Drawing.Point(0, 40);
            this.lblServiciosActivos.Name = "lblServiciosActivos";
            this.lblServiciosActivos.Size = new System.Drawing.Size(240, 80);
            this.lblServiciosActivos.TabIndex = 1;
            this.lblServiciosActivos.Text = "0";
            this.lblServiciosActivos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(240, 40);
            this.label5.TabIndex = 0;
            this.label5.Text = "Servicios Activos";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCard1
            // 
            this.panelCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.panelCard1.Controls.Add(this.lblTotalClientes);
            this.panelCard1.Controls.Add(this.label2);
            this.panelCard1.Location = new System.Drawing.Point(15, 40);
            this.panelCard1.Name = "panelCard1";
            this.panelCard1.Size = new System.Drawing.Size(240, 120);
            this.panelCard1.TabIndex = 0;
            // 
            // lblTotalClientes
            // 
            this.lblTotalClientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalClientes.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            this.lblTotalClientes.ForeColor = System.Drawing.Color.White;
            this.lblTotalClientes.Location = new System.Drawing.Point(0, 40);
            this.lblTotalClientes.Name = "lblTotalClientes";
            this.lblTotalClientes.Size = new System.Drawing.Size(240, 80);
            this.lblTotalClientes.TabIndex = 1;
            this.lblTotalClientes.Text = "0";
            this.lblTotalClientes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total Clientes";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1386, 788);
            this.Controls.Add(this.panelContenedor);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard - Taller Técnico";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDashboard_FormClosing);
            this.panelMenu.ResumeLayout(false);
            this.panelLogo.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelContenedor.ResumeLayout(false);
            this.panelDashboard.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitasHoy)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panelCard4.ResumeLayout(false);
            this.panelCard3.ResumeLayout(false);
            this.panelCard2.ResumeLayout(false);
            this.panelCard1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.Button btnInicio;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnEquipos;
        private System.Windows.Forms.Button btnCitas;
        private System.Windows.Forms.Button btnServicios;
        private System.Windows.Forms.Button btnInventario;
        private System.Windows.Forms.Button btnFacturacion;
        private System.Windows.Forms.Button btnUsuarios;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelContenedor;
        private System.Windows.Forms.Panel panelDashboard;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelCard1;
        private System.Windows.Forms.Label lblTotalClientes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelCard2;
        private System.Windows.Forms.Label lblServiciosActivos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panelCard3;
        private System.Windows.Forms.Label lblCitasPendientes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panelCard4;
        private System.Windows.Forms.Label lblStockBajo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvCitasHoy;
    }
}