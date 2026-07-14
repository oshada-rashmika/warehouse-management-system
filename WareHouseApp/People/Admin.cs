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
            string query = "SELECT COUNT(1) FROM Person WHERE Username = @User AND Password = @Pass";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@User", username),
                new SqlParameter("@Pass", password)
            };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            if (result != null && Convert.ToInt32(result) > 0)
            {
                this.userName = username;
                return true;
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
