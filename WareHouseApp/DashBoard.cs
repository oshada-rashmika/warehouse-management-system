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

        private void ShowModule(Control activeModule)
        {
            workspacePanel.Controls.Clear();

            if (activeModule is Form childForm)
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
            }

            activeModule.Dock = DockStyle.Fill;
            workspacePanel.Controls.Add(activeModule);
            activeModule.Visible = true;
            activeModule.BringToFront();
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
