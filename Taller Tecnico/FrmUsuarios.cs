using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace TallerTecnico
{
    public partial class FrmUsuarios : Form
    {
        private int usuarioSeleccionadoID = 0;
        private bool isEditing = false;

        public FrmUsuarios()
        {
            InitializeComponent();
            LoadUsuarios();
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void LoadUsuarios()
        {
            string query = @"SELECT UsuarioID, NombreUsuario, NombreCompleto, Rol, Email, Telefono,
                            FechaRegistro, CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
                            FROM Usuarios ORDER BY FechaRegistro DESC";

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);
            if (dt != null)
            {
                dgvUsuarios.DataSource = dt;
                dgvUsuarios.Columns["UsuarioID"].Visible = false;
                dgvUsuarios.Columns["NombreUsuario"].HeaderText = "Usuario";
                dgvUsuarios.Columns["NombreCompleto"].HeaderText = "Nombre Completo";
                dgvUsuarios.Columns["Rol"].HeaderText = "Rol";
                dgvUsuarios.Columns["Email"].HeaderText = "Email";
                dgvUsuarios.Columns["Telefono"].HeaderText = "Teléfono";
                dgvUsuarios.Columns["FechaRegistro"].HeaderText = "Fecha Registro";
                dgvUsuarios.Columns["Estado"].HeaderText = "Estado";

                lblTotal.Text = $"Total de usuarios: {dt.Rows.Count}";
            }
        }

        private void ClearFields()
        {
            txtUsuario.Clear();
            txtContrasena.Clear();
            txtNombreCompleto.Clear();
            cmbRol.SelectedIndex = -1;
            txtEmail.Clear();
            txtTelefono.Clear();
            chkActivo.Checked = true;
            usuarioSeleccionadoID = 0;
            isEditing = false;
            btnGuardar.Text = "💾 Guardar";
            txtContrasena.Enabled = true;
            txtUsuario.Enabled = true;
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Por favor ingrese el nombre de usuario", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return false;
            }

            if (!isEditing && string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Por favor ingrese la contraseña", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasena.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreCompleto.Text))
            {
                MessageBox.Show("Por favor ingrese el nombre completo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreCompleto.Focus();
                return false;
            }

            if (cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un rol", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRol.Focus();
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
                    // Actualizar (sin cambiar contraseña si está vacía)
                    string query = @"UPDATE Usuarios SET 
                                   NombreCompleto = @NombreCompleto,
                                   Rol = @Rol,
                                   Email = @Email,
                                   Telefono = @Telefono,
                                   Activo = @Activo
                                   WHERE UsuarioID = @UsuarioID";

                    SqlParameter[] parameters = {
                        new SqlParameter("@NombreCompleto", txtNombreCompleto.Text),
                        new SqlParameter("@Rol", cmbRol.SelectedItem.ToString()),
                        new SqlParameter("@Email", string.IsNullOrWhiteSpace(txtEmail.Text) ? DBNull.Value : (object)txtEmail.Text),
                        new SqlParameter("@Telefono", string.IsNullOrWhiteSpace(txtTelefono.Text) ? DBNull.Value : (object)txtTelefono.Text),
                        new SqlParameter("@Activo", chkActivo.Checked),
                        new SqlParameter("@UsuarioID", usuarioSeleccionadoID)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Usuario actualizado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsuarios();
                        ClearFields();
                    }
                }
                else
                {
                    // Insertar nuevo
                    string query = @"INSERT INTO Usuarios 
                                   (NombreUsuario, Contrasena, NombreCompleto, Rol, Email, Telefono, Activo)
                                   VALUES 
                                   (@Usuario, @Contrasena, @NombreCompleto, @Rol, @Email, @Telefono, @Activo)";

                    SqlParameter[] parameters = {
                        new SqlParameter("@Usuario", txtUsuario.Text),
                        new SqlParameter("@Contrasena", txtContrasena.Text),
                        new SqlParameter("@NombreCompleto", txtNombreCompleto.Text),
                        new SqlParameter("@Rol", cmbRol.SelectedItem.ToString()),
                        new SqlParameter("@Email", string.IsNullOrWhiteSpace(txtEmail.Text) ? DBNull.Value : (object)txtEmail.Text),
                        new SqlParameter("@Telefono", string.IsNullOrWhiteSpace(txtTelefono.Text) ? DBNull.Value : (object)txtTelefono.Text),
                        new SqlParameter("@Activo", chkActivo.Checked)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Usuario registrado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsuarios();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unique") || ex.Message.Contains("UNIQUE"))
                {
                    MessageBox.Show("El nombre de usuario ya existe", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Error al guardar usuario: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un usuario para editar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvUsuarios.SelectedRows[0];
            usuarioSeleccionadoID = Convert.ToInt32(row.Cells["UsuarioID"].Value);
            txtUsuario.Text = row.Cells["NombreUsuario"].Value.ToString();
            txtUsuario.Enabled = false;
            txtContrasena.Enabled = false;
            txtContrasena.Text = "••••••••";
            txtNombreCompleto.Text = row.Cells["NombreCompleto"].Value.ToString();
            cmbRol.SelectedItem = row.Cells["Rol"].Value.ToString();
            txtEmail.Text = row.Cells["Email"].Value != DBNull.Value ? row.Cells["Email"].Value.ToString() : "";
            txtTelefono.Text = row.Cells["Telefono"].Value != DBNull.Value ? row.Cells["Telefono"].Value.ToString() : "";
            chkActivo.Checked = row.Cells["Estado"].Value.ToString() == "Activo";

            isEditing = true;
            btnGuardar.Text = "💾 Actualizar";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un usuario para eliminar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("¿Está seguro de eliminar este usuario?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int usuarioID = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["UsuarioID"].Value);
                string query = "UPDATE Usuarios SET Activo = 0 WHERE UsuarioID = @UsuarioID";
                SqlParameter[] parameters = { new SqlParameter("@UsuarioID", usuarioID) };

                if (DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    MessageBox.Show("Usuario eliminado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsuarios();
                    ClearFields();
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadUsuarios();
                return;
            }

            string query = @"SELECT UsuarioID, NombreUsuario, NombreCompleto, Rol, Email, Telefono,
                            FechaRegistro, CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
                            FROM Usuarios 
                            WHERE NombreUsuario LIKE @Search OR NombreCompleto LIKE @Search
                            ORDER BY FechaRegistro DESC";

            SqlParameter[] parameters = { new SqlParameter("@Search", "%" + searchText + "%") };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null)
            {
                dgvUsuarios.DataSource = dt;
                lblTotal.Text = $"Usuarios encontrados: {dt.Rows.Count}";
            }
        }
    }
}