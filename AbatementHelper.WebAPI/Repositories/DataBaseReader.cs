using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace AbatementHelper.WebApi.Repositeories
{
    public class DataBaseReader
    {
        private static string ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(); //"Data Source=MyNameJeff;Initial Catalog=AbatementHelper.DataBase;Integrated Security=True";

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

        public static bool ReadRole(string role)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand sqlCommand = new SqlCommand();

            connection.Open();

            sqlCommand.Connection = connection;

            sqlCommand.CommandType = System.Data.CommandType.Text;

            sqlCommand.CommandText = "SELECT * FROM AspNetRoles WHERE Name = @role";

            sqlCommand.Parameters.AddWithValue("@role", role);

            SqlDataReader reader = sqlCommand.ExecuteReader();

            int count = reader.FieldCount;
            while (reader.Read())
            {
                if (reader["Name"].ToString() == role)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool UpdateDataBaseApproved(string email, bool approved)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand sqlCommand = new SqlCommand();

            

            try
            {
                SqlDataReader reader = sqlCommand.ExecuteReader();

                int count = reader.FieldCount;
                while (reader.Read())
                {
                    if (reader["Email"].ToString() == email)
                    {
                        return true;
                    }
                }

                connection.Open();

                sqlCommand.Connection = connection;

                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.Parameters.AddWithValue("@email", email);

                sqlCommand.Parameters.Add(new SqlParameter("@approved", System.Data.SqlDbType.Bit)).Value = approved;

                sqlCommand.CommandText = "UPDATE AspNetUsers SET Approved = @approved WHERE Email = @email";

                sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}