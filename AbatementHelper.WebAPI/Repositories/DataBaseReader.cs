//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Configuration;
//using AbatementHelper.CommonModels.Models;
//using AbatementHelper.CommonModels.WebApiModels;
//using AbatementHelper.WebAPI.Models;
//using AutoMapper;

//namespace AbatementHelper.WebApi.Repositeories
//{
//    public class DataBaseReader
//    {
//        private static string ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(); //"Data Source=MyNameJeff;Initial Catalog=AbatementHelper.DataBase;Integrated Security=True";

//        //public static DataBaseResult ReadEmail(string email)
//        //{
//        //    var querry = "SELECT * FROM AspNetUsers";

//        //    SqlConnection connection = new SqlConnection(ConnectionString);

//        //    SqlCommand command = new SqlCommand(querry, connection);

//        //    try
//        //    {
//        //        connection.Open();

//        //        SqlDataReader reader = command.ExecuteReader();

//        //        DataBaseResult result = new DataBaseResult();

//        //        while (reader.Read())
//        //        {
//        //            if (reader["Email"].ToString() == email)
//        //            {
//        //                result.Value = email;
//        //                result.Message = "Querry successful";
//        //                result.Success = true;

//        //                return result;
//        //            }
//        //            else
//        //            {
//        //                result.Message = "Querry unsuccessful";
//        //                result.Success = false;
//        //            }
//        //        }

//        //        return result;
//        //    }
//        //    catch
//        //    {
//        //        throw;
//        //    }
//        //    finally
//        //    {
//        //        connection.Close();
//        //    }
//        //}

//        //public static DataBaseResult ReadUsername(string email)
//        //{
//        //    var querry = "SELECT * FROM AspNetUsers";

//        //    SqlConnection connection = new SqlConnection(ConnectionString);

//        //    SqlCommand command = new SqlCommand(querry, connection);

//        //    try
//        //    {
//        //        connection.Open();

//        //        SqlDataReader reader = command.ExecuteReader();

//        //        DataBaseResult result = new DataBaseResult();

//        //        while (reader.Read())
//        //        {
//        //            if (reader["Email"].ToString() == email)
//        //            {
//        //                result.Value = reader["UserName"].ToString();
//        //                result.Message = "Querry successful";
//        //                result.Success = true;

//        //                return result;
//        //            }
//        //            else
//        //            {
//        //                result.Message = "Querry unsuccessful";
//        //                result.Success = false;
//        //            }
//        //        }

//        //        return result;
//        //    }
//        //    catch
//        //    {
//        //        throw;
//        //    }
//        //    finally
//        //    {
//        //        connection.Close();
//        //    }

//        //}

//        //public static DataBaseResultUser ReadUser(string email)
//        //{
//        //    List<string> userData = new List<string>();

//        //    SqlConnection connection = new SqlConnection(ConnectionString);

//        //    SqlCommand sqlCommand = new SqlCommand();

//        //    connection.Open();

//        //    sqlCommand.Connection = connection;

//        //    sqlCommand.CommandType = System.Data.CommandType.Text;

//        //    sqlCommand.CommandText = "SELECT * FROM AspNetUsers WHERE Email = @email";

//        //    sqlCommand.Parameters.AddWithValue("@email", email);

//        //    SqlDataReader reader = sqlCommand.ExecuteReader();

//        //    DataBaseUser user = new DataBaseUser();

//        //    DataBaseResultUser result = new DataBaseResultUser();

//        //    try
//        //    {
//        //        int count = reader.FieldCount;
//        //        while (reader.Read())
//        //        {
//        //            if (reader["Email"].ToString() == email)
//        //            {
//        //                user.Id = reader["Id"].ToString();
//        //                user.FirstName = reader["FirstName"].ToString();
//        //                user.LastName = reader["LastName"].ToString();
//        //                user.PhoneNumber = reader["PhoneNumber"].ToString();
//        //                //user.Country = reader["Country"].ToString();
//        //                //user.City = reader["City"].ToString();
//        //                //user.PostalCode = reader["PostalCode"].ToString();
//        //                //user.Street = reader["Street"].ToString();
//        //                user.Role = reader["Role"].ToString();
//        //                user.Approved = (bool)reader["Approved"];
//        //                user.Email = reader["Email"].ToString();
//        //                user.EmailConfirmed = (bool)reader["EmailConfirmed"];
//        //                user.PhoneNumberConfirmed = (bool)reader["PhoneNumberConfirmed"];
//        //                user.TwoFactorEnabled = (bool)reader["TwoFactorEnabled"];
//        //                user.AccessFailedCount = (int)reader["AccessFailedCount"];
//        //                user.UserName = reader["UserName"].ToString();
//        //                user.Deleted = (bool)reader["Deleted"];

//        //                result.Value = user;
//        //                result.Message = "Querry successful";
//        //                result.Success = true;
//        //            }

//        //            else
//        //            {
//        //                result.Message = "Querry unsuccessful";
//        //                result.Success = false;

//        //                return result;
//        //            }
//        //        }

//        //        return result;

//        //    }
//        //    catch
//        //    {
//        //        throw;
//        //    }
//        //    finally
//        //    {
//        //        connection.Close();
//        //    }

//        //}

//        public static DataBaseResultUser ReadUserById(string id)
//        {
//            List<string> userData = new List<string>();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            connection.Open();

//            sqlCommand.Connection = connection;

//            sqlCommand.CommandType = System.Data.CommandType.Text;

//            sqlCommand.CommandText = "SELECT * FROM AspNetUsers WHERE Id = @Id";

//            sqlCommand.Parameters.AddWithValue("@Id", id);

//            SqlDataReader reader = sqlCommand.ExecuteReader();

//            DataBaseUser user = new DataBaseUser();

//            DataBaseResultUser result = new DataBaseResultUser();

//            try
//            {
//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    if (reader["Id"].ToString() == id)
//                    {
//                        user.Id = reader["Id"].ToString();
//                        //user.FirstName = reader["FirstName"].ToString();
//                        //user.LastName = reader["LastName"].ToString();
//                        user.PhoneNumber = reader["PhoneNumber"].ToString();
//                        //user.Country = reader["Country"].ToString();
//                        //user.City = reader["City"].ToString();
//                        //user.PostalCode = reader["PostalCode"].ToString();
//                        //user.Street = reader["Street"].ToString();
//                        user.Role = reader["Role"].ToString();
//                        user.Approved = (bool)reader["Approved"];
//                        user.Email = reader["Email"].ToString();
//                        user.EmailConfirmed = (bool)reader["EmailConfirmed"];
//                        user.PhoneNumberConfirmed = (bool)reader["PhoneNumberConfirmed"];
//                        user.TwoFactorEnabled = (bool)reader["TwoFactorEnabled"];
//                        user.AccessFailedCount = (int)reader["AccessFailedCount"];
//                        user.UserName = reader["UserName"].ToString();
//                        user.Deleted = (bool)reader["Deleted"];

//                        result.Value = user;
//                        result.Message = "Querry successful";
//                        result.Success = true;
//                    }

//                    else
//                    {
//                        result.Message = "Querry unsuccessful";
//                        result.Success = false;

//                        return result;
//                    }
//                }

//                return result;

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static DataBaseResultUser ReadUserByUsername(string username)
//        {
//            List<string> userData = new List<string>();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            connection.Open();

//            sqlCommand.Connection = connection;

//            sqlCommand.CommandType = System.Data.CommandType.Text;

//            sqlCommand.CommandText = "SELECT * FROM AspNetUsers WHERE UserName = @UserName";

//            sqlCommand.Parameters.AddWithValue("@UserName", username);

//            SqlDataReader reader = sqlCommand.ExecuteReader();

//            DataBaseUser user = new DataBaseUser();

//            DataBaseResultUser result = new DataBaseResultUser();

//            try
//            {
//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    if (reader["UserName"].ToString() == username)
//                    {
//                        user.Id = reader["Id"].ToString();
//                        //user.FirstName = reader["FirstName"].ToString();
//                        //user.LastName = reader["LastName"].ToString();
//                        user.PhoneNumber = reader["PhoneNumber"].ToString();
//                        //user.Country = reader["Country"].ToString();
//                        //user.City = reader["City"].ToString();
//                        //user.PostalCode = reader["PostalCode"].ToString();
//                        //user.Street = reader["Street"].ToString();
//                        user.Role = reader["Role"].ToString();
//                        user.Approved = (bool)reader["Approved"];
//                        user.Email = reader["Email"].ToString();
//                        user.EmailConfirmed = (bool)reader["EmailConfirmed"];
//                        user.PhoneNumberConfirmed = (bool)reader["PhoneNumberConfirmed"];
//                        user.TwoFactorEnabled = (bool)reader["TwoFactorEnabled"];
//                        user.AccessFailedCount = (int)reader["AccessFailedCount"];
//                        user.UserName = reader["UserName"].ToString();
//                        user.Deleted = (bool)reader["Deleted"];

//                        result.Value = user;
//                        result.Message = "Querry successful";
//                        result.Success = true;
//                    }

//                    else
//                    {
//                        result.Message = "Querry unsuccessful";
//                        result.Success = false;

//                        return result;
//                    }
//                }

//                return result;

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static DataBaseResult ReadRole(string role)
//        {
//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            try
//            {
//                connection.Open();

//                sqlCommand.Connection = connection;

//                sqlCommand.CommandType = System.Data.CommandType.Text;

//                sqlCommand.CommandText = "SELECT * FROM AspNetRoles WHERE Name = @role";

//                sqlCommand.Parameters.AddWithValue("@role", role);

//                SqlDataReader reader = sqlCommand.ExecuteReader();

//                DataBaseResult result = new DataBaseResult();

//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    if (reader["Name"].ToString() == role)
//                    {
//                        result.Value = reader["Name"].ToString();
//                        result.Message = "Querry successful";
//                        result.Success = true;

//                        return result;
//                    }

//                    else
//                    {
//                        result.Message = "Querry unsuccessful";
//                        result.Success = false;

//                        return result;
//                    }
//                }

//                result.Message = "Querry unsuccessful";
//                result.Success = false;

//                return result;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static DataBaseResultUser UpdateDataBaseApproved(string email, bool approved)
//        {
//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommandUpdate = new SqlCommand();

//            SqlCommand sqlCommandSelect = new SqlCommand();

//            DataBaseResultUser result = new DataBaseResultUser();

//            connection.Open();

//            sqlCommandSelect.Connection = connection;

//            sqlCommandSelect.CommandType = System.Data.CommandType.Text;

//            sqlCommandSelect.CommandText = "SELECT * FROM AspNetUsers";

//            sqlCommandSelect.ExecuteNonQuery();

//            SqlDataReader reader = sqlCommandSelect.ExecuteReader();
//            try
//            {
//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    if (reader["Email"].ToString() == email)
//                    {
//                        if ((bool)reader["Approved"] == approved)
//                        {
//                            result.Message = $"User approved status is already: {(bool)reader["Approved"]}";
//                            result.Success = false;

//                            return result;
//                        }

//                        reader.Close();

//                        sqlCommandUpdate.Connection = connection;

//                        sqlCommandUpdate.CommandType = System.Data.CommandType.Text;

//                        sqlCommandUpdate.Parameters.AddWithValue("@email", email);

//                        sqlCommandUpdate.Parameters.Add(new SqlParameter("@approved", System.Data.SqlDbType.Bit)).Value = approved;

//                        sqlCommandUpdate.CommandText = "UPDATE AspNetUsers SET Approved = @approved WHERE Email = @email";

//                        sqlCommandUpdate.ExecuteNonQuery();

//                        sqlCommandUpdate.Parameters.Clear();

//                        //result.Value = ReadUser(email).Value;
//                        result.Message = "Querry successful";
//                        result.Success = true;

//                        return result;
//                    }
//                    else
//                    {
//                        result.Message = "Email does not exist";
//                        result.Success = false;

//                        return result;
//                    }
//                }

//                result.Message = "Querry unsuccessful";
//                result.Success = false;

//                return result;

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static DataBaseResultListOfUsers ReadAllUsers(string role)
//        {




//            List<DataBaseUser> users = new List<DataBaseUser>();

//            DataBaseResultListOfUsers result = new DataBaseResultListOfUsers();

//            var querry = "SELECT * FROM AspNetUsers where Role = @role";


//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand command = new SqlCommand(querry, connection);

//            command.Parameters.AddWithValue("@role", role);

//            try
//            {
//                connection.Open();

//                SqlDataReader reader = command.ExecuteReader();

//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    string email = reader["Email"].ToString();
//                    //users.Add(ReadUser(email).Value);
//                }

//                if (users != null)
//                {
//                    result.Value = users;
//                    result.Message = "Querry successful";
//                    result.Success = true;

//                    return result;
//                }

//                result.Message = "Querry unsuccessful";
//                result.Success = false;

//                return result;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static void EditUser(WebApiUser user)
//        {
//            List<WebApiUser> users = new List<WebApiUser>();

//            DataBaseResultListOfUsers result = new DataBaseResultListOfUsers();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            var querry = "UPDATE AspNetUsers SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Country = @Country, City = @City, PostalCode = @PostalCode, Street = @Street, Role = @Role, Approved = @Approved, Email = @Email, EmailConfirmed = @EmailConfirmed, PhoneNumberConfirmed = @PhoneNumberConfirmed, TwoFactorEnabled = @TwoFactorEnabled, UserName = @UserName, Deleted = @Deleted WHERE Id = @Id";

//            try
//            {
//                connection.Open();

//                sqlCommand.Connection = connection;

//                sqlCommand.CommandType = System.Data.CommandType.Text;

//                sqlCommand.Parameters.Add(ValueHandler("@Id", System.Data.SqlDbType.NVarChar, user.Id));
//                //sqlCommand.Parameters.Add(ValueHandler("@FirstName", System.Data.SqlDbType.NVarChar, user.FirstName));
//                //sqlCommand.Parameters.Add(ValueHandler("@LastName", System.Data.SqlDbType.NVarChar, user.LastName));
//                sqlCommand.Parameters.Add(ValueHandler("@PhoneNumber", System.Data.SqlDbType.NVarChar, user.PhoneNumber));
//                //sqlCommand.Parameters.Add(ValueHandler("@Country", System.Data.SqlDbType.NVarChar, user.Country));
//                //sqlCommand.Parameters.Add(ValueHandler("@City", System.Data.SqlDbType.NVarChar, user.City));
//                //sqlCommand.Parameters.Add(ValueHandler("@PostalCode", System.Data.SqlDbType.NVarChar, user.PostalCode));
//                //sqlCommand.Parameters.Add(ValueHandler("@Street", System.Data.SqlDbType.NVarChar, user.Street));
//                sqlCommand.Parameters.Add(ValueHandler("@Role", System.Data.SqlDbType.NVarChar, user.Role));
//                sqlCommand.Parameters.Add(ValueHandler("@Approved", System.Data.SqlDbType.NVarChar, user.Approved));
//                sqlCommand.Parameters.Add(ValueHandler("@Email", System.Data.SqlDbType.NVarChar, user.Email));
//                sqlCommand.Parameters.Add(ValueHandler("@EmailConfirmed", System.Data.SqlDbType.NVarChar, user.EmailConfirmed));
//                sqlCommand.Parameters.Add(ValueHandler("@PhoneNumberConfirmed", System.Data.SqlDbType.NVarChar, user.PhoneNumberConfirmed));
//                sqlCommand.Parameters.Add(ValueHandler("@TwoFactorEnabled", System.Data.SqlDbType.NVarChar, user.TwoFactorEnabled));
//                sqlCommand.Parameters.Add(ValueHandler("@UserName", System.Data.SqlDbType.NVarChar, user.UserName));
//                sqlCommand.Parameters.Add(ValueHandler("@Deleted", System.Data.SqlDbType.Bit, user.Deleted));

//                sqlCommand.CommandText = querry;

//                sqlCommand.ExecuteNonQuery();

//                sqlCommand.Parameters.Clear();
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static void EditUserPersonal(DataBaseUser user)
//        {
//            List<DataBaseUser> users = new List<DataBaseUser>();

//            DataBaseResultListOfUsers result = new DataBaseResultListOfUsers();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            var querry = "UPDATE AspNetUsers SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Country = @Country, City = @City, PostalCode = @PostalCode, Street = @Street, Email = @Email, TwoFactorEnabled = @TwoFactorEnabled, UserName = @UserName WHERE Id = @Id";

//            try
//            {
//                connection.Open();

//                sqlCommand.Connection = connection;

//                sqlCommand.CommandType = System.Data.CommandType.Text;

//                sqlCommand.Parameters.Add(ValueHandler("@Id", System.Data.SqlDbType.NVarChar, user.Id));
//                //sqlCommand.Parameters.Add(ValueHandler("@FirstName", System.Data.SqlDbType.NVarChar, user.FirstName));
//                //sqlCommand.Parameters.Add(ValueHandler("@LastName", System.Data.SqlDbType.NVarChar, user.LastName));
//                sqlCommand.Parameters.Add(ValueHandler("@PhoneNumber", System.Data.SqlDbType.NVarChar, user.PhoneNumber));
//                //sqlCommand.Parameters.Add(ValueHandler("@Country", System.Data.SqlDbType.NVarChar, user.Country));
//                //sqlCommand.Parameters.Add(ValueHandler("@City", System.Data.SqlDbType.NVarChar, user.City));
//                //sqlCommand.Parameters.Add(ValueHandler("@PostalCode", System.Data.SqlDbType.NVarChar, user.PostalCode));
//                //sqlCommand.Parameters.Add(ValueHandler("@Street", System.Data.SqlDbType.NVarChar, user.Street));
//                sqlCommand.Parameters.Add(ValueHandler("@Email", System.Data.SqlDbType.NVarChar, user.Email));
//                sqlCommand.Parameters.Add(ValueHandler("@TwoFactorEnabled", System.Data.SqlDbType.NVarChar, user.TwoFactorEnabled));
//                sqlCommand.Parameters.Add(ValueHandler("@UserName", System.Data.SqlDbType.NVarChar, user.UserName));


//                sqlCommand.CommandText = querry;

//                sqlCommand.ExecuteNonQuery();

//                sqlCommand.Parameters.Clear();
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static SqlParameter ValueHandler(string parameterName, System.Data.SqlDbType dbType, dynamic value)
//        {

//            if (value != null)
//            {
//                SqlParameter sqlParameter = new SqlParameter(parameterName, dbType);
//                sqlParameter.Value = value;
//                return sqlParameter;
//            }
//            else
//            {
//                return new SqlParameter(parameterName, DBNull.Value);
//            }
//        }


//        public static void DeleteUser(string id)
//        {
//            List<DataBaseUser> users = new List<DataBaseUser>();

//            DataBaseResultListOfUsers result = new DataBaseResultListOfUsers();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            var querry = "UPDATE AspNetUsers SET Deleted = @Deleted WHERE Id = @Id";

//            try
//            {
//                connection.Open();

//                sqlCommand.Connection = connection;

//                sqlCommand.CommandType = System.Data.CommandType.Text;

//                sqlCommand.Parameters.Add(new SqlParameter("@Deleted", System.Data.SqlDbType.Bit)).Value = true;
//                sqlCommand.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.NVarChar)).Value = id;

//                sqlCommand.CommandText = querry;

//                sqlCommand.ExecuteNonQuery();

//                sqlCommand.Parameters.Clear();
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static void RestoreUser(string id)
//        {
//            List<DataBaseUser> users = new List<DataBaseUser>();

//            DataBaseResultListOfUsers result = new DataBaseResultListOfUsers();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            var querry = "UPDATE AspNetUsers SET Deleted = @Deleted WHERE Id = @Id";

//            try
//            {
//                connection.Open();

//                sqlCommand.Connection = connection;

//                sqlCommand.CommandType = System.Data.CommandType.Text;

//                sqlCommand.Parameters.Add(new SqlParameter("@Deleted", System.Data.SqlDbType.Bit)).Value = false;
//                sqlCommand.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.NVarChar)).Value = id;

//                sqlCommand.CommandText = querry;

//                sqlCommand.ExecuteNonQuery();

//                sqlCommand.Parameters.Clear();
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }



//        //Store actions


//        public static DataBaseResultStore ReadStoreByUserName(string userName)
//        {
//            List<string> userData = new List<string>();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            connection.Open();

//            sqlCommand.Connection = connection;

//            sqlCommand.CommandType = System.Data.CommandType.Text;

//            sqlCommand.CommandText = "SELECT * FROM AspNetStores WHERE UserName = @userName";

//            sqlCommand.Parameters.AddWithValue("@userName", userName);

//            SqlDataReader reader = sqlCommand.ExecuteReader();

//            DataBaseStore store = new DataBaseStore();

//            DataBaseResultStore result = new DataBaseResultStore();

//            try
//            {
//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    if (reader["UserName"].ToString() == userName)
//                    {
//                        store.Id = reader["Id"].ToString();
//                        store.UserName = reader["UserName"].ToString();
//                        store.Email = reader["Email"].ToString();
//                        store.EmailConfirmed = (bool)reader["EmailConfirmed"];
//                        store.PhoneNumber = reader["PhoneNumber"].ToString();
//                        store.PhoneNumberConfirmed = (bool)reader["PhoneNumberConfirmed"];
//                        store.Country = reader["Country"].ToString();
//                        store.City = reader["City"].ToString();
//                        store.PostalCode = reader["PostalCode"].ToString();
//                        store.Street = reader["Street"].ToString();
//                        store.WorkingHoursWeek = reader["WorkingHoursWeek"].ToString();
//                        store.WorkingHoursWeekends = reader["WorkingHoursWeekends"].ToString();
//                        store.WorkingHoursHolidays = reader["WorkingHoursHolidays"].ToString();
//                        store.Role = reader["Role"].ToString();
//                        store.Approved = (bool)reader["Approved"];
//                        store.TwoFactorEnabled = (bool)reader["TwoFactorEnabled"];
//                        store.AccessFailedCount = (int)reader["AccessFailedCount"];
//                        store.MasterStoreID = reader["MasterStoreID"].ToString();
//                        store.Deleted = (bool)reader["Deleted"];

//                        result.Value = store;
//                        result.Message = "Querry successful";
//                        result.Success = true;
//                        result.Deleted = (bool)reader["Deleted"];
//                    }

//                    else
//                    {
//                        result.Message = "Querry unsuccessful";
//                        result.Success = false;

//                        return result;
//                    }
//                }

//                return result;

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }


//        public static DataBaseResultListOfStores ReadAllStores()
//        {
//            List<DataBaseStore> stores = new List<DataBaseStore>();

//            DataBaseResultListOfStores result = new DataBaseResultListOfStores();

//            var querry = "SELECT * FROM AspNetStores";

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand command = new SqlCommand(querry, connection);

//            try
//            {
//                connection.Open();

//                SqlDataReader reader = command.ExecuteReader();

//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    string email = reader["UserName"].ToString();
//                    stores.Add(ReadStoreByUserName(email).Value);
//                }

//                if (stores != null)
//                {
//                    result.Value = stores;
//                    result.Message = "Querry successful";
//                    result.Success = true;

//                    return result;
//                }

//                result.Message = "Querry unsuccessful";
//                result.Success = false;

//                return result;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static DataBaseResultListOfStores ReadAllStoresByMasterID(string masteStoreID)
//        {
//            List<DataBaseStore> stores = new List<DataBaseStore>();

//            DataBaseResultListOfStores result = new DataBaseResultListOfStores();

//            var querry = "SELECT * FROM AspNetStores WHERE MasterStoreID = @masterID";

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand command = new SqlCommand(querry, connection);

//            command.Parameters.AddWithValue("@masterID", masteStoreID);

//            try
//            {
//                connection.Open();

//                SqlDataReader reader = command.ExecuteReader();

//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    string userName = reader["UserName"].ToString();

//                    if (!ReadStoreByUserName(userName).Deleted)
//                    {
//                        stores.Add(ReadStoreByUserName(userName).Value);
//                    }
//                }

//                if (stores != null)
//                {
//                    result.Value = stores;
//                    result.Message = "Querry successful";
//                    result.Success = true;

//                    return result;
//                }

//                result.Message = "Querry unsuccessful";
//                result.Success = false;

//                return result;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static DataBaseResultListOfStores ReadDeletedStores(string masteStoreID)
//        {
//            List<DataBaseStore> stores = new List<DataBaseStore>();

//            DataBaseResultListOfStores result = new DataBaseResultListOfStores();

//            var querry = "SELECT * FROM AspNetStores WHERE MasterStoreID = @masterID";

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand command = new SqlCommand(querry, connection);

//            command.Parameters.AddWithValue("@masterID", masteStoreID);

//            try
//            {
//                connection.Open();

//                SqlDataReader reader = command.ExecuteReader();

//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    string userName = reader["UserName"].ToString();

//                    if (ReadStoreByUserName(userName).Deleted)
//                    {
//                        stores.Add(ReadStoreByUserName(userName).Value);
//                    }
//                }

//                if (stores != null)
//                {
//                    result.Value = stores;
//                    result.Message = "Querry successful";
//                    result.Success = true;

//                    return result;
//                }

//                result.Message = "Querry unsuccessful";
//                result.Success = false;

//                return result;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static DataBaseResultStore ReadStoreById(string id)
//        {
//            List<string> userData = new List<string>();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            connection.Open();

//            sqlCommand.Connection = connection;

//            sqlCommand.CommandType = System.Data.CommandType.Text;

//            sqlCommand.CommandText = "SELECT * FROM AspNetStores WHERE Id = @Id";

//            sqlCommand.Parameters.AddWithValue("@Id", id);

//            SqlDataReader reader = sqlCommand.ExecuteReader();

//            DataBaseStore store = new DataBaseStore();

//            DataBaseResultStore result = new DataBaseResultStore();

//            try
//            {
//                int count = reader.FieldCount;
//                while (reader.Read())
//                {
//                    if (reader["Id"].ToString() == id)
//                    {
//                        store.Id = reader["Id"].ToString();
//                        store.UserName = reader["UserName"].ToString();
//                        store.Email = reader["Email"].ToString();
//                        store.PhoneNumber = reader["PhoneNumber"].ToString();
//                        store.Country = reader["Country"].ToString();
//                        store.City = reader["City"].ToString();
//                        store.PostalCode = reader["PostalCode"].ToString();
//                        store.Street = reader["Street"].ToString();
//                        store.WorkingHoursWeek = reader["WorkingHoursWeek"].ToString();
//                        store.WorkingHoursWeekends = reader["WorkingHoursWeekends"].ToString();
//                        store.WorkingHoursHolidays = reader["WorkingHoursHolidays"].ToString();
//                        store.TwoFactorEnabled = (bool)reader["TwoFactorEnabled"];
//                        store.Deleted = (bool)reader["Deleted"];

//                        result.Value = store;
//                        result.Message = "Querry successful";
//                        result.Success = true;
//                    }

//                    else
//                    {
//                        result.Message = "Querry unsuccessful";
//                        result.Success = false;

//                        return result;
//                    }
//                }

//                return result;

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static void EditStore(DataBaseStore store)
//        {
//            List<DataBaseStore> stores = new List<DataBaseStore>();

//            DataBaseResultListOfStores result = new DataBaseResultListOfStores();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            var querry = "UPDATE AspNetStores SET UserName = @UserName, Email = @Email, PhoneNumber = @PhoneNumber, Country = @Country, City = @City, PostalCode = @PostalCode, Street = @Street, WorkingHoursWeek = @WorkingHoursWeek, WorkingHoursWeekends = @WorkingHoursWeekends, WorkingHoursHolidays = @WorkingHoursHolidays, TwoFactorEnabled = @TwoFactorEnabled WHERE Id = @Id";

//            try
//            {
//                connection.Open();

//                sqlCommand.Connection = connection;

//                sqlCommand.CommandType = System.Data.CommandType.Text;

//                sqlCommand.Parameters.Add(ValueHandler("@Id", System.Data.SqlDbType.NVarChar, store.Id));
//                sqlCommand.Parameters.Add(ValueHandler("@UserName", System.Data.SqlDbType.NVarChar, store.UserName));
//                sqlCommand.Parameters.Add(ValueHandler("@Email", System.Data.SqlDbType.NVarChar, store.Email));
//                sqlCommand.Parameters.Add(ValueHandler("@PhoneNumber", System.Data.SqlDbType.NVarChar, store.PhoneNumber));
//                sqlCommand.Parameters.Add(ValueHandler("@Country", System.Data.SqlDbType.NVarChar, store.Country));
//                sqlCommand.Parameters.Add(ValueHandler("@City", System.Data.SqlDbType.NVarChar, store.City));
//                sqlCommand.Parameters.Add(ValueHandler("@PostalCode", System.Data.SqlDbType.NVarChar, store.PostalCode));
//                sqlCommand.Parameters.Add(ValueHandler("@Street", System.Data.SqlDbType.NVarChar, store.Street));
//                sqlCommand.Parameters.Add(ValueHandler("WorkingHoursWeek", System.Data.SqlDbType.NVarChar, store.WorkingHoursWeek));
//                sqlCommand.Parameters.Add(ValueHandler("WorkingHoursWeekends", System.Data.SqlDbType.NVarChar, store.WorkingHoursWeekends));
//                sqlCommand.Parameters.Add(ValueHandler("WorkingHoursHolidays", System.Data.SqlDbType.NVarChar, store.WorkingHoursHolidays));
//                sqlCommand.Parameters.Add(ValueHandler("@TwoFactorEnabled", System.Data.SqlDbType.NVarChar, store.TwoFactorEnabled));

//                sqlCommand.CommandText = querry;

//                sqlCommand.ExecuteNonQuery();

//                sqlCommand.Parameters.Clear();
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        public static void DeleteStore(string id)
//        {
//            List<DataBaseStore> stores = new List<DataBaseStore>();

//            DataBaseResultListOfStores result = new DataBaseResultListOfStores();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            var querry = "UPDATE AspNetStores SET Deleted = @Deleted WHERE Id = @Id";

//            try
//            {
//                connection.Open();

//                sqlCommand.Connection = connection;

//                sqlCommand.CommandType = System.Data.CommandType.Text;

//                sqlCommand.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.NVarChar)).Value = id;
//                sqlCommand.Parameters.Add(new SqlParameter("@Deleted", System.Data.SqlDbType.Bit)).Value = true;

//                sqlCommand.CommandText = querry;

//                sqlCommand.ExecuteNonQuery();

//                sqlCommand.Parameters.Clear();
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }


//        }

//        public static void RestoreStore(string id)
//        {
//            List<DataBaseStore> stores = new List<DataBaseStore>();

//            DataBaseResultListOfStores result = new DataBaseResultListOfStores();

//            SqlConnection connection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand();

//            var querry = "UPDATE AspNetStores SET Deleted = @Deleted WHERE Id = @Id";

//            try
//            {
//                connection.Open();

//                sqlCommand.Connection = connection;

//                sqlCommand.CommandType = System.Data.CommandType.Text;

//                sqlCommand.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.NVarChar)).Value = id;
//                sqlCommand.Parameters.Add(new SqlParameter("@Deleted", System.Data.SqlDbType.Bit)).Value = false;

//                sqlCommand.CommandText = querry;

//                sqlCommand.ExecuteNonQuery();

//                sqlCommand.Parameters.Clear();
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }





















//        //public static DataBaseResult ReadEntityEmail(string email)
//        //{
            
//        //    using (var context = new ApplicationUserDbContext())
//        //    {
//        //        var user = (from u in context.Users where u.Email == email select u).FirstOrDefault();

//        //        return new DataBaseResult()
//        //        {
//        //            Value = user.Email,
//        //            Message = "Query successful",
//        //            Success = true
//        //        };
//        //    }
//        //}

//        //public static DataBaseResult ReadEntityUserName(string email)
//        //{
//        //    using (var context = new ApplicationUserDbContext())
//        //    {
//        //        var user = (from u in context.Users where u.Email == email select u).FirstOrDefault();

//        //        return new DataBaseResult()
//        //        {
//        //            Value = user.UserName,
//        //            Message = "Query successful",
//        //            Success = true
//        //        };
//        //    }
//        //}

//        //public static DataBaseResultUser ReadEntityUser(string email)
//        //{
//        //    using (var context = new ApplicationUserDbContext())
//        //    {
//        //        var user = (from u in context.Users where u.Email == email select u).FirstOrDefault();

//        //        var config = new MapperConfiguration(cfg => {
//        //            cfg.CreateMap<ApplicationUserDbContext, DataBaseUser>();
//        //        });

//        //        return new DataBaseResultUser()
//        //        {
                   
//        //        };
//        //    }
//        //}
//    }
//}