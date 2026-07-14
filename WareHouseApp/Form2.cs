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
            if (!(SessionManager.CurrentUser is Admin))
            {
                MessageBox.Show(
                    "Access Denied.\n\nYou do not have permission to view the Admin Dashboard.\n" +
                    "This area is restricted to users with the Administrator role.",
                    "Unauthorized Access",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            LoadAdminDashboardView();
        }

        private void LoadAdminDashboardView()
        {
            HideDashboard();

            Label title = new Label
            {
                Text     = "Admin Dashboard — Employees",
                Font     = new System.Drawing.Font("Arial", 16, FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };
            dynamicPanel.Controls.Add(title);

            Button btnBack = new Button
            {
                Text     = "Back to Dashboard",
                Location = new System.Drawing.Point(650, 20),
                Size     = new System.Drawing.Size(160, 30)
            };
            btnBack.Click += (s, ev) => ShowDashboard();
            dynamicPanel.Controls.Add(btnBack);

            DataGridView grid = new DataGridView
            {
                Location          = new System.Drawing.Point(20, 70),
                Size              = new System.Drawing.Size(800, 460),
                AllowUserToAddRows = false,
                ReadOnly          = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode     = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor   = System.Drawing.Color.White
            };
            dynamicPanel.Controls.Add(grid);

            try
            {
                string query = @"
                    SELECT
                        UserName     AS [Username],
                        FullName     AS [Full Name],
                        Role         AS [Role]
                    FROM Employees
                    ORDER BY Role, UserName";

                DataTable employeeData = DatabaseHelper.ExecuteQuery(query);

                if (employeeData == null || employeeData.Rows.Count == 0)
                {
                    Label lblEmpty = new Label
                    {
                        Text     = "No employee records found in the database.",
                        Location = new System.Drawing.Point(20, 120),
                        AutoSize = true,
                        Font     = new System.Drawing.Font("Arial", 11)
                    };
                    dynamicPanel.Controls.Add(lblEmpty);
                    return;
                }

                grid.DataSource = employeeData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load employee data from the database.\n\nDetails: {ex.Message}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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

            Label title = new Label
            {
                Text     = "Warehouse Operations",
                Font     = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            dynamicPanel.Controls.Add(title);

            Button btnBack = new Button { Text = "Back to Dashboard", Location = new Point(650, 20), Size = new Size(150, 30) };
            btnBack.Click += (s, ev) => ShowDashboard();
            dynamicPanel.Controls.Add(btnBack);

            // Materials grid
            DataGridView grid = new DataGridView
            {
                Location            = new Point(20, 70),
                Size                = new Size(800, 300),
                AllowUserToAddRows  = false,
                ReadOnly            = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode       = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor     = System.Drawing.Color.White
            };
            dynamicPanel.Controls.Add(grid);

            // Status label – shows loading spinner text or last action result
            Label lblStatus = new Label
            {
                Location  = new Point(20, 380),
                Size      = new Size(800, 22),
                ForeColor = System.Drawing.Color.DimGray,
                Font      = new Font("Arial", 9),
                Text      = "Ready."
            };
            dynamicPanel.Controls.Add(lblStatus);

            // Quantity input
            Label lblQty = new Label { Text = "Qty:", Location = new Point(240, 418), AutoSize = true };
            TextBox txtQty = new TextBox { Location = new Point(275, 415), Width = 80, Text = "10" };
            dynamicPanel.Controls.Add(lblQty);
            dynamicPanel.Controls.Add(txtQty);

            Button btnLoad = new Button { Text = "Load Stock",  Location = new Point(20,  410), Size = new Size(120, 35) };
            Button btnShip = new Button { Text = "Ship Stock",  Location = new Point(150, 410), Size = new Size(120, 35) };
            Button btnLog  = new Button { Text = "View Log",    Location = new Point(570, 410), Size = new Size(100, 35) };
            dynamicPanel.Controls.Add(btnLoad);
            dynamicPanel.Controls.Add(btnShip);
            dynamicPanel.Controls.Add(btnLog);

            // Transaction log grid (hidden until btnLog clicked)
            DataGridView logGrid = new DataGridView
            {
                Location            = new Point(20, 460),
                Size                = new Size(800, 120),
                AllowUserToAddRows  = false,
                ReadOnly            = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor     = System.Drawing.Color.White,
                Visible             = false
            };
            dynamicPanel.Controls.Add(logGrid);

            // ── Non-blocking grid refresh via BackgroundWorker ─────────────
            // The DB query runs on a pool thread; only the DataSource assignment
            // returns to the UI thread via RunWorkerCompleted, preventing freezing.
            void RefreshMaterialsAsync()
            {
                lblStatus.Text = "Loading data…";
                btnLoad.Enabled = false;
                btnShip.Enabled = false;

                var bw = new System.ComponentModel.BackgroundWorker();

                bw.DoWork += (s, args) =>
                {
                    args.Result = DatabaseHelper.ExecuteQuery("SELECT MaterialID, MaterialName, MaterialCount FROM Materials ORDER BY MaterialName");
                };

                bw.RunWorkerCompleted += (s, args) =>
                {
                    btnLoad.Enabled = true;
                    btnShip.Enabled = true;

                    if (args.Error != null)
                    {
                        lblStatus.Text = "Error loading data.";
                        MessageBox.Show($"Failed to load Materials:\n{args.Error.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    grid.DataSource = args.Result as DataTable;
                    lblStatus.Text  = $"Materials refreshed at {DateTime.Now:HH:mm:ss}";
                };

                bw.RunWorkerAsync();
            }

            void RefreshLogAsync()
            {
                var bw = new System.ComponentModel.BackgroundWorker();
                bw.DoWork += (s, args) =>
                {
                    args.Result = DatabaseHelper.ExecuteQuery(
                        @"SELECT TOP 50
                            ST.TransactionID,
                            M.MaterialName,
                            ST.TransactionType,
                            ST.Quantity,
                            ST.TransactionDate
                          FROM StockTransactions ST
                          JOIN Materials M ON ST.MaterialID = M.MaterialID
                          ORDER BY ST.TransactionDate DESC");
                };
                bw.RunWorkerCompleted += (s, args) =>
                {
                    if (args.Error != null) return;
                    logGrid.DataSource = args.Result as DataTable;
                };
                bw.RunWorkerAsync();
            }

            // Initial load
            RefreshMaterialsAsync();

            // ── Stock action helper ────────────────────────────────────────
            void ExecuteStockAction(bool isLoad)
            {
                if (grid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a material row first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
                {
                    MessageBox.Show("Please enter a valid positive integer for the quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int materialID = Convert.ToInt32(grid.SelectedRows[0].Cells["MaterialID"].Value);
                btnLoad.Enabled = false;
                btnShip.Enabled = false;
                lblStatus.Text  = isLoad ? "Loading stock…" : "Shipping stock…";

                // DB write also runs off the UI thread
                var bw = new System.ComponentModel.BackgroundWorker();
                bw.DoWork += (s, args) =>
                {
                    bool success = isLoad
                        ? shippingOperator.LoadStocks(materialID, qty, 1)
                        : shippingOperator.ShipStocks(materialID, qty, 1);
                    args.Result = success;
                };
                bw.RunWorkerCompleted += (s, args) =>
                {
                    btnLoad.Enabled = true;
                    btnShip.Enabled = true;

                    if (args.Error != null)
                    {
                        lblStatus.Text = "Error during operation.";
                        MessageBox.Show($"An error occurred:\n{args.Error.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    bool ok = (bool)args.Result;
                    if (ok)
                    {
                        lblStatus.Text = isLoad
                            ? $"Loaded {qty} unit(s) at {DateTime.Now:HH:mm:ss}"
                            : $"Shipped {qty} unit(s) at {DateTime.Now:HH:mm:ss}";
                        RefreshMaterialsAsync();
                        if (logGrid.Visible) RefreshLogAsync();
                    }
                    else
                    {
                        string msg = isLoad
                            ? "Load failed. Verify the Material ID exists."
                            : "Ship failed. Insufficient stock or invalid Material ID.";
                        MessageBox.Show(msg, "Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = "Operation failed.";
                    }
                };
                bw.RunWorkerAsync();
            }

            btnLoad.Click += (s, ev) => ExecuteStockAction(isLoad: true);
            btnShip.Click += (s, ev) => ExecuteStockAction(isLoad: false);
            btnLog.Click  += (s, ev) =>
            {
                logGrid.Visible = !logGrid.Visible;
                btnLog.Text     = logGrid.Visible ? "Hide Log" : "View Log";
                if (logGrid.Visible) RefreshLogAsync();
            };
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
                string name = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string phone = txtPhone.Text.Trim();

                if (string.IsNullOrWhiteSpace(name) || name == "Customer Name" ||
                    string.IsNullOrWhiteSpace(email) || email == "Email" ||
                    string.IsNullOrWhiteSpace(phone) || phone == "Phone")
                {
                    MessageBox.Show("All fields are required. Please fill out the customer details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    string sql = "INSERT INTO Customers (CustomerName, Email, Phone) VALUES (@Name, @Email, @Phone)";
                    SqlParameter[] p = { new SqlParameter("@Name", name), new SqlParameter("@Email", email), new SqlParameter("@Phone", phone) };
                    DatabaseHelper.ExecuteNonQuery(sql, p);
                    
                    MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refreshGrid();
                    txtName.Text = "Customer Name"; txtEmail.Text = "Email"; txtPhone.Text = "Phone";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while adding the customer:\n{ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            SessionManager.ClearSession();
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}