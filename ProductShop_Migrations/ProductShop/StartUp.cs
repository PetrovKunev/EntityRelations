using ProductShop.Data;
using ProductShop.Models;
using Newtonsoft;


namespace ProductShop
{
    using Newtonsoft;
    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext db = new ProductShopContext();

            string userText = File.ReadAllText("../../../Datasets/users.json");

            Console.WriteLine(ImportUsers(db, userText));
            
        }


        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson); 

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}