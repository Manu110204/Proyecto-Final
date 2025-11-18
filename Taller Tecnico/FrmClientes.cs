using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace TallerTecnico
{
    public partial class FrmClientes : Form
    {
        private int clienteSeleccionadoID = 0;
        private bool isEditing = false;

        public FrmClientes()
        {
            InitializeComponent();
            LoadClientes();
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvClientes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void LoadClientes()
        {
            string query = @"SELECT ClienteID, DNI, NombreCompleto, Telefono, Email, Direccion, 
                            FechaRegistro, 
                            CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
                            FROM Clientes ORDER BY FechaRegistro DESC";

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);
            if (dt != null)
            {
                dgvClientes.DataSource = dt;
                dgvClientes.Columns["ClienteID"].Visible = false;
                dgvClientes.Columns["DNI"].HeaderText = "DNI";
                dgvClientes.Columns["NombreCompleto"].HeaderText = "Nombre Completo";
                dgvClientes.Columns["Telefono"].HeaderText = "Teléfono";
                dgvClientes.Columns["Email"].HeaderText = "Email";
                dgvClientes.Columns["Direccion"].HeaderText = "Dirección";
                dgvClientes.Columns["FechaRegistro"].HeaderText = "Fecha Registro";
                dgvClientes.Columns["Estado"].HeaderText = "Estado";

                lblTotal.Text = $"Total de clientes: {dt.Rows.Count}";
            }
        }

        private void ClearFields()
        {
            txtDNI.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            chkActivo.Checked = true;
            clienteSeleccionadoID = 0;
            isEditing = false;
            btnGuardar.Text = "💾 Guardar";
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("Por favor ingrese el DNI del cliente", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDNI.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor ingrese el nombre del cliente", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Por favor ingrese el teléfono del cliente", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelefono.Focus();
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
                    string query = @"UPDATE Clientes SET 
                                   DNI = @DNI,
                                   NombreCompleto = @Nombre,
                                   Telefono = @Telefono,
                                   Email = @Email,
                                   Direccion = @Direccion,
                                   Activo = @Activo
                                   WHERE ClienteID = @ClienteID";

                    SqlParameter[] parameters = {
                        new SqlParameter("@DNI", txtDNI.Text),
                        new SqlParameter("@Nombre", txtNombre.Text),
                        new SqlParameter("@Telefono", txtTelefono.Text),
                        new SqlParameter("@Email", string.IsNullOrWhiteSpace(txtEmail.Text) ? DBNull.Value : (object)txtEmail.Text),
                        new SqlParameter("@Direccion", string.IsNullOrWhiteSpace(txtDireccion.Text) ? DBNull.Value : (object)txtDireccion.Text),
                        new SqlParameter("@Activo", chkActivo.Checked),
                        new SqlParameter("@ClienteID", clienteSeleccionadoID)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Cliente actualizado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadClientes();
                        ClearFields();
                    }
                }
                else
                {
                    // Insertar nuevo
                    string query = @"INSERT INTO Clientes (DNI, NombreCompleto, Telefono, Email, Direccion, Activo)
                                   VALUES (@DNI, @Nombre, @Telefono, @Email, @Direccion, @Activo)";

                    SqlParameter[] parameters = {
                        new SqlParameter("@DNI", txtDNI.Text),
                        new SqlParameter("@Nombre", txtNombre.Text),
                        new SqlParameter("@Telefono", txtTelefono.Text),
                        new SqlParameter("@Email", string.IsNullOrWhiteSpace(txtEmail.Text) ? DBNull.Value : (object)txtEmail.Text),
                        new SqlParameter("@Direccion", string.IsNullOrWhiteSpace(txtDireccion.Text) ? DBNull.Value : (object)txtDireccion.Text),
                        new SqlParameter("@Activo", chkActivo.Checked)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Cliente registrado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadClientes();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el cliente: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un cliente para editar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvClientes.SelectedRows[0];
            clienteSeleccionadoID = Convert.ToInt32(row.Cells["ClienteID"].Value);
            txtDNI.Text = row.Cells["DNI"].Value.ToString();
            txtNombre.Text = row.Cells["NombreCompleto"].Value.ToString();
            txtTelefono.Text = row.Cells["Telefono"].Value.ToString();
            txtEmail.Text = row.Cells["Email"].Value != DBNull.Value ? row.Cells["Email"].Value.ToString() : "";
            txtDireccion.Text = row.Cells["Direccion"].Value != DBNull.Value ? row.Cells["Direccion"].Value.ToString() : "";
            chkActivo.Checked = row.Cells["Estado"].Value.ToString() == "Activo";

            isEditing = true;
            btnGuardar.Text = "💾 Actualizar";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un cliente para eliminar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("¿Está seguro de eliminar este cliente?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int clienteID = Convert.ToInt32(dgvClientes.SelectedRows[0].Cells["ClienteID"].Value);
                string query = "UPDATE Clientes SET Activo = 0 WHERE ClienteID = @ClienteID";
                SqlParameter[] parameters = { new SqlParameter("@ClienteID", clienteID) };

                if (DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    MessageBox.Show("Cliente eliminado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadClientes();
                    ClearFields();
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadClientes();
                return;
            }

            string query = @"SELECT ClienteID, DNI, NombreCompleto, Telefono, Email, Direccion, 
                            FechaRegistro, 
                            CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
                            FROM Clientes 
                            WHERE DNI LIKE @Search OR NombreCompleto LIKE @Search OR Telefono LIKE @Search
                            ORDER BY FechaRegistro DESC";

            SqlParameter[] parameters = { new SqlParameter("@Search", "%" + searchText + "%") };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null)
            {
                dgvClientes.DataSource = dt;
                lblTotal.Text = $"Clientes encontrados: {dt.Rows.Count}";
            }
        }
    }
}