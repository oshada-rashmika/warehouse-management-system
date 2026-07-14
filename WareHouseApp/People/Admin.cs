using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace WareHouseApp.People
{
    class Admin : Person
    {
        

        public override bool Login(string username, string password)
        {
            string query = "SELECT PasswordHash, Role FROM Employees WHERE UserName = @User";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@User", username)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);
            if (result.Rows.Count > 0)
            {
                string dbHash = result.Rows[0]["PasswordHash"].ToString();
                string dbRole = result.Rows[0]["Role"].ToString();
                string inputHash = DatabaseHelper.HashPassword(password);

                if (dbHash == inputHash && dbRole == "Admin")
                {
                    return true;
                }
            }
            return false;
        }

        public override void ChangePassword(string newPassword)
        {
            throw new NotImplementedException();
        }

        public override void Logout(int userID)
        {
            throw new NotImplementedException();
        }
    }
}
