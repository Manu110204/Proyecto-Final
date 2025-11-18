using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace TallerTecnico
{
    public partial class FrmServicios : Form
    {
        private int servicioSeleccionadoID = 0;
        private bool isEditing = false;

        public FrmServicios()
        {
            InitializeComponent();
            LoadServicios();
            LoadClientes();
            LoadTecnicos();
            ConfigureDataGridView();
            ConfigureByRole();
        }

        private void ConfigureByRole()
        {
            if (SessionData.IsTecnico())
            {
                string query = @"SELECT s.*, c.NombreCompleto AS Cliente, 
                                e.TipoEquipo + ' ' + e.Marca + ' ' + e.Modelo AS Equipo,
                                u.NombreCompleto AS Tecnico
                                FROM Servicios s
                                INNER JOIN Clientes c ON s.ClienteID = c.ClienteID
                                INNER JOIN Equipos e ON s.EquipoID = e.EquipoID
                                INNER JOIN Usuarios u ON s.TecnicoAsignado = u.UsuarioID
                                WHERE s.TecnicoAsignado = @TecnicoID
                                ORDER BY s.FechaIngreso DESC";

                SqlParameter[] parameters = { new SqlParameter("@TecnicoID", SessionData.UsuarioID) };
                DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

                if (dt != null) dgvServicios.DataSource = dt;
            }
        }

        private void ConfigureDataGridView()
        {
            dgvServicios.EnableHeadersVisualStyles = false;
            dgvServicios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvServicios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvServicios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvServicios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void LoadClientes()
        {
            string query = "SELECT ClienteID, NombreCompleto FROM Clientes WHERE Activo = 1 ORDER BY NombreCompleto";
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);

            if (dt != null)
            {
                cmbCliente.DataSource = dt;
                cmbCliente.DisplayMember = "NombreCompleto";
                cmbCliente.ValueMember = "ClienteID";
                cmbCliente.SelectedIndex = -1;
            }
        }

        private void LoadTecnicos()
        {
            string query = "SELECT UsuarioID, NombreCompleto FROM Usuarios WHERE Rol = 'Tecnico' AND Activo = 1 ORDER BY NombreCompleto";
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);

            if (dt != null)
            {
                cmbTecnico.DataSource = dt;
                cmbTecnico.DisplayMember = "NombreCompleto";
                cmbTecnico.ValueMember = "UsuarioID";
                cmbTecnico.SelectedIndex = -1;
            }
        }

        private void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            // *** CORREGIDO ***
            if (cmbCliente.SelectedValue == null || cmbCliente.SelectedValue.ToString() == "")
                return;

            if (!int.TryParse(cmbCliente.SelectedValue.ToString(), out int clienteID))
                return;

            string query = @"SELECT EquipoID, 
                            TipoEquipo + ' ' + Marca + ' ' + Modelo AS Equipo
                            FROM Equipos 
                            WHERE ClienteID = @ClienteID
                            ORDER BY FechaRegistro DESC";

            SqlParameter[] parameters = { new SqlParameter("@ClienteID", clienteID) };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null)
            {
                cmbEquipo.DataSource = dt;
                cmbEquipo.DisplayMember = "Equipo";
                cmbEquipo.ValueMember = "EquipoID";
                cmbEquipo.SelectedIndex = -1;
            }
        }

        private void LoadServicios()
        {
            string query = @"SELECT 
                    s.ServicioID,
                    s.FechaIngreso,
                    c.NombreCompleto AS Cliente,
                    e.TipoEquipo + ' ' + e.Marca + ' ' + e.Modelo AS Equipo,
                    s.Estado,
                    u.NombreCompleto AS Tecnico,
                    s.CostoTotal,
                    s.FechaEntregaEstimada
                    FROM Servicios s
                    INNER JOIN Clientes c ON s.ClienteID = c.ClienteID
                    INNER JOIN Equipos e ON s.EquipoID = e.EquipoID
                    INNER JOIN Usuarios u ON s.TecnicoAsignado = u.UsuarioID
                    ORDER BY s.FechaIngreso DESC";

            if (SessionData.IsTecnico())
            {
                query = @"SELECT 
                s.ServicioID,
                s.FechaIngreso,
                c.NombreCompleto AS Cliente,
                e.TipoEquipo + ' ' + e.Marca + ' ' + e.Modelo AS Equipo,
                s.Estado,
                s.CostoTotal,
                s.FechaEntregaEstimada
                FROM Servicios s
                INNER JOIN Clientes c ON s.ClienteID = c.ClienteID
                INNER JOIN Equipos e ON s.EquipoID = e.EquipoID
                WHERE s.TecnicoAsignado = @TecnicoID
                ORDER BY s.FechaIngreso DESC";
            }

            SqlParameter[] parameters = SessionData.IsTecnico()
                ? new[] { new SqlParameter("@TecnicoID", SessionData.UsuarioID) }
                : null;

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null)
            {
                dgvServicios.DataSource = dt;

                if (dgvServicios.Columns.Contains("ServicioID"))
                    dgvServicios.Columns["ServicioID"].Visible = false;

                lblTotal.Text = $"Total de servicios: {dt.Rows.Count}";
            }
        }


        private void ClearFields()
        {
            cmbCliente.SelectedIndex = -1;
            cmbEquipo.SelectedIndex = -1;
            cmbTecnico.SelectedIndex = -1;
            cmbEstado.SelectedIndex = 0;
            dtpFechaIngreso.Value = DateTime.Now;
            dtpEntregaEstimada.Value = DateTime.Now.AddDays(3);
            txtDiagnostico.Clear();
            txtSolucion.Clear();
            txtCostoManoObra.Text = "0";
            txtObservaciones.Clear();
            servicioSeleccionadoID = 0;
            isEditing = false;
            btnGuardar.Text = "💾 Guardar";
        }

        private bool ValidateFields()
        {
            if (cmbCliente.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un cliente");
                return false;
            }
            if (cmbEquipo.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un equipo");
                return false;
            }
            if (cmbTecnico.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un técnico");
                return false;
            }
            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            try
            {
                if (isEditing)
                {
                    string query = @"UPDATE Servicios SET 
                                   ClienteID = @ClienteID,
                                   EquipoID = @EquipoID,
                                   TecnicoAsignado = @TecnicoID,
                                   FechaEntregaEstimada = @FechaEstimada,
                                   Diagnostico = @Diagnostico,
                                   SolucionAplicada = @Solucion,
                                   Estado = @Estado,
                                   CostoManoObra = @CostoManoObra,
                                   Observaciones = @Observaciones
                                   WHERE ServicioID = @ServicioID";

                    SqlParameter[] parameters = {
                        new SqlParameter("@ClienteID", cmbCliente.SelectedValue),
                        new SqlParameter("@EquipoID", cmbEquipo.SelectedValue),
                        new SqlParameter("@TecnicoID", cmbTecnico.SelectedValue),
                        new SqlParameter("@FechaEstimada", dtpEntregaEstimada.Value),
                        new SqlParameter("@Diagnostico", string.IsNullOrWhiteSpace(txtDiagnostico.Text) ? DBNull.Value : (object)txtDiagnostico.Text),
                        new SqlParameter("@Solucion", string.IsNullOrWhiteSpace(txtSolucion.Text) ? DBNull.Value : (object)txtSolucion.Text),
                        new SqlParameter("@Estado", cmbEstado.SelectedItem.ToString()),
                        new SqlParameter("@CostoManoObra", decimal.Parse(txtCostoManoObra.Text)),
                        new SqlParameter("@Observaciones", string.IsNullOrWhiteSpace(txtObservaciones.Text) ? DBNull.Value : (object)txtObservaciones.Text),
                        new SqlParameter("@ServicioID", servicioSeleccionadoID)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Servicio actualizado correctamente");
                        LoadServicios();
                        ClearFields();
                    }
                }
                else
                {
                    string query = @"INSERT INTO Servicios 
                                   (ClienteID, EquipoID, TecnicoAsignado, FechaIngreso, FechaEntregaEstimada, 
                                   Diagnostico, SolucionAplicada, Estado, CostoManoObra, Observaciones)
                                   VALUES 
                                   (@ClienteID, @EquipoID, @TecnicoID, @FechaIngreso, @FechaEstimada, 
                                   @Diagnostico, @Solucion, @Estado, @CostoManoObra, @Observaciones)";

                    SqlParameter[] parameters = {
                        new SqlParameter("@ClienteID", cmbCliente.SelectedValue),
                        new SqlParameter("@EquipoID", cmbEquipo.SelectedValue),
                        new SqlParameter("@TecnicoID", cmbTecnico.SelectedValue),
                        new SqlParameter("@FechaIngreso", dtpFechaIngreso.Value),
                        new SqlParameter("@FechaEstimada", dtpEntregaEstimada.Value),
                        new SqlParameter("@Diagnostico", string.IsNullOrWhiteSpace(txtDiagnostico.Text) ? DBNull.Value : (object)txtDiagnostico.Text),
                        new SqlParameter("@Solucion", string.IsNullOrWhiteSpace(txtSolucion.Text) ? DBNull.Value : (object)txtSolucion.Text),
                        new SqlParameter("@Estado", cmbEstado.SelectedItem.ToString()),
                        new SqlParameter("@CostoManoObra", decimal.Parse(txtCostoManoObra.Text)),
                        new SqlParameter("@Observaciones", string.IsNullOrWhiteSpace(txtObservaciones.Text) ? DBNull.Value : (object)txtObservaciones.Text)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Servicio registrado correctamente");
                        LoadServicios();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvServicios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un servicio");
                return;
            }

            servicioSeleccionadoID = Convert.ToInt32(dgvServicios.SelectedRows[0].Cells["ServicioID"].Value);

            string query = @"SELECT * FROM Servicios WHERE ServicioID = @ServicioID";
            SqlParameter[] parameters = { new SqlParameter("@ServicioID", servicioSeleccionadoID) };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                cmbCliente.SelectedValue = row["ClienteID"];
                cmbEquipo.SelectedValue = row["EquipoID"];
                cmbTecnico.SelectedValue = row["TecnicoAsignado"];
                dtpFechaIngreso.Value = Convert.ToDateTime(row["FechaIngreso"]);

                dtpEntregaEstimada.Value = row["FechaEntregaEstimada"] != DBNull.Value
                    ? Convert.ToDateTime(row["FechaEntregaEstimada"])
                    : DateTime.Now.AddDays(3);

                txtDiagnostico.Text = row["Diagnostico"] != DBNull.Value ? row["Diagnostico"].ToString() : "";
                txtSolucion.Text = row["SolucionAplicada"] != DBNull.Value ? row["SolucionAplicada"].ToString() : "";
                cmbEstado.SelectedItem = row["Estado"].ToString();
                txtCostoManoObra.Text = row["CostoManoObra"].ToString();
                txtObservaciones.Text = row["Observaciones"] != DBNull.Value ? row["Observaciones"].ToString() : "";

                isEditing = true;
                btnGuardar.Text = "💾 Actualizar";
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadServicios();
                return;
            }

            string query = @"SELECT 
                            s.ServicioID,
                            s.FechaIngreso,
                            c.NombreCompleto AS Cliente,
                            e.TipoEquipo + ' ' + e.Marca + ' ' + e.Modelo AS Equipo,
                            s.Estado,
                            u.NombreCompleto AS Tecnico,
                            s.CostoTotal,
                            s.FechaEntregaEstimada
                            FROM Servicios s
                            INNER JOIN Clientes c ON s.ClienteID = c.ClienteID
                            INNER JOIN Equipos e ON s.EquipoID = e.EquipoID
                            INNER JOIN Usuarios u ON s.TecnicoAsignado = u.UsuarioID
                            WHERE c.NombreCompleto LIKE @Search OR s.Estado LIKE @Search
                            ORDER BY s.FechaIngreso DESC";

            SqlParameter[] parameters = { new SqlParameter("@Search", "%" + searchText + "%") };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null)
            {
                dgvServicios.DataSource = dt;
                lblTotal.Text = $"Servicios encontrados: {dt.Rows.Count}";
            }
        }

        private void txtCostoManoObra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;

            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;
        }
    }
}
