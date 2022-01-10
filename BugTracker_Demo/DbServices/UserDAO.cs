using BugTracker_Demo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker_Demo.DbServices
{
    public class UserDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MVCDemoDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void AddUserToDatabase(RegisterModel user)
        {
            string sqlStatement = "INSERT INTO dbo.Users (userid, fullname, username, emailaddress, password) VALUES (@userid, @fullname, @username, @emailaddress, @password)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@userid", System.Data.SqlDbType.NVarChar, 50).Value = user.UserId;
                command.Parameters.Add("@fullname", System.Data.SqlDbType.NVarChar, 50).Value = user.FullName;
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = user.DesiredUserName;
                command.Parameters.Add("@emailaddress", System.Data.SqlDbType.NVarChar, 50).Value = user.EmailAddress;
                command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 50).Value = user.DesiredPassword;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}", reader["userid"], reader["fullname"], reader["desiredusername"], reader["emailaddress"], reader["desiredpassword"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public bool UserValid(RegisterModel user)
        {
            bool isValid = false;
            string sqlStatement = "SELECT * FROM dbo.Users WHERE username = @username AND password = @password";
            SqlConnection conn = new SqlConnection(connectionString);           
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStatement, conn);
                cmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = user.DesiredUserName;
                cmd.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 50).Value = user.DesiredPassword;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if(rdr.HasRows) { isValid = true; }
                }

            }
            return isValid;
        }

        public UserModel GetCurrentUser()
        {
            var model = new UserModel();
            string sqlStatement = "SELECT username FROM dbo.Users WHERE fullname = @fullname";
            SqlConnection conn = new SqlConnection(connectionString);
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStatement, conn);
                cmd.Parameters.Add("@fullname", System.Data.SqlDbType.NVarChar, 50).Value = "Ame";
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    model.UserName = (string)rdr["UserName"];
                }
            }
            return model;
        }
    }
}
