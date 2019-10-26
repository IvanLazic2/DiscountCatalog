using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using AbatementHelper.WebAPI.Models;

namespace AbatementHelper.WebApi.Repositeories
{
    public class DataBaseReader
    {
        private static string ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(); //"Data Source=MyNameJeff;Initial Catalog=AbatementHelper.DataBase;Integrated Security=True";

        public static DataBaseResult ReadEmail(string email)
        {
            var querry = "SELECT * FROM AspNetUsers";

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand command = new SqlCommand(querry, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                DataBaseResult result = new DataBaseResult();

                while (reader.Read())
                {
                    if (reader["Email"].ToString() == email)
                    {
                        result.Value = email;
                        result.Message = "Querry successful";
                        result.Success = true;

                        return result;
                    }
                    else
                    {
                        result.Message = "Querry unsuccessful";
                        result.Success = false;
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public static DataBaseResult ReadUsername(string email)
        {
            var querry = "SELECT * FROM AspNetUsers";

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand command = new SqlCommand(querry, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                DataBaseResult result = new DataBaseResult();

                while (reader.Read())
                {
                    if (reader["Email"].ToString() == email)
                    {
                        result.Value = reader["UserName"].ToString();
                        result.Message = "Querry successful";
                        result.Success = true;

                        return result;
                    }
                    else
                    {
                        result.Message = "Querry unsuccessful";
                        result.Success = false;
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }

        }

        public static DataBaseResultUser ReadUser(string email)
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

            DataBaseUser user = new DataBaseUser();

            DataBaseResultUser result = new DataBaseResultUser();

            try
            {
                int count = reader.FieldCount;
                while (reader.Read())
                {
                    if (reader["Email"].ToString() == email)
                    {
                        user.Id = reader["Id"].ToString();
                        user.FirstName = reader["FirstName"].ToString();
                        user.LastName = reader["LastName"].ToString();
                        user.PhoneNumber = reader["PhoneNumber"].ToString();
                        user.Country = reader["Country"].ToString();
                        user.City = reader["City"].ToString();
                        user.PostalCode = reader["PostalCode"].ToString();
                        user.Street = reader["Street"].ToString();
                        user.Role = reader["Role"].ToString();
                        user.Approved = (bool)reader["Approved"];
                        user.Email = reader["Email"].ToString();
                        user.EmailConfirmed = (bool)reader["EmailConfirmed"];
                        user.PhoneNumberConfirmed = (bool)reader["PhoneNumberConfirmed"];
                        user.TwoFactorEnabled = (bool)reader["TwoFactorEnabled"];
                        user.AccessFailedCount = (int)reader["AccessFailedCount"];
                        user.UserName = reader["UserName"].ToString();

                        result.Value = user;
                        result.Message = "Querry successful";
                        result.Success = true;
                    }

                    else
                    {
                        result.Message = "Querry unsuccessful";
                        result.Success = false;

                        return result;
                    }
                }

                return result;

            }
            catch
            {
                throw;
            }


        }

        public static DataBaseResult ReadRole(string role)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand sqlCommand = new SqlCommand();

            try
            {
                connection.Open();

                sqlCommand.Connection = connection;

                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.CommandText = "SELECT * FROM AspNetRoles WHERE Name = @role";

                sqlCommand.Parameters.AddWithValue("@role", role);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                DataBaseResult result = new DataBaseResult();

                int count = reader.FieldCount;
                while (reader.Read())
                {
                    if (reader["Name"].ToString() == role)
                    {
                        result.Value = reader["Name"].ToString();
                        result.Message = "Querry successful";
                        result.Success = true;

                        return result;
                    }

                    else
                    {
                        result.Message = "Querry unsuccessful";
                        result.Success = false;

                        return result;
                    }
                }

                result.Message = "Querry unsuccessful";
                result.Success = false;

                return result;
            }
            catch
            {
                throw;
            }

        }

        public static DataBaseResultUser UpdateDataBaseApproved(string email, bool approved)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand sqlCommandUpdate = new SqlCommand();

            SqlCommand sqlCommandSelect = new SqlCommand();

            DataBaseResultUser result = new DataBaseResultUser();

            connection.Open();

            sqlCommandSelect.Connection = connection;

            sqlCommandSelect.CommandType = System.Data.CommandType.Text;

            sqlCommandSelect.CommandText = "SELECT * FROM AspNetUsers";

            sqlCommandSelect.ExecuteNonQuery();

            SqlDataReader reader = sqlCommandSelect.ExecuteReader();
            try
            {
                int count = reader.FieldCount;
                while (reader.Read())
                {
                    if (reader["Email"].ToString() == email)
                    {
                        if ((bool)reader["Approved"] == approved)
                        {
                            result.Message = $"User approved status is already: {(bool)reader["Approved"]}";
                            result.Success = false;

                            return result;
                        }

                        reader.Close();

                        sqlCommandUpdate.Connection = connection;

                        sqlCommandUpdate.CommandType = System.Data.CommandType.Text;

                        sqlCommandUpdate.Parameters.AddWithValue("@email", email);

                        sqlCommandUpdate.Parameters.Add(new SqlParameter("@approved", System.Data.SqlDbType.Bit)).Value = approved;

                        sqlCommandUpdate.CommandText = "UPDATE AspNetUsers SET Approved = @approved WHERE Email = @email";

                        sqlCommandUpdate.ExecuteNonQuery();

                        sqlCommandUpdate.Parameters.Clear();

                        result.Value = ReadUser(email).Value;
                        result.Message = "Querry successful";
                        result.Success = true;

                        return result;
                    }
                    else
                    {
                        result.Message = "Email does not exist";
                        result.Success = false;

                        return result;
                    }
                }

                result.Message = "Querry unsuccessful";
                result.Success = false;

                return result;

            }
            catch
            {
                throw;
            }

        }

        public static DataBaseResultListOfUsers ReadAllUsers()
        {
            List<DataBaseUser> users = new List<DataBaseUser>();

            DataBaseResultListOfUsers result = new DataBaseResultListOfUsers();

            var querry = "SELECT * FROM AspNetUsers";

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand command = new SqlCommand(querry, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                int count = reader.FieldCount;
                while (reader.Read())
                {
                    string email = reader["Email"].ToString();
                    users.Add(ReadUser(email).Value);
                }

                if (users != null)
                {
                    result.Value = users;
                    result.Message = "Querry successful";
                    result.Success = true;

                    return result;
                }

                result.Message = "Querry unsuccessful";
                result.Success = false;

                return result;
            }
            catch
            {
                throw;
            }
        }

    }
}