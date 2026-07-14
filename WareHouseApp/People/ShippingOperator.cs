using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace WareHouseApp.People
{
    public class ShippingOperator : Person
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
            this.Password = newPassword;
        }
        public override void Logout(int userID)
        {

        }

        public bool LoadStocks(int materialID, int qty, int employeeID)
        {
            if (qty <= 0) return false;

            string updateSql = "UPDATE Materials SET MaterialCount = MaterialCount + @Qty WHERE MaterialID = @ID";
            SqlParameter[] updateParams = new SqlParameter[]
            {
                new SqlParameter("@Qty", qty),
                new SqlParameter("@ID", materialID)
            };
            int rows = DatabaseHelper.ExecuteNonQuery(updateSql, updateParams);
            
            if (rows > 0)
            {
                string logSql = "INSERT INTO StockTransactions (MaterialID, EmployeeID, TransactionType, Quantity) VALUES (@ID, @EmpID, 'Load', @Qty)";
                SqlParameter[] logParams = new SqlParameter[]
                {
                    new SqlParameter("@ID", materialID),
                    new SqlParameter("@EmpID", employeeID),
                    new SqlParameter("@Qty", qty)
                };
                DatabaseHelper.ExecuteNonQuery(logSql, logParams);
                return true;
            }
            return false;
        }

        public bool ShipStocks(int materialID, int qty, int employeeID)
        {
            if (qty <= 0) return false;
            
            string checkSql = "SELECT MaterialCount FROM Materials WHERE MaterialID = @ID";
            SqlParameter[] checkParams = new SqlParameter[] { new SqlParameter("@ID", materialID) };
            object countResult = DatabaseHelper.ExecuteScalar(checkSql, checkParams);
            if (countResult == null || Convert.ToInt32(countResult) < qty) return false;

            string updateSql = "UPDATE Materials SET MaterialCount = MaterialCount - @Qty WHERE MaterialID = @ID";
            SqlParameter[] updateParams = new SqlParameter[]
            {
                new SqlParameter("@Qty", qty),
                new SqlParameter("@ID", materialID)
            };
            int rows = DatabaseHelper.ExecuteNonQuery(updateSql, updateParams);

            if (rows > 0)
            {
                string logSql = "INSERT INTO StockTransactions (MaterialID, EmployeeID, TransactionType, Quantity) VALUES (@ID, @EmpID, 'Ship', @Qty)";
                SqlParameter[] logParams = new SqlParameter[]
                {
                    new SqlParameter("@ID", materialID),
                    new SqlParameter("@EmpID", employeeID),
                    new SqlParameter("@Qty", qty)
                };
                DatabaseHelper.ExecuteNonQuery(logSql, logParams);
                return true;
            }
            return false;
        }
    }
}
