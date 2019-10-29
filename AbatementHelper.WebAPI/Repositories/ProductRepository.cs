using AbatementHelper.CommonModels.Models;
using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace AbatementHelper.WebAPI.Repositories
{
    public class ProductRepository
    {

        private static string ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public static bool AddProductToDataBase(Product product)
        {
            

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand command = new SqlCommand("AddProduct", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@CompanyName", product.CompanyName);
            command.Parameters.AddWithValue("@StoreName", product.StoreName);
            command.Parameters.AddWithValue("@ProductOldPrice", product.ProductOldPrice);
            command.Parameters.AddWithValue("@ProductNewPrice", product.ProductNewPrice);
            command.Parameters.AddWithValue("@ProductAbatementDateBegin", product.ProductAbatementDateBegin);
            command.Parameters.AddWithValue("@ProductAbatementDateEnd", product.ProductAbatementDateEnd);
            command.Parameters.AddWithValue("@ProductNote", product.ProductNote);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}