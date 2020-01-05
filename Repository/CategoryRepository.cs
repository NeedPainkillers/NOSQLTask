using Microsoft.Extensions.Options;
using NOSQLTask.Context;
using NOSQLTask.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NOSQLTask.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategory(int id);
        Task AddCategory(Category item);
        Task RemoveCategory(int id);
        Task UpdateCategory(int id, Category item);
    }
    public class CategoryRepository : ICategoryRepository
    {
        //TODO: postgres function needs to be created
        private readonly PostgresContext _context = null;

        public CategoryRepository(IOptions<Settings> settings)
        {
            _context = new PostgresContext(settings);
        }

        public async Task AddCategory(Category item)
        {

            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(string.Format("CALL insert_on_category(varchar '{0}', varchar '{0}');", item.Name, item.Description), connection);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            List<Category> Response = new List<Category>();
            await using (var cmd = new NpgsqlCommand("SELECT t.* FROM public.\"category\" t ORDER BY category_id ASC", connection))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Response.Add(new Category()
                    {
                        CategoryId = reader.GetValue(0).ToString(),
                        Name = reader.GetValue(1).ToString()
                    });
                }
            return Response;
        }

        public async Task<Category> GetCategory(int id)
        {
            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(String.Format("SELECT t.* FROM public.\"category\" t WHERE t.category_id = {0}", id), connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            return new Category()
            {
                CategoryId = reader.GetValue(0).ToString(),
                Name = reader.GetValue(1).ToString()
            };
        }

        public async Task RemoveCategory(int id)
        {
            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand("CALL delete_on_category((@id));", connection);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateCategory(int id, Category item)
        {
            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(string.Format("CALL update_on_category((@id), varchar '{0}', varchar '{0}');", item.Name, item.Description), connection);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
