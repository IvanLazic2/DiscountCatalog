using AbatementHelper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.Classes.Repositories
{
    public class ProductRepository
    {
        public static bool AddProductToDataBase(Product product)
        {
            var connectionString = "Data Source=MyNameJeff;Initial Catalog=AbatementHelper.DataBase;Integrated Security=True";

            var query = "INSERT INTO ProductData (ProductName,CompanyName,ProductOldPrice,ProductNewPrice,ProductAbatementDateBegin,ProductAbatementDateEnd,ProductNote) VALUES ('@ProductName','@CompanyName','@ProductOldPrice','@ProductNewPrice','@ProductAbatementDateBegin','@ProductAbatementDateBegin','@ProductNote')";

            query = query.Replace("@ProductName", product.ProductName).
                          Replace("@CompanyName", product.CompanyName).
                          Replace("@ProductOldPrice", product.ProductOldPrice).
                          Replace("@ProductNewPrice", product.ProductNewPrice).
                          Replace("@ProductAbatementDateBegin", product.ProductAbatementDateBegin).
                          Replace("@ProductAbatementDateEnd", product.ProductAbatementDateEnd).
                          Replace("@ProductNote", product.ProductNote);

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
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
