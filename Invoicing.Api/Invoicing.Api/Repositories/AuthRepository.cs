using System.Data;
using Microsoft.Data.SqlClient;
using Invoicing.Api.Models;
using Invoicing.Api.Database;

namespace Invoicing.Api.Repositories
{
    public class AuthRepository
    {
        private readonly DbHelper _db;

        public AuthRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<ApiUser?> GetUserByApiKeyAsync(Guid apiKey)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT ApiUserId, Username, ApiKey, IsActive, CreatedAt
                FROM ApiUsers
                WHERE ApiKey = @key AND IsActive = 1
            ", conn);

            cmd.Parameters.AddWithValue("@key", apiKey);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ApiUser
                {
                    ApiUserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    ApiKey = reader.GetGuid(2),
                    IsActive = reader.GetBoolean(3),
                    CreatedAt = reader.GetDateTime(4)
                };
            }

            return null;
        }
    }
}
