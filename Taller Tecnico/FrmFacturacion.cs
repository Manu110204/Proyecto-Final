using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace TallerTecnico
{
    public partial class FrmFacturacion : Form
    {
        private int facturaSeleccionadaID = 0;

        public FrmFacturacion()
        {
            InitializeComponent();
            LoadServicios();
            LoadFacturas();
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            dgvFacturas.EnableHeadersVisualStyles = false;
            dgvFacturas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvFacturas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFacturas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvFacturas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void LoadServicios()
        {
            string query = @"SELECT s.ServicioID, 
                            c.NombreCompleto + ' - ' + e.TipoEquipo + ' ' + e.Marca AS Servicio
                            FROM Servicios s
                            INNER JOIN Clientes c ON s.ClienteID = c.ClienteID
                            INNER JOIN Equipos e ON s.EquipoID = e.EquipoID
                            WHERE s.Estado = 'Reparado' 
                            AND s.ServicioID NOT IN (SELECT ServicioID FROM Facturas WHERE Estado != 'Anulada')
                            ORDER BY s.FechaIngreso DESC";

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);
            if (dt != null)
            {
                cmbServicio.DataSource = dt;
                cmbServicio.DisplayMember = "Servicio";
                cmbServicio.ValueMember = "ServicioID";
                cmbServicio.SelectedIndex = -1;
            }
        }

        private void cmbServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbServicio.SelectedValue != null && cmbServicio.SelectedValue.GetType() == typeof(int))
            {
                int servicioID = Convert.ToInt32(cmbServicio.SelectedValue);
                string query = @"SELECT s.CostoTotal, c.NombreCompleto, c.DNI
                                FROM Servicios s
                                INNER JOIN Clientes c ON s.ClienteID = c.ClienteID
                                WHERE s.ServicioID = @ServicioID";

                SqlParameter[] parameters = { new SqlParameter("@ServicioID", servicioID) };
                DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    decimal costoTotal = Convert.ToDecimal(dt.Rows[0]["CostoTotal"]);
                    txtCliente.Text = dt.Rows[0]["NombreCompleto"].ToString();
                    txtDNI.Text = dt.Rows[0]["DNI"].ToString();

                    decimal subtotal = costoTotal / 1.18m;
                    decimal igv = costoTotal - subtotal;

                    txtSubtotal.Text = subtotal.ToString("N2");
                    txtIGV.Text = igv.ToString("N2");
                    txtTotal.Text = costoTotal.ToString("N2");
                }
            }
        }

        private void LoadFacturas()
        {
            string query = @"SELECT f.FacturaID, f.NumeroFactura, f.FechaEmision, 
                            c.NombreCompleto AS Cliente, f.Total, f.FormaPago, f.Estado
                            FROM Facturas f
                            INNER JOIN Clientes c ON f.ClienteID = c.ClienteID
                            ORDER BY f.FechaEmision DESC";

            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query);
            if (dt != null)
            {
                dgvFacturas.DataSource = dt;
                dgvFacturas.Columns["FacturaID"].Visible = false;
                dgvFacturas.Columns["NumeroFactura"].HeaderText = "N° Factura";
                dgvFacturas.Columns["FechaEmision"].HeaderText = "Fecha";
                dgvFacturas.Columns["Cliente"].HeaderText = "Cliente";
                dgvFacturas.Columns["Total"].HeaderText = "Total";
                dgvFacturas.Columns["FormaPago"].HeaderText = "Forma Pago";
                dgvFacturas.Columns["Estado"].HeaderText = "Estado";

                lblTotal.Text = $"Total de facturas: {dt.Rows.Count}";
            }
        }

        private string GenerarNumeroFactura()
        {
            string query = "SELECT COUNT(*) FROM Facturas";
            object result = DatabaseConnection.ExecuteScalar(query);
            int count = result != null ? Convert.ToInt32(result) : 0;
            return $"F{DateTime.Now:yyyyMM}-{(count + 1):D4}";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbServicio.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un servicio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbFormaPago.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione la forma de pago", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string numeroFactura = GenerarNumeroFactura();
                int servicioID = Convert.ToInt32(cmbServicio.SelectedValue);

                // Obtener ClienteID
                string queryCliente = "SELECT ClienteID FROM Servicios WHERE ServicioID = @ServicioID";
                SqlParameter[] paramsCliente = { new SqlParameter("@ServicioID", servicioID) };
                object clienteIDResult = DatabaseConnection.ExecuteScalar(queryCliente, paramsCliente);
                int clienteID = Convert.ToInt32(clienteIDResult);

                string query = @"INSERT INTO Facturas 
                               (NumeroFactura, ServicioID, ClienteID, Subtotal, IGV, Total, 
                               FormaPago, Estado, UsuarioRegistro)
                               VALUES 
                               (@NumeroFactura, @ServicioID, @ClienteID, @Subtotal, @IGV, @Total, 
                               @FormaPago, 'Pagada', @UsuarioID)";

                SqlParameter[] parameters = {
                    new SqlParameter("@NumeroFactura", numeroFactura),
                    new SqlParameter("@ServicioID", servicioID),
                    new SqlParameter("@ClienteID", clienteID),
                    new SqlParameter("@Subtotal", decimal.Parse(txtSubtotal.Text)),
                    new SqlParameter("@IGV", decimal.Parse(txtIGV.Text)),
                    new SqlParameter("@Total", decimal.Parse(txtTotal.Text)),
                    new SqlParameter("@FormaPago", cmbFormaPago.SelectedItem.ToString()),
                    new SqlParameter("@UsuarioID", SessionData.UsuarioID)
                };

                if (DatabaseConnection.ExecuteQuery(query, parameters))
                {
                    MessageBox.Show($"Factura {numeroFactura} generada exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFacturas();
                    LoadServicios();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar factura: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            cmbServicio.SelectedIndex = -1;
            txtCliente.Clear();
            txtDNI.Clear();
            txtSubtotal.Text = "0.00";
            txtIGV.Text = "0.00";
            txtTotal.Text = "0.00";
            cmbFormaPago.SelectedIndex = -1;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadFacturas();
                return;
            }

            string query = @"SELECT f.FacturaID, f.NumeroFactura, f.FechaEmision, 
                            c.NombreCompleto AS Cliente, f.Total, f.FormaPago, f.Estado
                            FROM Facturas f
                            INNER JOIN Clientes c ON f.ClienteID = c.ClienteID
                            WHERE f.NumeroFactura LIKE @Search OR c.NombreCompleto LIKE @Search
                            ORDER BY f.FechaEmision DESC";

            SqlParameter[] parameters = { new SqlParameter("@Search", "%" + searchText + "%") };
            DataTable dt = DatabaseConnection.ExecuteQueryDataTable(query, parameters);

            if (dt != null)
            {
                dgvFacturas.DataSource = dt;
                lblTotal.Text = $"Facturas encontradas: {dt.Rows.Count}";
            }
        }
    }
}