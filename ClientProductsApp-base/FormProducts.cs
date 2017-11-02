using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; //Stream
using System.Linq;
using System.Net; //HttpWebRequest
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;

//JavaScriptSerializer --> necessário criar referencia para System.Web.Extensions

namespace ClientProductsApp
{
    public partial class FormProducts : Form
    {

        public static string baseURI = @"http://restproductslab.apphb.com/api/products"; //needs to be updated!
        public static HttpClient client = new HttpClient();

    
    
        public FormProducts()
        {
            InitializeComponent();
        }

        private void buttonGetAll_Click(object sender, EventArgs e)
        {

            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(baseURI);
                richTextBoxShowProducts.Text = json;
            }

        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            Product newProduct = new Product
            {
                Name = textBoxName.Text,
                Category = textBoxCategory.Text,
                Price = decimal.Parse(textBoxPrice.Text)
            };
            CreateProductAsync(newProduct);

        }
        static async Task RunAsync()
        {
            // New code:
            client.BaseAddress = new Uri("http://restproductslab.apphb.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.ReadLine();
        }
        static async Task<Uri> CreateProductAsync(Product product)
        {
            // HttpResponseMessage response = await client.PostAsJsonAsync("api/products", product);
            // response.EnsureSuccessStatusCode();
            var response = await client.PostAsync("http://restproductslab.apphb.com/api/products",
            new StringContent(product.ToString(), Encoding.UTF8, "application/json"));
            // Return the URI of the created resource.
            MessageBox.Show(response.Headers.Location.ToString());
            return response.Headers.Location;
            
        }
    }
}
