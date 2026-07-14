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

        public MainDash()
        {
            InitializeComponent();
            InitializeEmployeeManagementUI();
            RefreshData();
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
                Text = "Add New Employee",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Location = new Point(20, 10),
                AutoSize = true
            };
            this.panel2.Controls.Add(lblCard2Title);

            Label lblUser = new Label { Text = "Username:", Location = new Point(20, 50), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtNewUsername = new TextBox { Location = new Point(100, 48), Size = new Size(180, 25), Font = new Font("Segoe UI", 10f) };
            this.panel2.Controls.Add(lblUser);
            this.panel2.Controls.Add(txtNewUsername);

            Label lblPass = new Label { Text = "Password:", Location = new Point(20, 90), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            txtNewPassword = new TextBox { Location = new Point(100, 88), Size = new Size(180, 25), Font = new Font("Segoe UI", 10f), PasswordChar = '•' };
            this.panel2.Controls.Add(lblPass);
            this.panel2.Controls.Add(txtNewPassword);

            Button btnAdd = new Button
            {
                Text = "Add Employee",
                Location = new Point(100, 130),
                Size = new Size(180, 35),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            this.panel2.Controls.Add(btnAdd);

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
            this.panel3.Controls.Add(gridEmployees);
        }

        private void RefreshData()
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
    }
}
