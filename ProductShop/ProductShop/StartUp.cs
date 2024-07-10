using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;


namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using (var db = new ProductShopContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                //Query 1. Import Users
                var usersJson = File.ReadAllText("./../../../Datasets/users.json");
                Console.WriteLine(ImportUsers(db, usersJson));

                //Query 2. Import Products
                var productsJson = File.ReadAllText("./../../../Datasets/products.json");
                Console.WriteLine(ImportProducts(db, productsJson));

                //Query 3. Import Categories
                var categoriesJson = File.ReadAllText("./../../../Datasets/categories.json");
                Console.WriteLine(ImportCategories(db, categoriesJson));

                //Query 4. Import Category Products
                var categoryProductsJson = File.ReadAllText("./../../../Datasets/categories-products.json");
                Console.WriteLine(ImportCategoryProducts(db, categoryProductsJson));

                //Query 5. Get Products in Range
                var productsInRangeJson = GetProductsInRange(db);
                File.WriteAllText("./../../../Results/products-in-range.json", productsInRangeJson);

                //Query 6. Get Sold Products
                var soldProductsJson = GetSoldProducts(db);
                File.WriteAllText("./../../../Results/users-sold-products.json", soldProductsJson);

                //Query 7. Get Categories By Products Count
                var categoriesByProductsCountJson = GetCategoriesByProductsCount(db);
                File.WriteAllText("./../../../Results/categories-by-products.json", categoriesByProductsCountJson);

                //Query 8. Get Users With Products
                //var usersWithProductsJson = GetUsersWithProducts(db);
                //File.WriteAllText("./../../../Results/users-and-products.json", usersWithProductsJson);
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);

            var validCategories = categories.Where(c => c.Name != null).ToList();

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();

            var productsInRangeJson = JsonConvert.SerializeObject(productsInRange, Formatting.Indented);

            return productsInRangeJson;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var usersSoldProducts = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(ps => ps.Buyer != null)
                        .Select(ps => new
                        {
                            name = ps.Name,
                            price = ps.Price,
                            buyerFirstName = ps.Buyer.FirstName,
                            buyerLastName = ps.Buyer.LastName
                        })
                        .ToList()
                })
                .ToList();

            var usersSoldProductsJson = JsonConvert.SerializeObject(usersSoldProducts, Formatting.Indented);

            return usersSoldProductsJson;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categoriesByProductsCount = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count,
                    averagePrice = c.CategoriesProducts.Average(cp => cp.Product.Price).ToString("F2"),
                    totalRevenue = c.CategoriesProducts.Sum(cp => cp.Product.Price).ToString("F2")
                })
                .OrderByDescending(c => c.productsCount)
                .ToList();

            var categoriesByProductsCountJson = JsonConvert.SerializeObject(categoriesByProductsCount, Formatting.Indented);

            return categoriesByProductsCountJson;
        }

        

    }
}