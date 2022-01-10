using BugTracker_Demo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BugTracker_Demo.DbServices
{
    public class FinishedDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MVCDemoDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void AddFinishedToDatabase(FinishedTicketModel finishedTicket)
        {
            string sqlStatement = "INSERT INTO dbo.FinishedTicket (name, description, type, username, projecttitle, startdate, finishdate) VALUES (@name, @description, @type, @username, @projecttitle, @startdate, @finishdate)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 50).Value = finishedTicket.Name;
                command.Parameters.Add("@description", System.Data.SqlDbType.NVarChar, 200).Value = finishedTicket.Description;
                command.Parameters.Add("@type", System.Data.SqlDbType.NVarChar, 50).Value = finishedTicket.Type;
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = finishedTicket.UserName;
                command.Parameters.Add("@projecttitle", System.Data.SqlDbType.NVarChar, 50).Value = finishedTicket.ProjectTitle;
                command.Parameters.Add("@startdate", System.Data.SqlDbType.SmallDateTime).Value = finishedTicket.StartDate;
                command.Parameters.Add("@finishdate", System.Data.SqlDbType.SmallDateTime).Value = finishedTicket.FinishDate;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())

                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}", reader["name"], reader["description"], reader["type"], reader["username"], reader["projecttitle"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }



    }
}
