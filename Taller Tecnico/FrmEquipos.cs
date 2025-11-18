using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace TallerTecnico
{
    public partial class FrmEquipos : Form
    {
        private int equipoSeleccionadoID = 0;
        private bool isEditing = false;

        public FrmEquipos()
        {
            InitializeComponent();
            LoadEquipos();
            LoadClientes();
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            dgvEquipos.EnableHeadersVisualStyles = false;
            dgvEquipos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvEquipos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEquipos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvEquipos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
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

        private void LoadEquipos()
        {
            string query = @"SELECT 
                            e.EquipoID,
                            c.NombreCompleto AS Cliente,
                            e.TipoEquipo,
                            e.Marca,
                            e.Modelo,
                            e.NumeroSerie,
                            e.Descripcion,
                            e.FechaRegistro
                            FROM Equipos e
                            INNER JOIN Clientes c ON e.ClienteID = c.ClienteID
                            ORDER BY e.FechaRegistro DESC";

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);
            if (dt != null)
            {
                dgvEquipos.DataSource = dt;
                dgvEquipos.Columns["EquipoID"].Visible = false;
                dgvEquipos.Columns["Cliente"].HeaderText = "Cliente";
                dgvEquipos.Columns["TipoEquipo"].HeaderText = "Tipo Equipo";
                dgvEquipos.Columns["Marca"].HeaderText = "Marca";
                dgvEquipos.Columns["Modelo"].HeaderText = "Modelo";
                dgvEquipos.Columns["NumeroSerie"].HeaderText = "N° Serie";
                dgvEquipos.Columns["Descripcion"].HeaderText = "Descripción";
                dgvEquipos.Columns["FechaRegistro"].HeaderText = "Fecha Registro";

                lblTotal.Text = $"Total de equipos: {dt.Rows.Count}";
            }
        }

        private void ClearFields()
        {
            cmbCliente.SelectedIndex = -1;
            cmbTipoEquipo.SelectedIndex = -1;
            txtMarca.Clear();
            txtModelo.Clear();
            txtNumeroSerie.Clear();
            txtDescripcion.Clear();
            equipoSeleccionadoID = 0;
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

            if (cmbTipoEquipo.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione el tipo de equipo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoEquipo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMarca.Text))
            {
                MessageBox.Show("Por favor ingrese la marca del equipo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMarca.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtModelo.Text))
            {
                MessageBox.Show("Por favor ingrese el modelo del equipo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtModelo.Focus();
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
                    string query = @"UPDATE Equipos SET 
                                   ClienteID = @ClienteID,
                                   TipoEquipo = @TipoEquipo,
                                   Marca = @Marca,
                                   Modelo = @Modelo,
                                   NumeroSerie = @NumeroSerie,
                                   Descripcion = @Descripcion
                                   WHERE EquipoID = @EquipoID";

                    SqlParameter[] parameters = {
                        new SqlParameter("@ClienteID", cmbCliente.SelectedValue),
                        new SqlParameter("@TipoEquipo", cmbTipoEquipo.SelectedItem.ToString()),
                        new SqlParameter("@Marca", txtMarca.Text),
                        new SqlParameter("@Modelo", txtModelo.Text),
                        new SqlParameter("@NumeroSerie", string.IsNullOrWhiteSpace(txtNumeroSerie.Text) ? DBNull.Value : (object)txtNumeroSerie.Text),
                        new SqlParameter("@Descripcion", string.IsNullOrWhiteSpace(txtDescripcion.Text) ? DBNull.Value : (object)txtDescripcion.Text),
                        new SqlParameter("@EquipoID", equipoSeleccionadoID)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Equipo actualizado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEquipos();
                        ClearFields();
                    }
                }
                else
                {
                    // Insertar nuevo
                    string query = @"INSERT INTO Equipos (ClienteID, TipoEquipo, Marca, Modelo, NumeroSerie, Descripcion)
                                   VALUES (@ClienteID, @TipoEquipo, @Marca, @Modelo, @NumeroSerie, @Descripcion)";

                    SqlParameter[] parameters = {
                        new SqlParameter("@ClienteID", cmbCliente.SelectedValue),
                        new SqlParameter("@TipoEquipo", cmbTipoEquipo.SelectedItem.ToString()),
                        new SqlParameter("@Marca", txtMarca.Text),
                        new SqlParameter("@Modelo", txtModelo.Text),
                        new SqlParameter("@NumeroSerie", string.IsNullOrWhiteSpace(txtNumeroSerie.Text) ? DBNull.Value : (object)txtNumeroSerie.Text),
                        new SqlParameter("@Descripcion", string.IsNullOrWhiteSpace(txtDescripcion.Text) ? DBNull.Value : (object)txtDescripcion.Text)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Equipo registrado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEquipos();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el equipo: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvEquipos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un equipo para editar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            equipoSeleccionadoID = Convert.ToInt32(dgvEquipos.SelectedRows[0].Cells["EquipoID"].Value);

            // Cargar datos del equipo
            string query = @"SELECT * FROM Equipos WHERE EquipoID = @EquipoID";
            SqlParameter[] parameters = { new SqlParameter("@EquipoID", equipoSeleccionadoID) };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                cmbCliente.SelectedValue = row["ClienteID"];
                cmbTipoEquipo.SelectedItem = row["TipoEquipo"].ToString();
                txtMarca.Text = row["Marca"].ToString();
                txtModelo.Text = row["Modelo"].ToString();
                txtNumeroSerie.Text = row["NumeroSerie"] != DBNull.Value ? row["NumeroSerie"].ToString() : "";
                txtDescripcion.Text = row["Descripcion"] != DBNull.Value ? row["Descripcion"].ToString() : "";

                isEditing = true;
                btnGuardar.Text = "💾 Actualizar";
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEquipos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un equipo para eliminar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("¿Está seguro de eliminar este equipo?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int equipoID = Convert.ToInt32(dgvEquipos.SelectedRows[0].Cells["EquipoID"].Value);
                string query = "DELETE FROM Equipos WHERE EquipoID = @EquipoID";
                SqlParameter[] parameters = { new SqlParameter("@EquipoID", equipoID) };

                if (DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    MessageBox.Show("Equipo eliminado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEquipos();
                    ClearFields();
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadEquipos();
                return;
            }

            string query = @"SELECT 
                            e.EquipoID,
                            c.NombreCompleto AS Cliente,
                            e.TipoEquipo,
                            e.Marca,
                            e.Modelo,
                            e.NumeroSerie,
                            e.Descripcion,
                            e.FechaRegistro
                            FROM Equipos e
                            INNER JOIN Clientes c ON e.ClienteID = c.ClienteID
                            WHERE c.NombreCompleto LIKE @Search OR e.TipoEquipo LIKE @Search 
                            OR e.Marca LIKE @Search OR e.Modelo LIKE @Search
                            ORDER BY e.FechaRegistro DESC";

            SqlParameter[] parameters = { new SqlParameter("@Search", "%" + searchText + "%") };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null)
            {
                dgvEquipos.DataSource = dt;
                lblTotal.Text = $"Equipos encontrados: {dt.Rows.Count}";
            }
        }
    }
}