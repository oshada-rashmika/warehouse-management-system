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
    public partial class DashBoard : Form
    {
        private CustomerDash customerDash;
        private InventoryDash inventoryDash;
        private Panel workspacePanel;
        private ContextMenuStrip profileMenu;
        private bool isDarkMode = false;

        public DashBoard()
        {
            InitializeComponent();
            
            workspacePanel = new Panel();
            workspacePanel.Location = mainDash1.Location;
            workspacePanel.Size = mainDash1.Size;
            workspacePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(workspacePanel);

            this.MinimumSize = new Size(1095, 595);
            this.Controls.Remove(mainDash1);

            InitializeModules();
            this.Load += DashBoard_Load;
        }

        private void InitializeModules()
        {
            customerDash = new CustomerDash();
            inventoryDash = new InventoryDash();

            button1.Text = "Employees";
            button2.Text = "Customers";
            button3.Text = "Inventory";
            button8.Text = "Settings";
            button8.Width = 90;
            button8.Left = 590;

            button7.Text = "Profile";
            button7.Width = 115;
            button7.Left = 690;
            
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = System.IO.Path.Combine(baseDir, "Resources", "logout.png");
                
                if (!System.IO.File.Exists(imagePath))
                {
                    imagePath = System.IO.Path.Combine(baseDir, "..", "..", "Resources", "logout.png");
                }

                if (System.IO.File.Exists(imagePath))
                {
                    Image original = Image.FromFile(imagePath);
                    btnLogout.Image = new Bitmap(original, new Size(24, 24));
                    btnLogout.ImageAlign = ContentAlignment.MiddleCenter;
                    btnLogout.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btnLogout.ImageAlign = ContentAlignment.MiddleLeft;
                    btnLogout.Padding = new Padding(60, 0, 0, 0);
                }
                else
                {
                    Console.WriteLine("[UI Warning] logout.png not found in build directory or project root resources.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UI Warning] Could not load logout icon: {ex.Message}");
            }
            
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;
            btnLogout.Click += BtnLogout_Click;
            
            button8.Click += ComingSoon_Click;
            button9.Click += ComingSoon_Click;
        }

        private void ComingSoon_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            SessionManager.ClearSession();

            LoginForm newLogin = new LoginForm();
            newLogin.Show();

            this.Close();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            if (SessionManager.Role != "Admin")
            {
                button1.Visible = false;
                ShowModule(inventoryDash);
            }
            else
            {
                ShowModule(mainDash1);
            }
            
            AdjustHeaderLayout();

            LoadProfileIcon();
            InitializeProfileDropdown();
        }

        private void AdjustHeaderLayout()
        {
            int margin = 12;
            int spacing = 10;

            button7.Width = 115;
            button7.Left = panel2.Width - button7.Width - margin;

            button8.Width = 90;
            button8.Left = button7.Left - button8.Width - spacing;
        }

        private void LoadProfileIcon()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string filename = SessionManager.Role == "Admin" ? "administrator.png" : "user.png";
                string imagePath = System.IO.Path.Combine(baseDir, "Resources", filename);

                if (!System.IO.File.Exists(imagePath))
                {
                    imagePath = System.IO.Path.Combine(baseDir, "..", "..", "Resources", filename);
                }

                if (System.IO.File.Exists(imagePath))
                {
                    Image original = Image.FromFile(imagePath);
                    button7.Image = new Bitmap(original, new Size(24, 24));
                    button7.ImageAlign = ContentAlignment.MiddleLeft;
                    button7.TextImageRelation = TextImageRelation.ImageBeforeText;
                    button7.Padding = new Padding(8, 0, 0, 0); // Nicely align the icon inside the button
                }
                else
                {
                    Console.WriteLine($"[UI Warning] Profile icon '{filename}' not found in resources folder.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UI Warning] Failed to load profile icon: {ex.Message}");
            }
        }

        private void InitializeProfileDropdown()
        {
            profileMenu = new ContextMenuStrip();
            
            ToolStripMenuItem userItem = new ToolStripMenuItem($"User: {SessionManager.CurrentUser}");
            userItem.Enabled = false; 
            
            ToolStripMenuItem roleItem = new ToolStripMenuItem($"Role: {SessionManager.Role}");
            roleItem.Enabled = false;
            
            profileMenu.Items.Add(userItem);
            profileMenu.Items.Add(roleItem);
            profileMenu.Items.Add(new ToolStripSeparator());
            
            ToolStripMenuItem logoutItem = new ToolStripMenuItem("Log Out");
            logoutItem.Click += BtnLogout_Click;
            profileMenu.Items.Add(logoutItem);
            
            profileMenu.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem darkModeItem = new ToolStripMenuItem("Toggle Dark/Light Mode");
            darkModeItem.Click += ToggleDarkMode_Click;
            profileMenu.Items.Add(darkModeItem);
            
            button7.Click += Button7_Click;
        }

        private void ToggleDarkMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            Color darkBg = Color.FromArgb(45, 45, 48);
            Color darkPanel = Color.FromArgb(30, 30, 30);
            Color darkText = Color.White;

            Color lightBg = SystemColors.Control;
            Color lightPanel1 = Color.RoyalBlue;
            Color lightPanel2 = Color.Silver;
            Color lightText = SystemColors.ControlText;

            this.BackColor = isDarkMode ? darkBg : lightBg;
            this.ForeColor = isDarkMode ? darkText : lightText;
            
            panel1.BackColor = isDarkMode ? darkPanel : lightPanel1;
            panel2.BackColor = isDarkMode ? Color.FromArgb(20, 20, 20) : lightPanel2;

            ApplyThemeRecursive(this, isDarkMode, darkBg, darkPanel, darkText, lightBg, lightText);
            
            foreach (Control c in panel1.Controls)
            {
                if (c is Button btn)
                {
                    btn.BackColor = isDarkMode ? Color.FromArgb(60, 60, 60) : SystemColors.Control;
                    btn.ForeColor = isDarkMode ? darkText : SystemColors.ControlText;
                }
            }
            foreach (Control c in panel2.Controls)
            {
                if (c is Button btn)
                {
                    btn.BackColor = isDarkMode ? Color.FromArgb(60, 60, 60) : SystemColors.Control;
                    btn.ForeColor = isDarkMode ? darkText : SystemColors.ControlText;
                }
            }
        }

        private void ApplyThemeRecursive(Control parent, bool isDark, Color darkBg, Color darkPanel, Color darkText, Color lightBg, Color lightText)
        {
            foreach (Control c in parent.Controls)
            {
                if (c == panel1 || c == panel2) continue;
                
                if (c is Form || c is UserControl || c is Panel || c is TableLayoutPanel)
                {
                    c.BackColor = isDark ? darkBg : lightBg;
                    c.ForeColor = isDark ? darkText : lightText;
                }
                else if (c is Button btn)
                {
                    if (btn.BackColor != Color.RoyalBlue)
                    {
                        btn.BackColor = isDark ? Color.FromArgb(60, 60, 60) : SystemColors.Control;
                        btn.ForeColor = isDark ? darkText : SystemColors.ControlText;
                    }
                }
                else if (c is TextBox || c is ComboBox)
                {
                    c.BackColor = isDark ? Color.FromArgb(60, 60, 60) : SystemColors.Window;
                    c.ForeColor = isDark ? darkText : SystemColors.WindowText;
                }
                else if (c is DataGridView dgv)
                {
                    dgv.BackgroundColor = isDark ? darkPanel : Color.White;
                    dgv.DefaultCellStyle.BackColor = isDark ? darkBg : Color.White;
                    dgv.DefaultCellStyle.ForeColor = isDark ? darkText : SystemColors.ControlText;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = isDark ? darkPanel : SystemColors.Control;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? darkText : SystemColors.ControlText;
                    dgv.EnableHeadersVisualStyles = false;
                }
                else if (c is Label lbl)
                {
                    if (lbl.ForeColor == Color.RoyalBlue && isDark)
                        lbl.ForeColor = Color.LightSkyBlue;
                    else if (lbl.ForeColor == Color.LightSkyBlue && !isDark)
                        lbl.ForeColor = Color.RoyalBlue;
                    else
                        lbl.ForeColor = isDark ? darkText : lightText;
                }
                
                if (c.HasChildren)
                {
                    ApplyThemeRecursive(c, isDark, darkBg, darkPanel, darkText, lightBg, lightText);
                }
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            profileMenu.Show(button7, new Point(0, button7.Height));
        }

        private void ShowModule(Control activeModule)
        {
            workspacePanel.Controls.Clear();

            if (activeModule is Form childForm)
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
            }
            
            if (activeModule is CustomerDash customerDash)
            {
                customerDash.RefreshData();
            }
            else if (activeModule is InventoryDash inventoryDash)
            {
                inventoryDash.RefreshData();
            }
            else if (activeModule is MainDash mainDash)
            {
                mainDash.RefreshData();
            }

            activeModule.Dock = DockStyle.Fill;
            workspacePanel.Controls.Add(activeModule);
            activeModule.Visible = true;
            activeModule.BringToFront();
            
            if (isDarkMode)
            {
                ApplyTheme();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ShowModule(mainDash1);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ShowModule(customerDash);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            ShowModule(inventoryDash);
        }
    }
}
