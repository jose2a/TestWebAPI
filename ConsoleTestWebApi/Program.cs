using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestWebApi
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            RunAsync().Wait();
            
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:9220/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var contacts = GetContactAsync("contact").Result as List<Model.Contact>;
                foreach (var c in contacts)
                {
                    Console.WriteLine(c.LastName + ", " + c.FirstName);
                }

                var newContact = new Model.Contact
                {
                    FirstName = "Oliver",
                    LastName = "Queen"
                };

                var uri = CreateContactAsync(newContact);
                Console.WriteLine(uri.Result.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task<List<Model.Contact>> GetContactAsync(string path)
        {
            //var contact;

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Model.Contact>>();
            }
            return null;
        }

        static async Task<Uri> CreateContactAsync(Model.Contact contact)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("contact", contact);
            response.EnsureSuccessStatusCode();

            if(response.IsSuccessStatusCode)
            {
                var ct = await response.Content.ReadAsAsync<Model.Contact>();
                Console.WriteLine(ct.LastName + ", " + ct.FirstName);
            }

            // return URI of the created resource.
            return response.Headers.Location;
        }

    }
}
