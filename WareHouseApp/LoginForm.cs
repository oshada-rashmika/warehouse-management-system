using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WareHouseApp
{
    public class LoginForm : Form
    {
        private Panel pnlBackground;
        private Label lblTitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
        private LinkLabel lnkSignUp;

        public LoginForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Login - Warehouse Management System";
            this.ClientSize = new Size(400, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(32, 34, 37);

            pnlBackground = new Panel
            {
                Size = new Size(320, 360),
                Location = new Point(40, 45),
                BackColor = Color.FromArgb(47, 49, 54),
                BorderStyle = BorderStyle.None
            };
            this.Controls.Add(pnlBackground);

            lblTitle = new Label
            {
                Text = "Welcome Back",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(320, 40),
                Location = new Point(0, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlBackground.Controls.Add(lblTitle);

            lblUsername = new Label
            {
                Text = "USERNAME",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 187, 190),
                Location = new Point(30, 90),
                AutoSize = true
            };
            pnlBackground.Controls.Add(lblUsername);

            txtUsername = new TextBox
            {
                Location = new Point(30, 115),
                Size = new Size(260, 30),
                Font = new Font("Segoe UI", 12f),
                BackColor = Color.FromArgb(64, 68, 75),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlBackground.Controls.Add(txtUsername);

            lblPassword = new Label
            {
                Text = "PASSWORD",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 187, 190),
                Location = new Point(30, 165),
                AutoSize = true
            };
            pnlBackground.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Location = new Point(30, 190),
                Size = new Size(260, 30),
                Font = new Font("Segoe UI", 12f),
                BackColor = Color.FromArgb(64, 68, 75),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '•'
            };
            pnlBackground.Controls.Add(txtPassword);

            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(30, 250),
                Size = new Size(260, 45),
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                BackColor = Color.FromArgb(114, 137, 218), 
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            pnlBackground.Controls.Add(btnLogin);

            lnkSignUp = new LinkLabel
            {
                Text = "Need an account? Register here.",
                Location = new Point(0, 310),
                Size = new Size(320, 20),
                Font = new Font("Segoe UI", 9f),
                LinkColor = Color.FromArgb(114, 137, 218),
                ActiveLinkColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            lnkSignUp.Click += LnkSignUp_Click;
            pnlBackground.Controls.Add(lnkSignUp);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "SELECT COUNT(1) FROM Person WHERE Username = @User AND Password = @Pass";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@User", username),
                    new SqlParameter("@Pass", password)
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);

                if (result != null && Convert.ToInt32(result) > 0)
                {
                    MainDash mainDashboard = new MainDash();
                    mainDashboard.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A database error occurred during login:\n\n{ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LnkSignUp_Click(object sender, EventArgs e)
        {
            FormSignUp signUpForm = new FormSignUp();
            signUpForm.ShowDialog(this);
        }
    }
}
