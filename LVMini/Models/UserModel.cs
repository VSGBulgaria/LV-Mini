using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LVMini.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }


        public static async Task<UserModel> CurrentLoggedUser(string username)
        {
            using (HttpClient client = new HttpClient())
            {
                var httpResponseMessage = await client.GetAsync($"http://localhost:53920/api/users/" + username);

                var data = httpResponseMessage.Content.ReadAsStringAsync().Result;

                var list = JsonConvert.DeserializeObject<List<UserModel>>(data);
                var user = list[0];

                var model = new UserModel()
                {
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    Role = user.Role
                };

                return model;
            }
        }
    }
}
