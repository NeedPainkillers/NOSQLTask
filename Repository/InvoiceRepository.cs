
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NOSQLTask.Data;
using NOSQLTask.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Repository
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice>> GetAllInvoices();
        Task<IEnumerable<Invoice>> GetAllInvoicesByClient(int ClientId);
        Task<IEnumerable<int>> GetClientsIdByProduct(string ProductId);
        Task<Invoice> GetInvoice(string id);
        Task AddInvoice(Invoice item);
        Task<DeleteResult> RemoveInvoice(string id);
        Task<UpdateResult> UpdateInvoice(string id, Invoice item);
    }

    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly MongoContext _context = null;

        public InvoiceRepository(IOptions<Settings> settings)
        {
            _context = new MongoContext(settings);
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoices()
        {
            return await _context.Invoices.Find(_ => true).ToListAsync();
        }

        public async Task<Invoice> GetInvoice(string id)
        {
            var filter = Builders<Invoice>.Filter.Eq("InvoiceId", id);
            return await _context.Invoices
                            .Find(filter)
                            .FirstOrDefaultAsync();
        }

        public async Task AddInvoice(Invoice item)
        {
            await _context.Invoices.InsertOneAsync(item);
        }

        public async Task<DeleteResult> RemoveInvoice(string id)
        {
            return await _context.Invoices.DeleteOneAsync(
                 Builders<Invoice>.Filter.Eq("InvoiceId", id));
        }

        public async Task<UpdateResult> UpdateInvoice(string id, Invoice item)
        {
            var filter = Builders<Invoice>.Filter.Eq(s => s.InvoiceId, id);
            var update = Builders<Invoice>.Update
                            .Set(s => s.ClientId, item.ClientId)
                            .Set(s => s.Status, item.Status)
                            .Set(s => s.ProductIds, item.ProductIds);

            return await _context.Invoices.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesByClient(int ClientId)
        {
            var filter = Builders<Invoice>.Filter.Eq("ClientId", ClientId) & Builders<Invoice>.Filter.Eq("Status", true);
            return await _context.Invoices
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetClientsIdByProduct(string ProductId)
        {
            var filter = Builders<Invoice>.Filter.ElemMatch(x => x.ProductIds, x => x.Equals(ProductId));

            var r = await _context.Invoices
                            .Find(filter)
                            .ToListAsync();

            return (from item in r 
                    select item.ClientId).Take(2);
        }
    }
}
