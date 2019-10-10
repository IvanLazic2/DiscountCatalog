using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebApi.Repositeories
{
    public class DataBaseReader
    {
        private static string ConnectionString = "Data Source=MyNameJeff;Initial Catalog=AbatementHelper.DataBase;Integrated Security=True";

        public static bool ReadEmail(string email)
        {
            var querry = "SELECT * FROM AspNetUsers";

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand command = new SqlCommand(querry, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader["Email"].ToString() == email)
                {
                    return true;
                }
            }


            return false;
        }

        public static string ReadUsername(string email)
        {
            var querry = "SELECT * FROM AspNetUsers";

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand command = new SqlCommand(querry, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader["Email"].ToString() == email)
                {
                    return reader["UserName"].ToString();
                }
            }

            return null;

        }

        public static string ReadUser(string email)
        {
            List<string> userData = new List<string>();

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand sqlCommand = new SqlCommand();

            connection.Open();

            sqlCommand.Connection = connection;

            sqlCommand.CommandType = System.Data.CommandType.Text;

            sqlCommand.CommandText = "SELECT * FROM AspNetUsers WHERE Email = @email";

            sqlCommand.Parameters.AddWithValue("@email", email);

            SqlDataReader reader = sqlCommand.ExecuteReader();

            int count = reader.FieldCount;
            while (reader.Read())
            {
                for (int i = 0; i < count; i++)
                {
                    userData.Add(reader.GetValue(i).ToString());
                }
            }

            return "";
        }

    }
}