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
        private TextBox txtCity;
        private TextBox txtSearch;
        private DataGridView gridCustomers;
        private int selectedCustomerId = -1;

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

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                Padding = new Padding(20)
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 240f));
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

        private void InitializeCustomerManagementUI()
        {
            Label lblCard1Title = new Label { Text = "Manage Customer", Font = new Font("Segoe UI", 12f, FontStyle.Bold), Location = new Point(20, 15), AutoSize = true };
            panel1.Controls.Add(lblCard1Title);

            Label lblName = new Label { Text = "Customer Name:", Location = new Point(20, 55), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtCustomerName = new TextBox { Location = new Point(130, 53), Size = new Size(260, 25), Font = new Font("Segoe UI", 10f) };
            panel1.Controls.Add(lblName);
            panel1.Controls.Add(txtCustomerName);

            Label lblContact = new Label { Text = "Contact Number:", Location = new Point(20, 95), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtContactNumber = new TextBox { Location = new Point(130, 93), Size = new Size(260, 25), Font = new Font("Segoe UI", 10f) };
            panel1.Controls.Add(lblContact);
            panel1.Controls.Add(txtContactNumber);

            Label lblCity = new Label { Text = "City:", Location = new Point(20, 135), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtCity = new TextBox { Location = new Point(130, 133), Size = new Size(260, 25), Font = new Font("Segoe UI", 10f) };
            panel1.Controls.Add(lblCity);
            panel1.Controls.Add(txtCity);

            Button btnAdd = new Button
            {
                Text = "Add",
                Location = new Point(130, 173),
                Size = new Size(80, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            panel1.Controls.Add(btnAdd);

            Button btnUpdate = new Button
            {
                Text = "Update",
                Location = new Point(220, 173),
                Size = new Size(80, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += BtnUpdate_Click;
            panel1.Controls.Add(btnUpdate);

            Button btnDelete = new Button
            {
                Text = "Delete",
                Location = new Point(310, 173),
                Size = new Size(80, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            panel1.Controls.Add(btnDelete);

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

            Button btnPrint = new Button
            {
                Text = "Print Report",
                Location = new Point(600, 10),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += (s, e) => TablePrinter.PrintDataGridView(gridCustomers, "Active Customers Report");
            this.panel3.Controls.Add(btnPrint);

            gridCustomers = new DataGridView
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
            gridCustomers.SelectionChanged += GridCustomers_SelectionChanged;
            panel3.Controls.Add(gridCustomers);
        }

        public void RefreshData(string searchTerm = "")
        {
            try
            {
                string query;
                SqlParameter[] parameters = null;

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = "SELECT CustomerId, CustomerName, ContactNumber, City FROM Customer ORDER BY CustomerId DESC";
                }
                else
                {
                    query = "SELECT CustomerId, CustomerName, ContactNumber, City FROM Customer WHERE CustomerName LIKE @SearchTerm ORDER BY CustomerId DESC";
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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string name = txtCustomerName.Text.Trim();
            string contact = txtContactNumber.Text.Trim();
            string city = txtCity.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Customer Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string insertQuery = "INSERT INTO Customer (CustomerName, ContactNumber, City) VALUES (@Name, @Contact, @City)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Contact", string.IsNullOrWhiteSpace(contact) ? (object)DBNull.Value : contact),
                    new SqlParameter("@City", string.IsNullOrWhiteSpace(city) ? (object)DBNull.Value : city)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(insertQuery, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCustomerName.Clear();
                    txtContactNumber.Clear();
                    txtCity.Clear();
                    
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to add customer. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A database error occurred while adding the customer:\n\n{ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (gridCustomers.SelectedRows.Count > 0)
            {
                var row = gridCustomers.SelectedRows[0];
                if (row.Cells["CustomerId"].Value != null)
                {
                    selectedCustomerId = Convert.ToInt32(row.Cells["CustomerId"].Value);
                    txtCustomerName.Text = row.Cells["CustomerName"].Value?.ToString();
                    txtContactNumber.Text = row.Cells["ContactNumber"].Value?.ToString();
                    txtCity.Text = row.Cells["City"].Value?.ToString();
                }
            }
            else
            {
                selectedCustomerId = -1;
                txtCustomerName.Clear();
                txtContactNumber.Clear();
                txtCity.Clear();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == -1)
            {
                MessageBox.Show("Please select a customer from the table to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string name = txtCustomerName.Text.Trim();
            string contact = txtContactNumber.Text.Trim();
            string city = txtCity.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Customer Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string updateQuery = "UPDATE Customer SET CustomerName = @Name, ContactNumber = @Contact, City = @City WHERE CustomerId = @Id";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Contact", string.IsNullOrWhiteSpace(contact) ? (object)DBNull.Value : contact),
                    new SqlParameter("@City", string.IsNullOrWhiteSpace(city) ? (object)DBNull.Value : city),
                    new SqlParameter("@Id", selectedCustomerId)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCustomerName.Clear();
                    txtContactNumber.Clear();
                    txtCity.Clear();
                    selectedCustomerId = -1;
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to update customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == -1)
            {
                MessageBox.Show("Please select a customer from the table to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string deleteQuery = "DELETE FROM Customer WHERE CustomerId = @Id";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@Id", selectedCustomerId)
                    };

                    int rowsAffected = DatabaseHelper.ExecuteNonQuery(deleteQuery, parameters);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerName.Clear();
                        txtContactNumber.Clear();
                        txtCity.Clear();
                        selectedCustomerId = -1;
                        RefreshData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            RefreshData(searchTerm);
        }
    }
}
