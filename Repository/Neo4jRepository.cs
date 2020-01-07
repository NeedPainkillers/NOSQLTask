using Microsoft.Extensions.Options;
using NOSQLTask.Context;
using NOSQLTask.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Repository
{
    public interface INeo4jRepository
    {
        Task<IEnumerable<ProductNeo4j>> GetProducts(string invoiceid);
        Task<IEnumerable<Order>> GetOrders(int ClientId);
        Task AddData(int ClientId, string ProductId, Category category, string invoiceId, int orderId);
    }
    public class Neo4jRepository : INeo4jRepository
    {
        private const string ClientFormat = "merge (c:Client:Client {{ClientId:{0}}})";
        private const string OrderFormat = "merge (o:Order:Order {{OrderId:{0}, InvoiceId: {1}}})";
        private const string ProductFormat = "merge (p:Product:Product {{ProductId:{0}}})";
        private const string CategoryId = "merge (ca:Category:Test {{CategoryId: {0}}})";
        private readonly Neo4jContext _context = null;
        public Neo4jRepository(IOptions<Settings> settings)
        {
            _context = new Neo4jContext(settings);
        }


        public async Task AddData(int ClientId, string ProductId, Category category, string invoiceId, int orderId)
        {
            //TODO:
            /*
            merge (c:Client:Client {ClientId:10})
            merge (o:Order:Order {OrderId:10, InvoiceId: "TEST"})
            merge (p:Product:Product {ProductId:"1"})
            merge (ca:Category:Test {CategoryId: 10})
            merge (c)-[:ORDERED]->(o)
            merge (o)-[:INCLUDED]->(p)
            merge (p)-[:TYPE_OF]->(ca)
            merge (ca)-[:OF_TYPE]->(p)
            */
            if (await _context.Connect())
                await _context.GetCypher
                     .Merge(string.Format(ClientFormat, ClientId))
                     .Merge(string.Format(OrderFormat, orderId, invoiceId))
                     .Merge(string.Format(ProductFormat, ProductId))
                     .Merge(string.Format(CategoryId, category.CategoryId))
                     .Merge("merge (c)-[:ORDERED]->(o)")
                     .Merge("merge (o)-[:INCLUDED]->(p)")
                     .Merge("merge (p)-[:TYPE_OF]->(ca)")
                     .Merge("merge (ca)-[:OF_TYPE]->(p)")
                     .ExecuteWithoutResultsAsync();
            else
            {
                throw new Exception("Connection with Neo4j failed");
            }

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetOrders(int ClientId)
        {
            if (await _context.Connect())
                return (await _context.GetCypher
                    .Match("(client:Client)--(order:Order)")
                    .Where((Client client) => client.ClientId.Equals(ClientId))
                    //.Where(string.Format("client.ClientId = {0}", ClientId))
                    .Return(order => order.As<Order>())
                    .ResultsAsync).Take(10);
            else
            {
                throw new Exception("Connection with Neo4j failed");
            }
        }

        public async Task<IEnumerable<ProductNeo4j>> GetProducts(string InvoiceId)
        {
            if (await _context.Connect())
                return (await _context.GetCypher
                    .Match("(order:Order)--(product:Products)--(category:Category)--(otherProduct:Products)")
                    //.Where((Invoice invoice) => invoices.Contains(invoice.InvoiceId))
                    .Where((Order order) => order.InvoiceId.Equals(InvoiceId))
                    //.Where(string.Format("order.InvoiceId = {0}", InvoiceId))
                    .AndWhere("otherProduct.ProductId != product.ProductId")
                    .Return(product => product.As<ProductNeo4j>())
                    .ResultsAsync).Take(10);
            else
            {
                throw new Exception("Connection with Neo4j failed");
            }
        }
    }
}
