using NOSQLTask.Data;
using NOSQLTask.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using StackExchange.Redis;
using Microsoft.Extensions.Options;

namespace NOSQLTask.Repository
{
    /// <summary>
    /// Products are stored at Redis
    /// 
    /// Keys for product are created using this pattern ->
    /// Product:<id>:<field>
    /// 
    /// </summary>
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProduct(string key);
        Task AddProduct(Product item);
        Task RemoveProduct(string id);
        Task UpdateProduct(string key, Product item);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly RedisContext _context = null;

        public ProductRepository(IOptions<Settings> settings)
        {
            _context = new RedisContext(settings);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            IDatabase database = _context.Connection.GetDatabase();

            List<Product> products = new List<Product>();

            foreach (var ep in _context.Connection.GetEndPoints())
            {
                var server = _context.Connection.GetServer(ep);
                var keys = server.Keys(database: database.Database, pattern: "Product:[0-9]*[^:]$");

                foreach (var key in keys)
                {
                    var keyS = key.ToString();

                    var id = Task.Run(() => database.StringGetAsync(key));
                    var specs = Task.Run(() => database.StringGetAsync(keyS + ":specs"));
                    var priceS = Task.Run(() => database.StringGetAsync(keyS + ":price"));
                    var quantityS = Task.Run(() => database.StringGetAsync(keyS + ":quantity"));

                    if (!(await priceS).TryParse(out int price))
                        price = -1;
                    if (!(await quantityS).TryParse(out int quantity))
                        quantity = -1;

                    var product = new Product()
                    {
                        ProductId = await id,
                        Specification = await specs,
                        Price = price,
                        Quantity = quantity
                    };

                    products.Add(product);
                }

            }

            return products;
        }

        public async Task<Product> GetProduct(string ProductId)
        {
            IDatabase database = _context.Connection.GetDatabase();

            var key = "Product:" + ProductId;

            var id = Task.Run(() => database.StringGetAsync(key));
            var specs = Task.Run(() => database.StringGetAsync(key + ":specs"));
            var priceS = Task.Run(() => database.StringGetAsync(key + ":price"));
            var quantityS = Task.Run(() => database.StringGetAsync(key + ":quantity"));
            var type = Task.Run(() => database.StringGetAsync(key + ":type"));
            var category = Task.Run(() => database.StringGetAsync(key + ":category"));

            (await priceS).TryParse(out int price);
            (await quantityS).TryParse(out int quantity);

            Product product = new Product()
            {
                ProductId = await id,
                Specification = await specs,
                Price = price,
                Quantity = quantity,
                Category = await category,
                Type = await type
            };

            return product;
        }

        public async Task AddProduct(Product item)
        {
            IDatabase database = _context.Connection.GetDatabase();
            string pattern = "Product:" + item.ProductId.Trim();
            RedisKey key = new RedisKey().Append(pattern);

            //int max = 0;
            //foreach (var ep in _context.Connection.GetEndPoints())
            //{
            //    var server = _context.Connection.GetServer(ep);
            //    var keys = server.Keys(database: database.Database, pattern: "Product:[0-9]*[^:]$");
            //    if (keys.Any())
            //        max = (from k in keys
            //               let x = Regex.Match(k, @"\d+").Value
            //               select Int32.Parse(x))
            //                 .OrderByDescending(m => m)
            //                 .FirstOrDefault();
            //}
            //max++;
            //key.Append(max.ToString());

            await database.StringSetAsync(key, item.ProductId);
            await database.StringSetAsync(key.Append(":price"), item.Price);
            await database.StringSetAsync(key.Append(":quantity"), item.Quantity);
            await database.StringSetAsync(key.Append(":specs"), item.Specification);
            await database.StringSetAsync(key.Append(":type"), item.Type);
            await database.StringSetAsync(key.Append(":category"), item.Category);
        }


        public async Task RemoveProduct(string id)
        {
            IDatabase database = _context.Connection.GetDatabase();

            foreach (var ep in _context.Connection.GetEndPoints())
            {
                var server = _context.Connection.GetServer(ep);
                var pattern = string.Format("Product:{0}:*", id);
                var keys = server.Keys(database: database.Database, pattern: pattern).ToArray();

                await database.KeyDeleteAsync(keys);
            }
        }

        public Task UpdateProduct(string key, Product item)
        {
            throw new NotImplementedException();
        }
    }
}
