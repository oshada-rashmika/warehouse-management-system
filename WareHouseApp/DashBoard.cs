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

        public DashBoard()
        {
            InitializeComponent();
            InitializeModules();
            this.Load += DashBoard_Load;
        }

        private void InitializeModules()
        {
            customerDash = new CustomerDash();
            customerDash.Location = new Point(277, 79);
            customerDash.Size = new Size(806, 510);
            customerDash.Visible = false;
            this.Controls.Add(customerDash);

            inventoryDash = new InventoryDash();
            inventoryDash.Location = new Point(277, 79);
            inventoryDash.Size = new Size(806, 510);
            inventoryDash.Visible = false;
            this.Controls.Add(inventoryDash);

            button1.Text = "Employees";
            button2.Text = "Customers";
            button3.Text = "Inventory";

            try
            {
                string imagePath = @"C:\Users\MSI\Downloads\CS107.3 Object Oriented Programming with C#\WareHouseApp - repeat coursework\WareHouseApp - repeat coursework\WareHouseApp\Resources\logout.png";
                btnLogout.Image = Image.FromFile(imagePath);
                btnLogout.ImageAlign = ContentAlignment.MiddleLeft;
                btnLogout.TextImageRelation = TextImageRelation.ImageBeforeText;
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
        }

        private void ShowModule(UserControl activeModule)
        {
            mainDash1.Visible = false;
            customerDash.Visible = false;
            inventoryDash.Visible = false;

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
