using System;
using System.Windows.Forms;

namespace WareHouseApp
{
    public class FormSignUp : Form
    {
        private Label    lblTitle;
        private Label    lblUsername;
        private Label    lblPassword;
        private Label    lblConfirmPassword;
        private TextBox  txtUsername;
        private TextBox  txtPassword;
        private TextBox  txtConfirmPassword;
        private Button   btnRegister;
        private Button   btnCancel;

        public FormSignUp()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text            = "Create Account";
            this.ClientSize      = new System.Drawing.Size(480, 360);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = System.Drawing.Color.FromArgb(245, 245, 245);

            lblTitle = new Label
            {
                Text      = "Create New Account",
                Font      = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(5, 146, 18),
                Location  = new System.Drawing.Point(80, 20),
                Size      = new System.Drawing.Size(330, 40),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            lblUsername = MakeLabel("Username:", 90);
            txtUsername = MakeTextBox(90);

            lblPassword = MakeLabel("Password:", 150);
            txtPassword = MakeTextBox(150, isPassword: true);

            lblConfirmPassword = MakeLabel("Confirm Password:", 210);
            txtConfirmPassword = MakeTextBox(210, isPassword: true);

            btnRegister = new Button
            {
                Text      = "Register",
                Location  = new System.Drawing.Point(110, 280),
                Size      = new System.Drawing.Size(110, 38),
                BackColor = System.Drawing.Color.FromArgb(155, 236, 0),
                FlatStyle = FlatStyle.Flat,
                Font      = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold)
            };
            btnRegister.Click += BtnRegister_Click;

            btnCancel = new Button
            {
                Text      = "Cancel",
                Location  = new System.Drawing.Point(240, 280),
                Size      = new System.Drawing.Size(110, 38),
                BackColor = System.Drawing.Color.FromArgb(220, 80, 60),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold)
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitle,
                lblUsername,       txtUsername,
                lblPassword,       txtPassword,
                lblConfirmPassword, txtConfirmPassword,
                btnRegister,       btnCancel
            });
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string username        = txtUsername.Text.Trim();
            string password        = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowWarning("Username cannot be empty.");
                txtUsername.Focus();
                return;
            }

            if (username.Contains(" "))
            {
                ShowWarning("Username must not contain spaces.");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowWarning("Password cannot be empty.");
                txtPassword.Focus();
                return;
            }
            
            if (password.Length < 6)
            {
                ShowWarning("Password must be at least 6 characters long.");
                txtPassword.Focus();
                return;
            }

            if (password != confirmPassword)
            {
                ShowWarning("Passwords do not match. Please re-enter.");
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
                    ShowWarning($"The username \"{username}\" is already in use. Please choose a different one.");
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An unexpected error occurred during registration.\n\nDetails: {ex.Message}",
                    "System Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static Label MakeLabel(string text, int top)
        {
            return new Label
            {
                Text      = text,
                Location  = new System.Drawing.Point(40, top + 5),
                Size      = new System.Drawing.Size(150, 25),
                Font      = new System.Drawing.Font("Microsoft Sans Serif", 10f),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
        }

        private static TextBox MakeTextBox(int top, bool isPassword = false)
        {
            return new TextBox
            {
                Location     = new System.Drawing.Point(200, top),
                Size         = new System.Drawing.Size(230, 32),
                Font         = new System.Drawing.Font("Microsoft Sans Serif", 11f),
                PasswordChar = isPassword ? '*' : '\0'
            };
        }
    }
}
