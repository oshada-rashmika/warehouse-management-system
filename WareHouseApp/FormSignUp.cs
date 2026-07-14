using System;
using System.Drawing;
using System.Windows.Forms;

namespace WareHouseApp
{
    public class FormSignUp : Form
    {
        private Panel pnlBackground;
        private Label lblTitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;
        private LinkLabel lnkReturn;

        public FormSignUp()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Register - Warehouse Management System";
            this.ClientSize = new Size(400, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(32, 34, 37);

            pnlBackground = new Panel
            {
                Size = new Size(320, 430),
                Location = new Point(40, 45),
                BackColor = Color.FromArgb(47, 49, 54),
                BorderStyle = BorderStyle.None
            };
            this.Controls.Add(pnlBackground);

            lblTitle = new Label
            {
                Text = "Create Account",
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

            lblConfirmPassword = new Label
            {
                Text = "CONFIRM PASSWORD",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 187, 190),
                Location = new Point(30, 240),
                AutoSize = true
            };
            pnlBackground.Controls.Add(lblConfirmPassword);

            txtConfirmPassword = new TextBox
            {
                Location = new Point(30, 265),
                Size = new Size(260, 30),
                Font = new Font("Segoe UI", 12f),
                BackColor = Color.FromArgb(64, 68, 75),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '•'
            };
            pnlBackground.Controls.Add(txtConfirmPassword);

            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(30, 325),
                Size = new Size(260, 45),
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                BackColor = Color.FromArgb(67, 181, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            pnlBackground.Controls.Add(btnRegister);

            lnkReturn = new LinkLabel
            {
                Text = "Already have an account? Login here.",
                Location = new Point(0, 385),
                Size = new Size(320, 20),
                Font = new Font("Segoe UI", 9f),
                LinkColor = Color.FromArgb(114, 137, 218),
                ActiveLinkColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            lnkReturn.Click += (s, e) => this.Close();
            pnlBackground.Controls.Add(lnkReturn);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (username.Contains(" "))
            {
                MessageBox.Show("Username must not contain spaces.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please re-enter.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return;
            }

            try
            {
                bool registered = DatabaseHelper.RegisterEmployee(username, password);

                if (registered)
                {
                    MessageBox.Show(
                        $"Account created successfully!\n\nUsername: {username}\n\nYou may now log in.",
                        "Registration Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"The username \"{username}\" is already in use. Please choose a different one.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An unexpected database error occurred during registration.\n\nDetails: {ex.Message}",
                    "System Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
