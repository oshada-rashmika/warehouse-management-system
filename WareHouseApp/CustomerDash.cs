using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WareHouseApp
{
    public class CustomerDash : UserControl
    {
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private TextBox txtCustomerName;
        private TextBox txtContactNumber;
        private TextBox txtSearch;
        private DataGridView gridCustomers;

        public CustomerDash()
        {
            InitializeLayout();
            InitializeCustomerManagementUI();
            RefreshData();
        }

        private void InitializeLayout()
        {
            this.Size = new Size(806, 510);
            this.BackColor = SystemColors.Control;

            panel1 = new Panel { BackColor = SystemColors.ButtonHighlight, Location = new Point(42, 45), Size = new Size(330, 189) };
            panel2 = new Panel { BackColor = SystemColors.ButtonHighlight, Location = new Point(444, 45), Size = new Size(330, 189) };
            panel3 = new Panel { BackColor = SystemColors.ButtonHighlight, Location = new Point(42, 290), Size = new Size(732, 189) };

            this.Controls.Add(panel1);
            this.Controls.Add(panel2);
            this.Controls.Add(panel3);
        }

        private void InitializeCustomerManagementUI()
        {
            Label lblCard1Title = new Label { Text = "Add New Customer", Font = new Font("Segoe UI", 12f, FontStyle.Bold), Location = new Point(20, 15), AutoSize = true };
            panel1.Controls.Add(lblCard1Title);

            Label lblName = new Label { Text = "Customer Name:", Location = new Point(20, 55), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtCustomerName = new TextBox { Location = new Point(130, 53), Size = new Size(180, 25), Font = new Font("Segoe UI", 10f) };
            panel1.Controls.Add(lblName);
            panel1.Controls.Add(txtCustomerName);

            Label lblContact = new Label { Text = "Contact Number:", Location = new Point(20, 95), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtContactNumber = new TextBox { Location = new Point(130, 93), Size = new Size(180, 25), Font = new Font("Segoe UI", 10f) };
            panel1.Controls.Add(lblContact);
            panel1.Controls.Add(txtContactNumber);

            Button btnSave = new Button
            {
                Text = "Save Customer",
                Location = new Point(130, 135),
                Size = new Size(180, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            panel1.Controls.Add(btnSave);

            Label lblCard2Title = new Label { Text = "Search Customer", Font = new Font("Segoe UI", 12f, FontStyle.Bold), Location = new Point(20, 15), AutoSize = true };
            panel2.Controls.Add(lblCard2Title);

            Label lblSearch = new Label { Text = "Name:", Location = new Point(20, 65), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtSearch = new TextBox { Location = new Point(75, 63), Size = new Size(225, 25), Font = new Font("Segoe UI", 10f) };
            panel2.Controls.Add(lblSearch);
            panel2.Controls.Add(txtSearch);

            Button btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(75, 105),
                Size = new Size(225, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            panel2.Controls.Add(btnSearch);

            Label lblCard3Title = new Label { Text = "Active Customers", Font = new Font("Segoe UI", 12f, FontStyle.Bold), Location = new Point(20, 10), AutoSize = true };
            panel3.Controls.Add(lblCard3Title);

            gridCustomers = new DataGridView
            {
                Location = new Point(20, 40),
                Size = new Size(690, 130),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            panel3.Controls.Add(gridCustomers);
        }

        private void RefreshData(string searchTerm = "")
        {
            try
            {
                string query;
                SqlParameter[] parameters = null;

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = "SELECT CustomerId, CustomerName, ContactNumber FROM Customer ORDER BY CustomerId DESC";
                }
                else
                {
                    query = "SELECT CustomerId, CustomerName, ContactNumber FROM Customer WHERE CustomerName LIKE @SearchTerm ORDER BY CustomerId DESC";
                    parameters = new SqlParameter[]
                    {
                        new SqlParameter("@SearchTerm", "%" + searchTerm + "%")
                    };
                }

                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
                gridCustomers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load customer data:\n\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string name = txtCustomerName.Text.Trim();
            string contact = txtContactNumber.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Customer Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string insertQuery = "INSERT INTO Customer (CustomerName, ContactNumber) VALUES (@Name, @Contact)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Contact", string.IsNullOrWhiteSpace(contact) ? (object)DBNull.Value : contact)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(insertQuery, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Customer saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCustomerName.Clear();
                    txtContactNumber.Clear();
                    
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to save customer. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A database error occurred while saving the customer:\n\n{ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            RefreshData(searchTerm);
        }
    }
}
