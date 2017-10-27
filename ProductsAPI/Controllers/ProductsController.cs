using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductsAPI.Models;
namespace ProductsAPI.Controllers
{
    public class ProductsController : ApiController
    {
        //Probably a database in a real scenario...
        string conectionSTR = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\migue\Dropbox\IS\
        RestClassProj\RestClassProject\DBProds\App_Data\Database1.mdf;Integrated Security=True";


        List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };
        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        [Route("api/products/{id:int}")] //specifies that the id parameter is an integer
        public IHttpActionResult GetProduct(int id)
         {
            var product = products.FirstOrDefault((p) => p.Id == id);
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
    }
}
