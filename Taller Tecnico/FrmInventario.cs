using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace TallerTecnico
{
    public partial class FrmInventario : Form
    {
        private int repuestoSeleccionadoID = 0;
        private bool isEditing = false;

        public FrmInventario()
        {
            InitializeComponent();
            LoadRepuestos();
            ConfigureDataGridView();
            ConfigureByRole();
        }

        private void ConfigureByRole()
        {
            // Si es Técnico, solo puede consultar (modo lectura)
            if (SessionData.IsTecnico())
            {
                btnGuardar.Enabled = false;
                btnNuevo.Enabled = false;
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;

                txtCodigo.ReadOnly = true;
                txtNombre.ReadOnly = true;
                txtDescripcion.ReadOnly = true;
                cmbCategoria.Enabled = false;
                txtStockActual.ReadOnly = true;
                txtStockMinimo.ReadOnly = true;
                txtPrecioCompra.ReadOnly = true;
                txtPrecioVenta.ReadOnly = true;
                txtProveedor.ReadOnly = true;
                chkActivo.Enabled = false;

                label1.Text = "📦 Consulta de Inventario (Solo Lectura)";
            }
        }

        private void ConfigureDataGridView()
        {
            dgvRepuestos.EnableHeadersVisualStyles = false;
            dgvRepuestos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvRepuestos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRepuestos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRepuestos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void LoadRepuestos()
        {
            string query = @"SELECT RepuestoID, Codigo, Nombre, Descripcion, Categoria, 
                            StockActual, StockMinimo, PrecioCompra, PrecioVenta, Proveedor,
                            CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
                            FROM Repuestos ORDER BY Nombre";

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);
            if (dt != null)
            {
                dgvRepuestos.DataSource = dt;
                dgvRepuestos.Columns["RepuestoID"].Visible = false;
                dgvRepuestos.Columns["Codigo"].HeaderText = "Código";
                dgvRepuestos.Columns["Nombre"].HeaderText = "Nombre";
                dgvRepuestos.Columns["Descripcion"].HeaderText = "Descripción";
                dgvRepuestos.Columns["Categoria"].HeaderText = "Categoría";
                dgvRepuestos.Columns["StockActual"].HeaderText = "Stock Actual";
                dgvRepuestos.Columns["StockMinimo"].HeaderText = "Stock Mínimo";
                dgvRepuestos.Columns["PrecioCompra"].HeaderText = "Precio Compra";
                dgvRepuestos.Columns["PrecioVenta"].HeaderText = "Precio Venta";
                dgvRepuestos.Columns["Proveedor"].HeaderText = "Proveedor";
                dgvRepuestos.Columns["Estado"].HeaderText = "Estado";

                // Resaltar repuestos con stock bajo
                foreach (DataGridViewRow row in dgvRepuestos.Rows)
                {
                    int stockActual = Convert.ToInt32(row.Cells["StockActual"].Value);
                    int stockMinimo = Convert.ToInt32(row.Cells["StockMinimo"].Value);

                    if (stockActual <= stockMinimo)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(198, 40, 40);
                    }
                }

                lblTotal.Text = $"Total de repuestos: {dt.Rows.Count}";

                // Contar stock bajo
                int stockBajo = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["StockActual"]) <= Convert.ToInt32(dr["StockMinimo"]))
                        stockBajo++;
                }
                lblStockBajo.Text = $"Repuestos con stock bajo: {stockBajo}";
            }
        }

        private void ClearFields()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            cmbCategoria.SelectedIndex = -1;
            txtStockActual.Text = "0";
            txtStockMinimo.Text = "5";
            txtPrecioCompra.Text = "0";
            txtPrecioVenta.Text = "0";
            txtProveedor.Clear();
            chkActivo.Checked = true;
            repuestoSeleccionadoID = 0;
            isEditing = false;
            btnGuardar.Text = "💾 Guardar";
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Por favor ingrese el código del repuesto", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor ingrese el nombre del repuesto", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (cmbCategoria.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione una categoría", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategoria.Focus();
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
                    string query = @"UPDATE Repuestos SET 
                                   Codigo = @Codigo,
                                   Nombre = @Nombre,
                                   Descripcion = @Descripcion,
                                   Categoria = @Categoria,
                                   StockActual = @StockActual,
                                   StockMinimo = @StockMinimo,
                                   PrecioCompra = @PrecioCompra,
                                   PrecioVenta = @PrecioVenta,
                                   Proveedor = @Proveedor,
                                   Activo = @Activo
                                   WHERE RepuestoID = @RepuestoID";

                    SqlParameter[] parameters = {
                        new SqlParameter("@Codigo", txtCodigo.Text),
                        new SqlParameter("@Nombre", txtNombre.Text),
                        new SqlParameter("@Descripcion", string.IsNullOrWhiteSpace(txtDescripcion.Text) ? DBNull.Value : (object)txtDescripcion.Text),
                        new SqlParameter("@Categoria", cmbCategoria.SelectedItem.ToString()),
                        new SqlParameter("@StockActual", int.Parse(txtStockActual.Text)),
                        new SqlParameter("@StockMinimo", int.Parse(txtStockMinimo.Text)),
                        new SqlParameter("@PrecioCompra", decimal.Parse(txtPrecioCompra.Text)),
                        new SqlParameter("@PrecioVenta", decimal.Parse(txtPrecioVenta.Text)),
                        new SqlParameter("@Proveedor", string.IsNullOrWhiteSpace(txtProveedor.Text) ? DBNull.Value : (object)txtProveedor.Text),
                        new SqlParameter("@Activo", chkActivo.Checked),
                        new SqlParameter("@RepuestoID", repuestoSeleccionadoID)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Repuesto actualizado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRepuestos();
                        ClearFields();
                    }
                }
                else
                {
                    // Insertar nuevo
                    string query = @"INSERT INTO Repuestos 
                                   (Codigo, Nombre, Descripcion, Categoria, StockActual, StockMinimo, 
                                   PrecioCompra, PrecioVenta, Proveedor, Activo)
                                   VALUES 
                                   (@Codigo, @Nombre, @Descripcion, @Categoria, @StockActual, @StockMinimo, 
                                   @PrecioCompra, @PrecioVenta, @Proveedor, @Activo)";

                    SqlParameter[] parameters = {
                        new SqlParameter("@Codigo", txtCodigo.Text),
                        new SqlParameter("@Nombre", txtNombre.Text),
                        new SqlParameter("@Descripcion", string.IsNullOrWhiteSpace(txtDescripcion.Text) ? DBNull.Value : (object)txtDescripcion.Text),
                        new SqlParameter("@Categoria", cmbCategoria.SelectedItem.ToString()),
                        new SqlParameter("@StockActual", int.Parse(txtStockActual.Text)),
                        new SqlParameter("@StockMinimo", int.Parse(txtStockMinimo.Text)),
                        new SqlParameter("@PrecioCompra", decimal.Parse(txtPrecioCompra.Text)),
                        new SqlParameter("@PrecioVenta", decimal.Parse(txtPrecioVenta.Text)),
                        new SqlParameter("@Proveedor", string.IsNullOrWhiteSpace(txtProveedor.Text) ? DBNull.Value : (object)txtProveedor.Text),
                        new SqlParameter("@Activo", chkActivo.Checked)
                    };

                    if (DatabaseConnection.ExecuteQuery(query, parameters))
                    {
                        MessageBox.Show("Repuesto registrado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRepuestos();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el repuesto: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvRepuestos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un repuesto para editar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvRepuestos.SelectedRows[0];
            repuestoSeleccionadoID = Convert.ToInt32(row.Cells["RepuestoID"].Value);
            txtCodigo.Text = row.Cells["Codigo"].Value.ToString();
            txtNombre.Text = row.Cells["Nombre"].Value.ToString();
            txtDescripcion.Text = row.Cells["Descripcion"].Value != DBNull.Value ? row.Cells["Descripcion"].Value.ToString() : "";
            cmbCategoria.SelectedItem = row.Cells["Categoria"].Value.ToString();
            txtStockActual.Text = row.Cells["StockActual"].Value.ToString();
            txtStockMinimo.Text = row.Cells["StockMinimo"].Value.ToString();
            txtPrecioCompra.Text = row.Cells["PrecioCompra"].Value.ToString();
            txtPrecioVenta.Text = row.Cells["PrecioVenta"].Value.ToString();
            txtProveedor.Text = row.Cells["Proveedor"].Value != DBNull.Value ? row.Cells["Proveedor"].Value.ToString() : "";
            chkActivo.Checked = row.Cells["Estado"].Value.ToString() == "Activo";

            isEditing = true;
            btnGuardar.Text = "💾 Actualizar";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvRepuestos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un repuesto para eliminar", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("¿Está seguro de eliminar este repuesto?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int repuestoID = Convert.ToInt32(dgvRepuestos.SelectedRows[0].Cells["RepuestoID"].Value);
                string query = "UPDATE Repuestos SET Activo = 0 WHERE RepuestoID = @RepuestoID";
                SqlParameter[] parameters = { new SqlParameter("@RepuestoID", repuestoID) };

                if (DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    MessageBox.Show("Repuesto eliminado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRepuestos();
                    ClearFields();
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadRepuestos();
                return;
            }

            string query = @"SELECT RepuestoID, Codigo, Nombre, Descripcion, Categoria, 
                            StockActual, StockMinimo, PrecioCompra, PrecioVenta, Proveedor,
                            CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
                            FROM Repuestos 
                            WHERE Codigo LIKE @Search OR Nombre LIKE @Search OR Categoria LIKE @Search
                            ORDER BY Nombre";

            SqlParameter[] parameters = { new SqlParameter("@Search", "%" + searchText + "%") };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null)
            {
                dgvRepuestos.DataSource = dt;
                lblTotal.Text = $"Repuestos encontrados: {dt.Rows.Count}";
            }
        }

        private void btnStockBajo_Click(object sender, EventArgs e)
        {
            string query = @"SELECT RepuestoID, Codigo, Nombre, Descripcion, Categoria, 
                            StockActual, StockMinimo, PrecioCompra, PrecioVenta, Proveedor,
                            CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
                            FROM Repuestos 
                            WHERE StockActual <= StockMinimo AND Activo = 1
                            ORDER BY Nombre";

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);
            if (dt != null)
            {
                dgvRepuestos.DataSource = dt;
                lblTotal.Text = $"Repuestos con stock bajo: {dt.Rows.Count}";
            }
        }

        private void txtNumerico_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDecimal_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo números y punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Solo un punto decimal
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
    }
}