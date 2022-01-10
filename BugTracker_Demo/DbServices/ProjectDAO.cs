using BugTracker_Demo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker_Demo.DbServices
{
    public class ProjectDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MVCDemoDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        public void AddProjectToDatabase(ProjectModel project)
        {
            string sqlStatement = "INSERT INTO dbo.Project (title, description, username) VALUES (@title, @description, @username)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@title", System.Data.SqlDbType.NVarChar, 50).Value = project.Title;
                command.Parameters.Add("@description", System.Data.SqlDbType.NVarChar, 200).Value = project.Description;
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = "me";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())

                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}", reader["title"], reader["description"], reader["username"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public void DeleteProjectFromDatabase(ProjectModel project)
        {
            string sqlStatement = "DELETE FROM dbo.Project WHERE title = @title AND description = @description AND username = @username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@title", System.Data.SqlDbType.NVarChar, 50).Value = project.Title;
                command.Parameters.Add("@description", System.Data.SqlDbType.NVarChar, 200).Value = project.Description;
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = project.UserName;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())

                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}", reader["name"], reader["description"], reader["username"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public List<ProjectModel> GetProjectList()
        {
            var ProjectList = new List<ProjectModel>();
            string sqlStatement = "SELECT * FROM dbo.Project";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sqlStatement, conn);
            using (conn)
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var project = new ProjectModel();

                    project.Title = (string)rdr["Title"];
                    project.Description = (string)rdr["Description"];
                    project.UserName = (string)rdr["UserName"];

                    ProjectList.Add(project);
                }
            }

            return ProjectList;
        }

        public List<ProjectHybridModel> RetrieveProjectHybridList()
        {
            var hybridList = new List<ProjectHybridModel>();
            var projectList = GetProjectList();
            


            foreach (var project in projectList)
            {
                string sqlStatement = "SELECT * FROM dbo.Ticket WHERE projecttitle = @ProjectTitle";
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(sqlStatement, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                using (conn)
                {
                    conn.Open();
                    var ticketList = new List<TicketModel>();
                    cmd.Parameters.Add("@ProjectTitle", System.Data.SqlDbType.NVarChar, 50).Value = project.Title;

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

                        ticketList.Add(ticket);
                    }
                    var tempHybrid = new ProjectHybridModel();
                    tempHybrid.Project = project;
                    tempHybrid.TicketList = ticketList;
                    hybridList.Add(tempHybrid);
                }
            }
            

            return hybridList;
        }

        public List<TicketModel> GetTicketListFromProject(ProjectModel project)
        {
            var ticketList = new List<TicketModel>();
            string sqlStatement = "SELECT * FROM dbo.Ticket WHERE projecttitle = @ProjectTitle";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sqlStatement, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            using (conn)
            {
                cmd.Parameters.Add("@ProjectTitle", System.Data.SqlDbType.NVarChar, 50).Value = project.Title;
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

                    ticketList.Add(ticket);
                }            
            }

            return ticketList;
        }
    }
}
