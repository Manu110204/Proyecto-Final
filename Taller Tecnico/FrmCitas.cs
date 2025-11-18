using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace TallerTecnico
{
    public partial class FrmCitas : Form
    {
        private int citaSeleccionadaID = 0;
        private bool isEditing = false;

        public FrmCitas()
        {
            InitializeComponent();
            LoadCitas();
            LoadClientes();
            LoadTecnicos();
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            dgvCitas.EnableHeadersVisualStyles = false;
            dgvCitas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvCitas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCitas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCitas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
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
            // Evita ejecutar cuando el ComboBox está cargando
            if (cmbCliente.SelectedIndex == -1) return;

            // Verifica que el valor sea un número válido
            if (cmbCliente.SelectedValue == null) return;
            if (cmbCliente.SelectedValue is DataRowView) return;

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

        private void LoadCitas()
        {
            string query = @"SELECT 
                            ci.CitaID,
                            ci.FechaCita,
                            ci.HoraCita,
                            c.NombreCompleto AS Cliente,
                            c.Telefono,
                            e.TipoEquipo + ' ' + e.Marca + ' ' + e.Modelo AS Equipo,
                            ci.MotivoConsulta,
                            ci.Estado,
                            u.NombreCompleto AS TecnicoAsignado
                            FROM Citas ci
                            INNER JOIN Clientes c ON ci.ClienteID = c.ClienteID
                            INNER JOIN Equipos e ON ci.EquipoID = e.EquipoID
                            LEFT JOIN Usuarios u ON ci.TecnicoAsignado = u.UsuarioID
                            ORDER BY ci.FechaCita DESC, ci.HoraCita DESC";

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);
            if (dt != null)
            {
                dgvCitas.DataSource = dt;
                dgvCitas.Columns["CitaID"].Visible = false;
                dgvCitas.Columns["FechaCita"].HeaderText = "Fecha Cita";
                dgvCitas.Columns["HoraCita"].HeaderText = "Hora";
                dgvCitas.Columns["Cliente"].HeaderText = "Cliente";
                dgvCitas.Columns["Telefono"].HeaderText = "Teléfono";
                dgvCitas.Columns["Equipo"].HeaderText = "Equipo";
                dgvCitas.Columns["MotivoConsulta"].HeaderText = "Motivo";
                dgvCitas.Columns["Estado"].HeaderText = "Estado";
                dgvCitas.Columns["TecnicoAsignado"].HeaderText = "Técnico";

                lblTotal.Text = $"Total de citas: {dt.Rows.Count}";
            }
        }

        private void ClearFields()
        {
            cmbCliente.SelectedIndex = -1;
            cmbEquipo.SelectedIndex = -1;
            cmbTecnico.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Now.AddDays(1);
            dtpHora.Value = DateTime.Now;
            txtMotivo.Clear();
            cmbEstado.SelectedIndex = 0;
            citaSeleccionadaID = 0;
            isEditing = false;
            btnGuardar.Text = "💾 Guardar";
        }

        private bool ValidateFields()
        {
            if (cmbCliente.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un cliente", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCliente.Focus();
                return false;
            }

            if (cmbEquipo.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un equipo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbEquipo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Por favor ingrese el motivo de la consulta", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotivo.Focus();
                return false;
            }

            if (dtpFecha.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("La fecha de la cita no puede ser anterior a hoy", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFecha.Focus();
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
                    // Actualizar
                    string query = @"UPDATE Citas SET 
                                   ClienteID = @ClienteID,
                                   EquipoID = @EquipoID,
                                   FechaCita = @FechaCita,
                                   HoraCita = @HoraCita,
                                   MotivoConsulta = @Motivo,
                                   Estado = @Estado,
                                   TecnicoAsignado = @TecnicoID
                                   WHERE CitaID = @CitaID";

                    SqlParameter[] parameters = {
                        new SqlParameter("@ClienteID", cmbCliente.SelectedValue),
                        new SqlParameter("@EquipoID", cmbEquipo.SelectedValue),
                        new SqlParameter("@FechaCita", dtpFecha.Value.Date),
                        new SqlParameter("@HoraCita", dtpHora.Value.TimeOfDay),
                        new SqlParameter("@Motivo", txtMotivo.Text),
                        new SqlParameter("@Estado", cmbEstado.SelectedItem.ToString()),
                        new SqlParameter("@TecnicoID", cmbTecnico.SelectedValue != null ? cmbTecnico.SelectedValue : DBNull.Value),
                        new SqlParameter("@CitaID", citaSeleccionadaID)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Cita actualizada exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCitas();
                        ClearFields();
                    }
                }
                else
                {
                    // Insertar nueva
                    string query = @"INSERT INTO Citas 
                                   (ClienteID, EquipoID, FechaCita, HoraCita, MotivoConsulta, Estado, TecnicoAsignado)
                                   VALUES 
                                   (@ClienteID, @EquipoID, @FechaCita, @HoraCita, @Motivo, @Estado, @TecnicoID)";

                    SqlParameter[] parameters = {
                        new SqlParameter("@ClienteID", cmbCliente.SelectedValue),
                        new SqlParameter("@EquipoID", cmbEquipo.SelectedValue),
                        new SqlParameter("@FechaCita", dtpFecha.Value.Date),
                        new SqlParameter("@HoraCita", dtpHora.Value.TimeOfDay),
                        new SqlParameter("@Motivo", txtMotivo.Text),
                        new SqlParameter("@Estado", cmbEstado.SelectedItem.ToString()),
                        new SqlParameter("@TecnicoID", cmbTecnico.SelectedValue != null ? cmbTecnico.SelectedValue : DBNull.Value)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Cita registrada exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCitas();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la cita: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCitas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione una cita para editar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            citaSeleccionadaID = Convert.ToInt32(dgvCitas.SelectedRows[0].Cells["CitaID"].Value);

            // Cargar datos de la cita
            string query = @"SELECT * FROM Citas WHERE CitaID = @CitaID";
            SqlParameter[] parameters = { new SqlParameter("@CitaID", citaSeleccionadaID) };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                cmbCliente.SelectedValue = row["ClienteID"];
                cmbEquipo.SelectedValue = row["EquipoID"];
                dtpFecha.Value = Convert.ToDateTime(row["FechaCita"]);
                dtpHora.Value = DateTime.Today.Add((TimeSpan)row["HoraCita"]);
                txtMotivo.Text = row["MotivoConsulta"].ToString();
                cmbEstado.SelectedItem = row["Estado"].ToString();
                if (row["TecnicoAsignado"] != DBNull.Value)
                    cmbTecnico.SelectedValue = row["TecnicoAsignado"];

                isEditing = true;
                btnGuardar.Text = "💾 Actualizar";
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCitas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione una cita para eliminar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("¿Está seguro de eliminar esta cita?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int citaID = Convert.ToInt32(dgvCitas.SelectedRows[0].Cells["CitaID"].Value);
                string query = "UPDATE Citas SET Estado = 'Cancelada' WHERE CitaID = @CitaID";
                SqlParameter[] parameters = { new SqlParameter("@CitaID", citaID) };

                if (DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    MessageBox.Show("Cita cancelada exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCitas();
                    ClearFields();
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadCitas();
                return;
            }

            string query = @"SELECT 
                            ci.CitaID,
                            ci.FechaCita,
                            ci.HoraCita,
                            c.NombreCompleto AS Cliente,
                            c.Telefono,
                            e.TipoEquipo + ' ' + e.Marca + ' ' + e.Modelo AS Equipo,
                            ci.MotivoConsulta,
                            ci.Estado,
                            u.NombreCompleto AS TecnicoAsignado
                            FROM Citas ci
                            INNER JOIN Clientes c ON ci.ClienteID = c.ClienteID
                            INNER JOIN Equipos e ON ci.EquipoID = e.EquipoID
                            LEFT JOIN Usuarios u ON ci.TecnicoAsignado = u.UsuarioID
                            WHERE c.NombreCompleto LIKE @Search OR ci.Estado LIKE @Search
                            ORDER BY ci.FechaCita DESC, ci.HoraCita DESC";

            SqlParameter[] parameters = { new SqlParameter("@Search", "%" + searchText + "%") };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null)
            {
                dgvCitas.DataSource = dt;
                lblTotal.Text = $"Citas encontradas: {dt.Rows.Count}";
            }
        }
    }
}