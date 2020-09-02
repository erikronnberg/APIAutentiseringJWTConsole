using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APIAutentiseringJWTConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await AuthUser();

            Auth auth = JsonConvert.DeserializeObject<Auth>(result);

            var x = await GetUserAsync(auth.Token);

            Console.WriteLine(x);
            Console.ReadKey();
        }

        public static async Task<string> AuthUser()
        {
            var user = new User();
            user.Username = "test";
            user.Password = "test";

            var url = "http://localhost:4000/users/authenticate";

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        public static async Task<string> GetUserAsync(string token)
        {
            var url = "http://localhost:4000/users/";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(url);

            string result = response.Content.ReadAsStringAsync().Result;

            return result;
        }
    }
}
