using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBProds.Models;
using System.Data;
using System.Configuration;

namespace ProductsAPI.Controllers
{
    public class ProductsController : ApiController
    {
        //Probably a database in a real scenario...
        public String getConnectionString()
        {
            /*var uriString = ConfigurationManager.AppSettings["4db8c604-d7f7-4f44-a85e-a81900fa0271.sqlserver.sequelizer.com"];
            var uri = new Uri(uriString);
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = uri.Host,
                InitialCatalog = uri.AbsolutePath.Trim('/'),
                //UserID = uri.UserInfo.Split(':').First(),
                UserID = "ycuybrshuiebsfdm",
                //Password = uri.UserInfo.Split(':').Last(),
                Password = "jYHo42aBzidnUTZ5asAMSrnppLYaYWiPvSyanaMeftE3KDEdAnczTCTZLs8qb8KL",
            }.ConnectionString;
            return connectionString;
            */
            string connectionStr = @"Server=4db8c604-d7f7-4f44-a85e-a81900fa0271.sqlserver.sequelizer.com;Database=db4db8c604d7f74f44a85ea81900fa0271;User ID=ycuybrshuiebsfdm;Password=jYHo42aBzidnUTZ5asAMSrnppLYaYWiPvSyanaMeftE3KDEdAnczTCTZLs8qb8KL";
            return connectionStr;
        }

        

        List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };

       // public string ConnectionString { get => connectionString; set => connectionString = value; }
        //public Uri Uri { get => uri; set => uri = value; }

        public IEnumerable<Product> GetAllProducts()
        {
            List<Product> allProducts = new List<Product>();
            SqlConnection connection = new SqlConnection(getConnectionString());
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Prods ORDER BY Id", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Product product = new Product();
                product.Id = (int)reader["Id"];
                product.Name = (string)reader["Name"];
                product.Category = (reader["Category"] == DBNull.Value) ? "" : (string)reader["Category"];
                product.Price = (reader["Price"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["Price"]);
                allProducts.Add(product);
            }
            connection.Close();
            reader.Close();
            return allProducts;
        }

        [Route("api/products/{id:int}")] //specifies that the id parameter is an integer
        public IHttpActionResult GetProduct(int id)
         {
            SqlConnection connec = new SqlConnection(getConnectionString());
            connec.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Prods WHERE Id ="+id,connec);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            Product product = new Product();
            product.Id = (int)reader["Id"];
            product.Name = (string)reader["Name"];
            product.Category = (reader["Category"] == DBNull.Value) ? "" : (string)reader["Category"];
            product.Price = (reader["Price"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["Price"]);
           
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product); //Respecting HTTP errors (200 OK)
        }

        [Route("api/products/{category}")]
        public IEnumerable<Product> GetProductByCategory(string category)
        {
            List<Product> listProds = new List<Product>();
            foreach (Product prod in products)
            {
                if (prod.Category == category)
                {
                    listProds.Add(prod);
                }
            }
            if (listProds == null)
            {
                return listProds;
            }
            else
            {
                return listProds; //Respecting HTTP errors (200 OK)
            }
        }
        // POST api/<controller> //our controller is products
        public IHttpActionResult PostProduct(Product p)
        {
            
            SqlConnection connec = new SqlConnection(getConnectionString());
            connec.Open();
            SqlCommand command = new SqlCommand("INSERT INTO Prods (NAME,CATEGORY,PRICE) VALUES (@Name,@Category,@Price)", connec);
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.AddWithValue("@Name", p.Name);
            command.Parameters.AddWithValue("@Category", p.Category);
            command.Parameters.AddWithValue("@Price", p.Price);
            int numRows = command.ExecuteNonQuery();
            connec.Close();
            return Ok(numRows);

        }
        // PUT api/<controller>/5
        public IHttpActionResult PutProduct(Product p)
        {
            SqlConnection connec = new SqlConnection(getConnectionString());
            connec.Open();
            SqlCommand command = new SqlCommand("UPDATE Prods SET NAME=@Name,CATEGORY=@Category,Price=@Price WHERE Id=@Id", connec);
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.AddWithValue("@Id", p.Id);
            command.Parameters.AddWithValue("@Name", p.Name);
            command.Parameters.AddWithValue("@Category", p.Category);
            command.Parameters.AddWithValue("@Price", p.Price);
            int numRows = command.ExecuteNonQuery();
            connec.Close();
            return Ok(numRows);
        }
        // DELETE api/<controller>/5
        [HttpDelete]
        [Route("api/Products/{id:int}")]
        public IHttpActionResult DeleteProduct(int id)
        {
            SqlConnection connec = new SqlConnection(getConnectionString());
            connec.Open();
            SqlCommand cmd= new SqlCommand("DELETE FROM Prods WHERE Id=@Id",connec);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@Id", id);
            int numRows = cmd.ExecuteNonQuery();
            connec.Close();
            return Ok(numRows);
        }
    }
}
