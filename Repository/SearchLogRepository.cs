using System;
using NOSQLTask.Context;
using NOSQLTask.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Microsoft.Extensions.Options;

namespace NOSQLTask.Repository
{
    public interface ISearchLogRepository
    {
        Task<IEnumerable<SearchLog>> GetAllLogs(int id);
        Task<SearchLog> GetLog(string key);
        Task AddLog(SearchLog item);
        Task RemoveLog(string id);
        Task UpdateLog(string key, SearchLog item);
    }

    public class SearchLogRepository : ISearchLogRepository
    {
        private readonly ESContext _context = null;

        public SearchLogRepository(IOptions<Settings> settings)
        {
            _context = new ESContext(settings);
        }

        public async Task AddLog(SearchLog item)
        {
            var response = await _context.Connection.IndexAsync(item, idx => idx.Index("ContextIdx"));
        }

        public Task<IEnumerable<SearchLog>> GetAllLogs(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<SearchLog> GetLog(string key)
        {
            var response = await _context.Connection.GetAsync<SearchLog>(1, idx => idx.Index("ContextIdx"));
            var log = response.Source; // the original document

            return log;
        }

        public Task RemoveLog(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLog(string key, SearchLog item)
        {
            throw new NotImplementedException();
        }
    }
}
