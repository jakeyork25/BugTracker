using BugTracker_Demo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BugTracker_Demo.DbServices
{
    public class TicketDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MVCDemoDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void AddTicketToDatabase(TicketModel ticket)
        {
            string sqlStatement = "INSERT INTO dbo.Ticket (name, description, priority, type, progress, username, projecttitle, date) VALUES (@name, @description, @priority, @type, @progress, @username, @projecttitle, @date)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Name;
                command.Parameters.Add("@description", System.Data.SqlDbType.NVarChar, 200).Value = ticket.Description;
                command.Parameters.Add("@priority", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Priority;
                command.Parameters.Add("@type", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Type;
                command.Parameters.Add("@progress", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Progress;
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = ticket.UserName;
                command.Parameters.Add("@projecttitle", System.Data.SqlDbType.NVarChar, 50).Value = ticket.ProjectTitle;
                command.Parameters.Add("@date", System.Data.SqlDbType.SmallDateTime).Value = ticket.Date;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())

                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", reader["name"], reader["description"], reader["priority"], reader["type"], reader["progress"], reader["username"], reader["projecttitle"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public void DeleteTicketFromDatabase(TicketModel ticket)
        {
            string sqlStatement = "DELETE FROM dbo.Ticket WHERE name = @name AND description = @description AND priority = @priority AND type = @type AND progress = @progress AND username = @username AND projecttitle = @projecttitle";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Name;
                command.Parameters.Add("@description", System.Data.SqlDbType.NVarChar, 200).Value = ticket.Description;
                command.Parameters.Add("@priority", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Priority;
                command.Parameters.Add("@type", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Type;
                command.Parameters.Add("@progress", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Progress;
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = ticket.UserName;
                command.Parameters.Add("@projecttitle", System.Data.SqlDbType.NVarChar, 50).Value = ticket.ProjectTitle;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())

                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", reader["name"], reader["description"], reader["priority"], reader["type"], reader["progress"], reader["username"], reader["projecttitle"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public void DeleteAllTicketsForProjectFromDatabase(string projectTitle)
        {
            string sqlStatement = "DELETE FROM dbo.Note WHERE projecttitle = @projecttitle";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@projecttitle", System.Data.SqlDbType.NVarChar, 50).Value = projectTitle;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())

                    {
                        Console.WriteLine(String.Format("{0}", reader["projecttitle"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public List<TicketModel> GetTicketAttributes(string organization)
        {
            var tempTicketList = new List<TicketModel>();

            if (organization.Equals("Date"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);
            }
            else if (organization.Equals("Most Urgent"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Urgent'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Important'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Medium'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Unimportant'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);
            }
            else if (organization.Equals("Least Urgent"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Unimportant'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Medium'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Important'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Urgent'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);  
            }
            else if (organization.Equals("Most Progress"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Near Complete'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Over Half'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Under Half'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'New'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);
            }
            else if (organization.Equals("Least Progress"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'New'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Under Half'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Over Half'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Near Complete'";
                tempTicketList = GetTicketStats(tempTicketList, sqlStatement);
            }
            


            return tempTicketList;
        }

        public List<TicketModel> GetTicketStats(List<TicketModel> tempModel, string statement)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(statement, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            using (conn)
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ticket = new TicketModel();

                    ticket.Name = (string)rdr["Name"];
                    ticket.Description = (string)rdr["Description"];
                    ticket.Priority = (string)rdr["Priority"];
                    ticket.Type = (string)rdr["Type"];
                    ticket.Progress = (string)rdr["Progress"];
                    ticket.UserName = (string)rdr["UserName"];
                    ticket.ProjectTitle = (string)rdr["ProjectTitle"];
                    ticket.Date = (DateTime)rdr["Date"];

                    tempModel.Add(ticket);
                }

            }

            return tempModel;
        }

        public List<TicketModel> GetTicketsThroughPriority(TicketColumnModel ticketColumnModel)
        {
            String search = ticketColumnModel.search;

            var model = new List<TicketModel>();

            string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = '" + search + "'";
            model = GetTicketStats(model, sqlStatement);
            return model;
        }

        public List<TicketModel> GetTicketsThroughType(TicketColumnModel ticketColumnModel)
        {
            string search = ticketColumnModel.search;
            var model = new List<TicketModel>();

            string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Type = '" + search + "'";
            model = GetTicketStats(model, sqlStatement);
            return model;
        }

        public List<TicketModel> GetTicketsThroughProgress(TicketColumnModel ticketColumnModel)
        {
            string search = ticketColumnModel.search;
            var model = new List<TicketModel>();

            string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = '" + search + "'";
            model = GetTicketStats(model, sqlStatement);
            return model;
        }

        public List<TicketModel> GetTicketsThroughUsername(TicketColumnModel ticketColumnModel)
        {
            string search = ticketColumnModel.search;
            var model = new List<TicketModel>();

            string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Username LIKE '%" + search + "%'";
            model = GetTicketStats(model, sqlStatement);
            return model;
        }

        public List<TicketModel> SearchThroughTickets(SearchModel searchModel, string organization)
        {
            String search = searchModel.search;
            String type = searchModel.type;

            var model = new List<TicketModel>();

            if (organization.Equals("Most Recent"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);
            }
            else if (organization.Equals("Least Recent"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket ORDER BY Id DESC";
                model = GetTicketStats(model, sqlStatement);
            }
            else if (organization.Equals("Most Urgent"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Urgent' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Important' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Medium' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Unimportant' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);
            } 
            else if (organization.Equals("Least Urgent"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Unimportant' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Medium' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Important' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Priority = 'Urgent' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);
            }
            else if (organization.Equals("Most Progress"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Near Complete' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Over Half' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Under Half' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'New' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);
            }
            else if (organization.Equals("Least Progress"))
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'New' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Under Half' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Over Half' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);

                sqlStatement = "SELECT * FROM dbo.Ticket WHERE Progress = 'Near Complete' AND " + type + " LIKE '%" + search + "%'";
                model = GetTicketStats(model, sqlStatement);
            }

            return model;
        }


    }

}


    
    

