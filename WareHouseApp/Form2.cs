using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WareHouseApp.People;
using System.Data.SqlClient;

namespace WareHouseApp
{
    public partial class Form2 : Form
    {
        private Panel dynamicPanel;
        private ShippingOperator shippingOperator = new ShippingOperator();

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            LoadCustomerManagementView();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadWarehouseOperationsView();
        }

        private void HideDashboard()
        {
            button2.Visible = false; button3.Visible = false; button4.Visible = false;
            button5.Visible = false; button6.Visible = false; button7.Visible = false;
            pictureBox2.Visible = false; pictureBox3.Visible = false; pictureBox4.Visible = false;
            pictureBox5.Visible = false; pictureBox6.Visible = false; pictureBox7.Visible = false;

            if (dynamicPanel != null)
            {
                dynamicPanel.Dispose();
            }
            dynamicPanel = new Panel();
            dynamicPanel.Location = new Point(230, 90);
            dynamicPanel.Size = new Size(850, 600);
            this.Controls.Add(dynamicPanel);
            dynamicPanel.BringToFront();
        }

        private void ShowDashboard()
        {
            if (dynamicPanel != null)
            {
                dynamicPanel.Dispose();
            }
            button2.Visible = true; button3.Visible = true; button4.Visible = true;
            button5.Visible = true; button6.Visible = true; button7.Visible = true;
            pictureBox2.Visible = true; pictureBox3.Visible = true; pictureBox4.Visible = true;
            pictureBox5.Visible = true; pictureBox6.Visible = true; pictureBox7.Visible = true;
        }

        private void LoadWarehouseOperationsView()
        {
            HideDashboard();

            Label title = new Label() { Text = "Warehouse Operations", Font = new Font("Arial", 16, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            dynamicPanel.Controls.Add(title);

            Button btnBack = new Button() { Text = "Back to Dashboard", Location = new Point(650, 20), Size = new Size(150, 30) };
            btnBack.Click += (s, ev) => ShowDashboard();
            dynamicPanel.Controls.Add(btnBack);

            DataGridView grid = new DataGridView() { Location = new Point(20, 70), Size = new Size(800, 400), AllowUserToAddRows = false, ReadOnly = true };
            dynamicPanel.Controls.Add(grid);

            Action refreshGrid = () => { grid.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM Materials"); };
            refreshGrid();

            Button btnLoad = new Button() { Text = "Load Stock", Location = new Point(20, 490), Size = new Size(120, 35) };
            Button btnShip = new Button() { Text = "Ship Stock", Location = new Point(160, 490), Size = new Size(120, 35) };
            
            btnLoad.Click += (s, ev) => {
                if (grid.SelectedRows.Count > 0)
                {
                    int materialID = Convert.ToInt32(grid.SelectedRows[0].Cells["MaterialID"].Value);
                    shippingOperator.LoadStocks(materialID, 10, 1); // hardcoded qty 10, emp 1 for demo
                    refreshGrid();
                }
                else { MessageBox.Show("Select a material first."); }
            };

            btnShip.Click += (s, ev) => {
                if (grid.SelectedRows.Count > 0)
                {
                    int materialID = Convert.ToInt32(grid.SelectedRows[0].Cells["MaterialID"].Value);
                    if (!shippingOperator.ShipStocks(materialID, 10, 1))
                    {
                        MessageBox.Show("Insufficient stock or error.");
                    }
                    refreshGrid();
                }
                else { MessageBox.Show("Select a material first."); }
            };

            dynamicPanel.Controls.Add(btnLoad);
            dynamicPanel.Controls.Add(btnShip);
        }

        private void LoadCustomerManagementView()
        {
            HideDashboard();

            Label title = new Label() { Text = "Customer Management", Font = new Font("Arial", 16, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            dynamicPanel.Controls.Add(title);

            Button btnBack = new Button() { Text = "Back to Dashboard", Location = new Point(650, 20), Size = new Size(150, 30) };
            btnBack.Click += (s, ev) => ShowDashboard();
            dynamicPanel.Controls.Add(btnBack);

            DataGridView grid = new DataGridView() { Location = new Point(20, 70), Size = new Size(800, 300), AllowUserToAddRows = false, ReadOnly = true };
            dynamicPanel.Controls.Add(grid);

            Action refreshGrid = () => { grid.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM Customers"); };
            refreshGrid();

            TextBox txtName = new TextBox() { Location = new Point(20, 390), Width = 200, Text = "Customer Name" };
            TextBox txtEmail = new TextBox() { Location = new Point(240, 390), Width = 200, Text = "Email" };
            TextBox txtPhone = new TextBox() { Location = new Point(460, 390), Width = 200, Text = "Phone" };
            
            Button btnAdd = new Button() { Text = "Add Customer", Location = new Point(680, 385), Size = new Size(140, 30) };
            btnAdd.Click += (s, ev) => {
                string sql = "INSERT INTO Customers (CustomerName, Email, Phone) VALUES (@Name, @Email, @Phone)";
                SqlParameter[] p = { new SqlParameter("@Name", txtName.Text), new SqlParameter("@Email", txtEmail.Text), new SqlParameter("@Phone", txtPhone.Text) };
                DatabaseHelper.ExecuteNonQuery(sql, p);
                refreshGrid();
                txtName.Text = "Customer Name"; txtEmail.Text = "Email"; txtPhone.Text = "Phone";
            };

            dynamicPanel.Controls.Add(txtName);
            dynamicPanel.Controls.Add(txtEmail);
            dynamicPanel.Controls.Add(txtPhone);
            dynamicPanel.Controls.Add(btnAdd);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        bool expand = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (expand == false)
            {
                dropdownContainer.Height += 115;
                if (dropdownContainer.Height >= dropdownContainer.MinimumSize.Height)
                {
                    timer1.Stop();
                    expand = true;
                }
            }
            else
            {
                dropdownContainer.Height -= 115;
                if (dropdownContainer.Height <= dropdownContainer.MinimumSize.Height)
                {
                    timer1.Stop();
                    expand = false;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}