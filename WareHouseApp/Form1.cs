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
using WareHouseApp.Resources.AppStrings;

namespace WareHouseApp
{
    public partial class Form1 : Form
    {
        Admin admin = new Admin();
        LoginPage loginPage = new LoginPage();
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butLogin_Click(object sender, EventArgs e)
        {
            string UserNameTxt = txtName.Text;
            string PassTxt = txtPassword.Text;
            try
            {
                admin.Login(UserNameTxt, PassTxt);
                Console.WriteLine("Logged IN");

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                MessageBox.Show(loginPage.LoginErrorTitleEn, loginPage.LoginErrorMessageEn, MessageBoxButtons.RetryCancel);
            }
            finally
            {
                txtPassword.Text="";
            }

            
        }
    }
}
