using CRUDActor.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDActor
{
    class DatabaseHelper
    {
        private String connectionString { get; set; }
        private String sqlCommandString { get; set; }

        public DatabaseHelper()
        {
            connectionString = "Data Source=172.26.171.18; Network Library=DBMSSOCN;" +
                "Initial Catalog=ToDoDB; User ID=sa;Password=Ceid@5202";

        }

        public bool Create(TaskEntry entry)
        {
            String strSQL = "INSERT INTO ToDoTable VALUES('" + entry.taskDescr + "','" + entry.userID + "')";
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand myCommand = new SqlCommand(strSQL, cnn))
                {
                    myCommand.Parameters.AddWithValue("@userID", "sa");
                    cnn.Open();
                    //SqlDataReader reader = myCommand.ExecuteReader();
                    myCommand.ExecuteNonQuery();
                    //reader.Close();
                    cnn.Close();
                }
            }
            return true;
        }
        public List<TaskEntry> Read(string user)
        {
            List<TaskEntry> myEntries = new List<TaskEntry>();
            String strSQL = "SELECT * FROM ToDoTable WHERE UserID = '" + user + "'";
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand myCommand = new SqlCommand(strSQL, cnn))
                {
                    myCommand.Parameters.AddWithValue("@userID", "sa");
                    cnn.Open();
                    using (SqlDataReader reader = myCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            myEntries.Add(new TaskEntry(reader["TaskDescr"].ToString()
                                , reader["UserID"].ToString()));

                        }
                        reader.Close();
                    }
                    cnn.Close();
                }
            }
            return myEntries;
        }
        public bool Update(TaskEntry entry_old, TaskEntry entry_new)
        {
            String strSQL = "UPDATE ToDoTable " +
                "SET TaskDescr='" + entry_new.taskDescr + "' " +
                "WHERE TaskDescr='" + entry_old.taskDescr + "' AND UserID='" + entry_old.userID + "'";
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand myCommand = new SqlCommand(strSQL, cnn))
                {
                    myCommand.Parameters.AddWithValue("@userID", "sa");
                    cnn.Open();
                    //SqlDataReader reader = myCommand.ExecuteReader();
                    myCommand.ExecuteNonQuery();
                    //reader.Close();
                    cnn.Close();
                }
            }
            return true;
        }
        public bool Delete(TaskEntry entry)
        {
            String strSQL = "DELETE FROM ToDoTable WHERE TaskDescr='" + entry.taskDescr + "' AND UserID='" + entry.userID + "'";
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand myCommand = new SqlCommand(strSQL, cnn))
                {
                    myCommand.Parameters.AddWithValue("@userID", "sa");
                    cnn.Open();
                    //SqlDataReader reader = myCommand.ExecuteReader();
                    myCommand.ExecuteNonQuery();
                    //reader.Close();
                    cnn.Close();
                }
            }
            return true;
        }
    }
}