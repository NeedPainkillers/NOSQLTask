using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using NOSQLTask.Data;
using NOSQLTask.Context;

namespace NOSQLTask.Repository
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllClients();
        Task<Client> GetClient(int id);
        Task AddClient(Client item);
        Task RemoveClient(int id);
        Task UpdateClient(int id, Client item);
    }
    public class ClientRepository : IClientRepository
    {
        //TODO: postgres function needs to be created
        private readonly PostgresContext _context = null;

        public ClientRepository(IOptions<Settings> settings)
        {
            _context = new PostgresContext(settings);
        }

        public async Task AddClient(Client item)
        {

            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(string.Format("CALL insert_on_client(text '{0}');", item.Name), connection);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            List<Client> Response = new List<Client>();
            await using (var cmd = new NpgsqlCommand("SELECT t.* FROM public.\"client\" t ORDER BY client_id ASC", connection))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Response.Add(new Client()
                    {
                        ClientId = reader.GetInt32(0),
                        Name = reader.GetValue(1).ToString()
                    });
                }
            return Response;
        }

        public async Task<Client> GetClient(int id)
        {
            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(String.Format("SELECT t.* FROM public.\"client\" t WHERE t.client_id = {0}", id), connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();
            return new Client()
            {
                ClientId = reader.GetInt32(0),
                Name = reader.GetValue(1).ToString()
            };
        }

        public async Task RemoveClient(int id)
        {
            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand("CALL delete_on_client((@id));", connection);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateClient(int id, Client item)
        {
            var connection = _context.GetConnection;

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(string.Format("CALL update_on_client((@id), varchar '{0}');", item.Name), connection);
            cmd.Parameters.AddWithValue("id", item.ClientId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
