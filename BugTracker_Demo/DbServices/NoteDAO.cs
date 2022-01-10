using BugTracker_Demo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker_Demo.DbServices
{
    public class NoteDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MVCDemoDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<NoteModel> RetreiveNotesForTicket(string ticketName)
        {
            string sqlStatement = "SELECT * FROM dbo.Note WHERE Ticket = @ticketName";
            List<NoteModel> noteList = new List<NoteModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@ticketName", System.Data.SqlDbType.NVarChar, 50).Value = ticketName;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        var note = new NoteModel();

                        note.Title = (string)reader["Title"];
                        note.Description = (string)reader["Description"];
                        note.Ticket = (string)reader["Ticket"];
                        note.UserName = (string)reader["UserName"];

                        noteList.Add(note);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return noteList;
        }

        public void AddNoteToDatabase(NoteModel note)
        {
            string sqlStatement = "INSERT INTO dbo.Note (title, description, ticket, username) VALUES (@title, @description, @ticket, @username)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@title", System.Data.SqlDbType.NVarChar, 50).Value = note.Title;
                command.Parameters.Add("@description", System.Data.SqlDbType.NVarChar, 200).Value = note.Description;
                command.Parameters.Add("@ticket", System.Data.SqlDbType.NVarChar, 50).Value = note.Ticket;
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = note.UserName;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())

                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}, {3}", reader["title"], reader["description"], reader["ticket"], reader["username"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public void DeleteNoteFromDatabase(NoteModel note)
        {
            string sqlStatement = "DELETE FROM dbo.Note WHERE title = @title AND description = @description AND ticket = @ticket AND username = @username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@title", System.Data.SqlDbType.NVarChar, 50).Value = note.Title;
                command.Parameters.Add("@description", System.Data.SqlDbType.NVarChar, 200).Value = note.Description;
                command.Parameters.Add("@ticket", System.Data.SqlDbType.NVarChar, 50).Value = note.Ticket;
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = note.UserName;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())

                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}, {3}", reader["title"], reader["description"], reader["ticket"], reader["username"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public void DeleteAllNotesForTicketFromDatabase(string ticketName)
        {
            string sqlStatement = "DELETE FROM dbo.Note WHERE ticket = @ticket";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@ticket", System.Data.SqlDbType.NVarChar, 50).Value = ticketName;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())

                    {
                        Console.WriteLine(String.Format("{0}", reader["ticket"]));
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
