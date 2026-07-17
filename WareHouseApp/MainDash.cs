using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WareHouseApp
{
    public partial class MainDash : UserControl
    {
        private Label lblTotalEmployees;
        private TextBox txtNewUsername;
        private TextBox txtNewPassword;
        private DataGridView gridEmployees;
        private int selectedEmpId = -1;

        public MainDash()
        {
            InitializeComponent();
            InitializeLayout();
            InitializeEmployeeManagementUI();
            RefreshData();
        }

        private void InitializeLayout()
        {
            this.Controls.Remove(panel1);
            this.Controls.Remove(panel2);
            this.Controls.Remove(panel3);

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

            panel1.Dock = DockStyle.Fill;
            panel1.Margin = new Padding(10);

            panel2.Dock = DockStyle.Fill;
            panel2.Margin = new Padding(10);

            panel3.Dock = DockStyle.Fill;
            panel3.Margin = new Padding(10);
            panel3.Padding = new Padding(20, 50, 20, 20);

            mainLayout.Controls.Add(panel1, 0, 0);
            mainLayout.Controls.Add(panel2, 1, 0);
            mainLayout.Controls.Add(panel3, 0, 1);
            mainLayout.SetColumnSpan(panel3, 2);

            this.Controls.Add(mainLayout);
        }

        private void InitializeEmployeeManagementUI()
        {
            Label lblCard1Title = new Label
            {
                Text = "Total Employees",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.panel1.Controls.Add(lblCard1Title);

            lblTotalEmployees = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 36f, FontStyle.Bold),
                ForeColor = Color.RoyalBlue,
                Location = new Point(20, 60),
                AutoSize = true
            };
            this.panel1.Controls.Add(lblTotalEmployees);

            Label lblCard2Title = new Label
            {
                Text = "Manage Employee",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Location = new Point(20, 10),
                AutoSize = true
            };
            this.panel2.Controls.Add(lblCard2Title);

            Label lblUser = new Label { Text = "Username:", Location = new Point(20, 50), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtNewUsername = new TextBox { Location = new Point(120, 48), Size = new Size(245, 25), Font = new Font("Segoe UI", 10f) };
            this.panel2.Controls.Add(lblUser);
            this.panel2.Controls.Add(txtNewUsername);

            Label lblPass = new Label { Text = "Password:", Location = new Point(20, 90), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtNewPassword = new TextBox { Location = new Point(120, 88), Size = new Size(245, 25), Font = new Font("Segoe UI", 10f), PasswordChar = '•' };
            this.panel2.Controls.Add(lblPass);
            this.panel2.Controls.Add(txtNewPassword);

            Button btnAdd = new Button
            {
                Text = "Add",
                Location = new Point(120, 130),
                Size = new Size(75, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            this.panel2.Controls.Add(btnAdd);

            Button btnUpdate = new Button
            {
                Text = "Update",
                Location = new Point(205, 130),
                Size = new Size(75, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += BtnUpdate_Click;
            this.panel2.Controls.Add(btnUpdate);

            Button btnDelete = new Button
            {
                Text = "Delete",
                Location = new Point(290, 130),
                Size = new Size(75, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            this.panel2.Controls.Add(btnDelete);

            Label lblCard3Title = new Label
            {
                Text = "Registered Employees Database",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Location = new Point(20, 10),
                AutoSize = true
            };
            this.panel3.Controls.Add(lblCard3Title);

            gridEmployees = new DataGridView
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
            gridEmployees.SelectionChanged += GridEmployees_SelectionChanged;
            this.panel3.Controls.Add(gridEmployees);
        }

        public void RefreshData()
        {
            try
            {
                string countQuery = "SELECT COUNT(1) FROM Person";
                object countResult = DatabaseHelper.ExecuteScalar(countQuery);
                lblTotalEmployees.Text = (countResult != null) ? countResult.ToString() : "0";
                string gridQuery = "SELECT EmpId, Username FROM Person ORDER BY EmpId DESC";
                DataTable dt = DatabaseHelper.ExecuteQuery(gridQuery);
                gridEmployees.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to refresh employee data:\n\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text.Trim();
            string password = txtNewPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Both Username and Password are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (DatabaseHelper.IsUsernameTaken(username))
                {
                    MessageBox.Show($"The username '{username}' is already in use.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool success = DatabaseHelper.RegisterEmployee(username, password);
                if (success)
                {
                    MessageBox.Show("Employee added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNewUsername.Clear();
                    txtNewPassword.Clear();
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to add employee. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A database error occurred while adding the employee:\n\n{ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (gridEmployees.SelectedRows.Count > 0)
            {
                var row = gridEmployees.SelectedRows[0];
                if (row.Cells["EmpId"].Value != null)
                {
                    selectedEmpId = Convert.ToInt32(row.Cells["EmpId"].Value);
                    txtNewUsername.Text = row.Cells["Username"].Value.ToString();
                    txtNewPassword.Clear();
                }
            }
            else
            {
                selectedEmpId = -1;
                txtNewUsername.Clear();
                txtNewPassword.Clear();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedEmpId == -1)
            {
                MessageBox.Show("Please select an employee from the table to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string username = txtNewUsername.Text.Trim();
            string password = txtNewPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool success = DatabaseHelper.UpdateEmployee(selectedEmpId, username, password);
                if (success)
                {
                    MessageBox.Show("Employee updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNewUsername.Clear();
                    txtNewPassword.Clear();
                    selectedEmpId = -1;
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to update employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedEmpId == -1)
            {
                MessageBox.Show("Please select an employee from the table to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = DatabaseHelper.DeleteEmployee(selectedEmpId);
                    if (success)
                    {
                        MessageBox.Show("Employee deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtNewUsername.Clear();
                        txtNewPassword.Clear();
                        selectedEmpId = -1;
                        RefreshData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
