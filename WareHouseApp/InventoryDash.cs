using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WareHouseApp
{
    public class InventoryDash : UserControl
    {
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private TextBox txtMaterialName;
        private TextBox txtQuantity;
        private TextBox txtMaterialId;
        private TextBox txtNewQuantity;
        private DataGridView gridInventory;

        public InventoryDash()
        {
            InitializeLayout();
            InitializeInventoryManagementUI();
            RefreshData();
        }

        private void InitializeLayout()
        {
            this.Size = new Size(806, 510);
            this.BackColor = SystemColors.Control;

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                Padding = new Padding(20)
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 220f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            panel1 = new Panel { BackColor = SystemColors.ButtonHighlight, Dock = DockStyle.Fill, Margin = new Padding(10) };
            panel2 = new Panel { BackColor = SystemColors.ButtonHighlight, Dock = DockStyle.Fill, Margin = new Padding(10) };
            panel3 = new Panel { BackColor = SystemColors.ButtonHighlight, Dock = DockStyle.Fill, Margin = new Padding(10), Padding = new Padding(20, 50, 20, 20) };

            mainLayout.Controls.Add(panel1, 0, 0);
            mainLayout.Controls.Add(panel2, 1, 0);
            mainLayout.Controls.Add(panel3, 0, 1);
            mainLayout.SetColumnSpan(panel3, 2);

            this.Controls.Add(mainLayout);
        }

        private void InitializeInventoryManagementUI()
        {
            Label lblCard1Title = new Label { Text = "Log New Stock", Font = new Font("Segoe UI", 12f, FontStyle.Bold), Location = new Point(20, 15), AutoSize = true };
            panel1.Controls.Add(lblCard1Title);

            Label lblName = new Label { Text = "Material Name:", Location = new Point(20, 55), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtMaterialName = new TextBox { Location = new Point(130, 53), Size = new Size(180, 25), Font = new Font("Segoe UI", 10f) };
            panel1.Controls.Add(lblName);
            panel1.Controls.Add(txtMaterialName);

            Label lblQuantity = new Label { Text = "Quantity:", Location = new Point(20, 95), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtQuantity = new TextBox { Location = new Point(130, 93), Size = new Size(180, 25), Font = new Font("Segoe UI", 10f) };
            panel1.Controls.Add(lblQuantity);
            panel1.Controls.Add(txtQuantity);

            Button btnLog = new Button
            {
                Text = "Log Stock",
                Location = new Point(130, 135),
                Size = new Size(180, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLog.FlatAppearance.BorderSize = 0;
            btnLog.Click += BtnLog_Click;
            panel1.Controls.Add(btnLog);

            Label lblCard2Title = new Label { Text = "Update Stock Level", Font = new Font("Segoe UI", 12f, FontStyle.Bold), Location = new Point(20, 15), AutoSize = true };
            panel2.Controls.Add(lblCard2Title);

            Label lblId = new Label { Text = "Material ID:", Location = new Point(20, 55), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtMaterialId = new TextBox { Location = new Point(130, 53), Size = new Size(180, 25), Font = new Font("Segoe UI", 10f) };
            panel2.Controls.Add(lblId);
            panel2.Controls.Add(txtMaterialId);

            Label lblNewQty = new Label { Text = "New Quantity:", Location = new Point(20, 95), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtNewQuantity = new TextBox { Location = new Point(130, 93), Size = new Size(180, 25), Font = new Font("Segoe UI", 10f) };
            panel2.Controls.Add(lblNewQty);
            panel2.Controls.Add(txtNewQuantity);

            Button btnUpdate = new Button
            {
                Text = "Update Stock Level",
                Location = new Point(130, 135),
                Size = new Size(180, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += BtnUpdate_Click;
            panel2.Controls.Add(btnUpdate);
            
            Label lblCard3Title = new Label { Text = "Inventory Tracking Database", Font = new Font("Segoe UI", 12f, FontStyle.Bold), Location = new Point(20, 10), AutoSize = true };
            panel3.Controls.Add(lblCard3Title);

            gridInventory = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            panel3.Controls.Add(gridInventory);
        }

        private void RefreshData()
        {
            try
            {
                string query = "SELECT Id, Name, Count FROM Material ORDER BY Id DESC";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                gridInventory.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load inventory data:\n\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            string name = txtMaterialName.Text.Trim();
            string qtyText = txtQuantity.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Material Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(qtyText, out int qty) || qty < 0)
            {
                MessageBox.Show("Quantity must be a valid non-negative integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string insertQuery = "INSERT INTO Material (Name, Count) VALUES (@Name, @Qty)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Qty", qty)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(insertQuery, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Stock logged successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaterialName.Clear();
                    txtQuantity.Clear();
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to log stock. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A database error occurred while logging stock:\n\n{ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            string idText = txtMaterialId.Text.Trim();
            string newQtyText = txtNewQuantity.Text.Trim();

            if (!int.TryParse(idText, out int materialId) || materialId <= 0)
            {
                MessageBox.Show("Please enter a valid Material ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(newQtyText, out int newQty) || newQty < 0)
            {
                MessageBox.Show("New Quantity must be a valid non-negative integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string updateQuery = "UPDATE Material SET Count = @Qty WHERE Id = @Id";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Qty", newQty),
                    new SqlParameter("@Id", materialId)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Stock level updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaterialId.Clear();
                    txtNewQuantity.Clear();
                    RefreshData();
                }
                else
                {
                    MessageBox.Show($"No material found with ID {materialId}.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A database error occurred while updating stock:\n\n{ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
