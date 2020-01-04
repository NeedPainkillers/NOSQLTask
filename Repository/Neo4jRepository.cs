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
        Task AddData();
    }
    public class Neo4jRepository : INeo4jRepository
    {
        private readonly Neo4jContext _context = null;
        public Neo4jRepository(IOptions<Settings> settings)
        {
            _context = new Neo4jContext(settings);
        }

        public Task AddData()
        {
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
